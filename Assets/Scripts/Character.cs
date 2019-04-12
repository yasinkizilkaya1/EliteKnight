using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constants

    private const string CARD_DATA_BILL = ".asset";
    private const string CARD_DATA_PATH = "Assets/Data/Characters/";
    private const string TAG_ITEM = "Item";

    #endregion

    #region Fields

    public CharacterData characterData;
    public GameManager gameManager;
    public UIManager UIManager;
    public Knife knife;
    public Gun Gun;

    public int CurrentHP;
    public int MaxHP;
    public int CurrentDefence;
    public int Speed;
    public int Power;
    public int CharacterWay;
    public int run;
    public int Energy;
    public int MaxDefance;
    public int DeadEnemyCount;
    private int mDefaultSpeed;
    private int mRunSpeed;

    public bool IsNewGun;
    public bool isDead;
    public bool isTire;

    public float EnergyReload;
    public float shooting;

    private Vector3 mMousePositionVector;
    public GameObject BodyObject;
    public GameObject RightWeaponObject;
    public List<Gun> Guns;
    private List<Key> Keys;
    public int SelectionWeaponId;

    private MoveForward MoveForward;
    private MoveReserve MoveReserve;
    private MoveRight MoveRight;
    private MoveLeft MoveLeft;
    private FireWeapon FireWeapon;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (gameManager != null)
        {
            Moving(gameManager);
            Run(gameManager);
            CharacterTurn(gameManager);
            Attack();


            if (Input.GetAxis("Mouse ScrollWheel") > 0f && Guns.Count > 1)
            {
                if (SelectionWeaponId < Guns.Count - 1)
                {
                    SelectionWeaponId++;
                }
                else
                {
                    SelectionWeaponId = 0;
                }
                gameManager.GunSlot.GunChange(Guns[SelectionWeaponId]);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Guns.Count > 1)
            {
                if (SelectionWeaponId > 0)
                {
                    SelectionWeaponId--;
                }
                else
                {
                    SelectionWeaponId = Guns.Count - 1;
                }
                gameManager.GunSlot.GunChange(Guns[SelectionWeaponId]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_ITEM))
        {
            Destroy(collider.gameObject);
            gameManager.GunSlot.GunAdd(collider.GetComponent<Gun>().weapon, RightWeaponObject);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        UIManager = gameManager.UIManager;
        Keys = gameManager.KeySettings.Keys;

        characterData = gameManager.CharacterData;
        name = characterData.Name;
        CurrentHP = characterData.Health;
        MaxHP = CurrentHP;
        Energy = characterData.Energy;
        CurrentDefence = characterData.Defence;
        Power = characterData.Power;
        Speed = characterData.Speed;
        mDefaultSpeed = characterData.Speed;
        EnergyReload = characterData.EnergyReloadTime;
        mRunSpeed = characterData.RunSpeed;
        MaxDefance = characterData.Defence;

        MoveForward = new MoveForward();
        MoveReserve = new MoveReserve();
        MoveRight = new MoveRight();
        MoveLeft = new MoveLeft();
        FireWeapon = new FireWeapon();
        MoveForward.Character = this;
        MoveReserve.Character = this;
        MoveRight.Character = this;
        MoveLeft.Character = this;
        FireWeapon.Character = this;
    }

    private void Moving(GameManager GameManager)
    {
        if (GameManager.isPause == false)
        {
            if (Input.GetKey(Keys[0].CurrentKey))
            {
                MoveForward.Execute();
            }

            if (Input.GetKey(Keys[1].CurrentKey))
            {
                MoveReserve.Execute();
            }

            if (Input.GetKey(Keys[2].CurrentKey))
            {
                MoveRight.Execute();
            }

            if (Input.GetKey(Keys[3].CurrentKey))
            {
                MoveLeft.Execute();
            }

            if (Input.GetKey(Keys[0].CurrentKey) == false && Input.GetKey(Keys[1].CurrentKey) == false && Input.GetKey(Keys[2].CurrentKey) == false && Input.GetKey(Keys[3].CurrentKey) == false)
            {
                CharacterWay = 0;
            }
        }
    }

    private void CharacterTurn(GameManager gameManager)
    {
        if (gameManager.isPause == false)
        {
            mMousePositionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mMousePositionVector.z = transform.position.z;
            BodyObject.transform.right = (mMousePositionVector - transform.position);
        }
    }

    private void Run(GameManager gameManager)
    {
        if (gameManager.isPause == false)
        {
            if (Input.GetKey(Keys[4].CurrentKey))
            {
                if (Energy > 0)
                {
                    Speed = mRunSpeed;
                    Energy -= characterData.EnergyIncreaseAmmount;
                    run = 1;
                }
                else
                {
                    Speed = mDefaultSpeed;
                    isTire = true;

                    if (isTire == true && EnergyReload > 0)
                    {
                        EnergyReload -= Time.deltaTime;
                        run = 0;
                    }
                }
            }
            else
            {
                Speed = mDefaultSpeed;
                run = 0;

                if (isTire == true && EnergyReload > 0)
                {
                    EnergyReload -= Time.deltaTime;
                    run = 0;
                }

                if (EnergyReload <= 0.2 && characterData.MaxEnergy >= Energy)
                {
                    Energy += characterData.EnergyIncreaseAmmount;

                    if (characterData.MaxEnergy == Energy)
                    {
                        isTire = false;
                        EnergyReload = characterData.EnergyReloadTime;
                    }
                }
            }
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(Keys[6].CurrentKey) && gameManager.isPause == false)
        {
            if (Gun == null && RightWeaponObject == null)
            {
                knife.isattack = true;
            }
            else
            {
                FireWeapon.Execute();
            }
        }
        else if (Input.GetKeyDown(Keys[6].CurrentKey) == false && Gun == null)
        {
            knife.isattack = false;
        }
    }

    #endregion

    #region Public Method

    public void HealthDisCount(int value)
    {
        int remainingDamage = 0;

        if (CurrentDefence > 0)
        {
            if (value > CurrentDefence)
            {
                remainingDamage = value - CurrentDefence;
                CurrentDefence = 0;
            }
            else
            {
                CurrentDefence -= value;
            }
        }
        else if (CurrentHP > 0)
        {
            if (value > CurrentHP)
            {
                Destroy(gameObject);
                isDead = true;
            }
            else
            {
                CurrentHP -= value;
            }

            if (remainingDamage != 0)
            {
                CurrentHP -= remainingDamage;
            }
        }
    }

    public void SlowDown(bool inside, int power, float healthDecreaseTime)
    {
        if (shooting > 0)
        {
            if (inside == true)
            {
                Speed = 2;

                if (shooting > 0)
                {
                    shooting -= Time.deltaTime;

                    if (shooting <= 0)
                    {
                        HealthDisCount(power);
                        shooting = healthDecreaseTime;
                    }
                }
            }
            else if (Input.GetKey(gameManager.KeySettings.Keys[4].CurrentKey) == false && inside == false)
            {
                Speed = characterData.Speed;
            }
        }
        else
        {
            shooting = healthDecreaseTime;
        }
    }

    #endregion
}