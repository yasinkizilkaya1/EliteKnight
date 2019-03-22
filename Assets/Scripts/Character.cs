using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constants

    private const string CARD_DATA_BILL = ".asset";
    private const string CARD_DATA_PATH = "Assets/Data/Characters/";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_ITEM = "Item";

    #endregion

    #region Fields

    public CharacterData characterData;
    public GameManager gameManager;
    public UIManager UIManager;
    public Gun Gun;
    public Knife knife;

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
    public int SelectionWeaponId;

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
            Moving(gameManager, UIManager);
            Run(gameManager, UIManager);
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
                GunChange(Guns[SelectionWeaponId]);
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
                GunChange(Guns[SelectionWeaponId]);
            }
        }

        if (CurrentHP == 0)
        {
            isDead = true;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_ITEM))
        {
            Destroy(collider.gameObject);
            GunAdd(collider.GetComponent<Gun>().weapon);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        UIManager = gameManager.UIManager;
        characterData = gameManager.CharacterData;
        name = characterData.Name;
        CurrentHP = characterData.Health;
        MaxHP = CurrentHP;
        Energy = characterData.Energy;
        CurrentDefence = characterData.Defence;
        Power = characterData.Power;

        mDefaultSpeed = characterData.Speed;
        EnergyReload = characterData.EnergyReloadTime;
        mRunSpeed = characterData.RunSpeed;
        MaxDefance = characterData.Defence;
    }

    private void Moving(GameManager GameManager, UIManager uIManager)
    {
        if (GameManager.isPause == false)
        {
            if (Input.GetKey(uIManager.UpEnum))
            {
                transform.Translate(Speed * Time.deltaTime, 0, 0);
                CharacterWay = 1;
            }

            if (Input.GetKey(uIManager.DownEnum))
            {
                transform.Translate(-Speed * Time.deltaTime, 0, 0);
                CharacterWay = 1;
            }

            if (Input.GetKey(uIManager.LeftEnum))
            {
                transform.Translate(0, Speed * Time.deltaTime, 0);
                CharacterWay = 3;
            }

            if (Input.GetKey(uIManager.RightEnum))
            {
                transform.Translate(0, -Speed * Time.deltaTime, 0);
                CharacterWay = 2;
            }

            if (Input.GetKey(uIManager.UpEnum) == false && Input.GetKey(uIManager.LeftEnum) == false && Input.GetKey(uIManager.DownEnum) == false && Input.GetKey(uIManager.RightEnum) == false)
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

    private void Run(GameManager gameManager, UIManager uIManager)
    {
        if (gameManager.isPause == false)
        {
            if (Input.GetKey(uIManager.RunEnum))
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
        if (Input.GetButtonDown("Fire1") && gameManager.isPause == false)
        {
            if (Gun == null && RightWeaponObject == null)
            {
                knife.isattack = true;
            }
            else
            {
                Gun.Fire();
            }
        }
        else if (Input.GetButtonDown("Fire1") == false && Gun == null)
        {
            knife.isattack = false;
        }
    }

    #endregion

    #region Public Method

    public void GunAdd(Weapon weapon)
    {
        Gun NewGun = Instantiate(weapon.ItemObject, RightWeaponObject.transform).GetComponent<Gun>() as Gun;
        NewGun.transform.localScale = new Vector3(10, 10, 1);
        NewGun.gameObject.SetActive(false);
        Guns.Add(NewGun);
    }

    public void GunChange(Gun gun)
    {
        if (Gun != gun && Gun.isWeaponReload == false)
        {
            IsNewGun = true;
            Gun.gameObject.SetActive(false);
            Gun = gun;
            gun.gameObject.SetActive(true);
            gameManager.GunSlot.ItemImage[0].sprite = gun.weapon.Icon;
            gameManager.GunSlot.Items[0] = Gun.weapon;
        }
    }

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
            else if (Input.GetKey(UIManager.RunEnum) == false && inside == false)
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