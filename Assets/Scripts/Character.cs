using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constants

    private const string CARD_DATA_BILL = ".asset";
    private const string CARD_DATA_PATH = "Assets/Data/CharacterData/";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_HEALTH = "Health";
    private const string TAG_ARMOR = "Armor";
    private const string TAG_CLIP = "Clip";
    private const string TAG_AK47 = "Ak47";
    private const string TAG_SHOTGUN = "Shotgun";
    private const int DECELERATION = 1;
    private const int CLIPAMOUNT = 30;
    private const float ENERGYRELOADTIME = 5f;
    private const float SHOOTİNGRATE = 1f;

    #endregion

    #region Fields

    public CharacterData characterData;
    public GameManager gameManager;
    public Gun gun;
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
    private int mMaxEnergy;
    private int mDefaultSpeed;
    private int RunSpeed;

    public bool IsNewGun;
    public bool isDead;
    public bool isTire;

    public float EnergyReload;
    public float shooting;

    private Vector3 mousePositionVector;
    public GameObject BodyObject;
    public GameObject RightWeaponObject;
    public GameObject RightGunObject;
    public List<GameObject> WeaponList;

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
            AutoClipReload();
        }

        if (CurrentHP == 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (collider.gameObject.CompareTag(TAG_AK47))
            {
                GunChange(collider, WeaponList[1]);
            }
            else if (collider.gameObject.CompareTag(TAG_SHOTGUN))
            {
                GunChange(collider, WeaponList[2]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(TAG_HEALTH))
        {
            CurrentHP++;// look at here
            Destroy(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag(TAG_ARMOR))
        {
            CurrentDefence++;
            Destroy(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag(TAG_CLIP))
        {
            gameManager.spawn.CharacterList[0].gun.SpareBulletCount += CLIPAMOUNT;
            Destroy(collider.gameObject);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        characterData = (WarriorData)AssetDatabase.LoadAssetAtPath(CARD_DATA_PATH + gameManager.SelectedCardNameString + CARD_DATA_BILL, typeof(WarriorData));

        name = characterData.Name;
        CurrentHP = characterData.Health;
        MaxHP = CurrentHP;
        Energy = characterData.Energy;
        mMaxEnergy = Energy;
        CurrentDefence = characterData.Defence;
        Power = characterData.Power;

        mDefaultSpeed = characterData.Speed;
        EnergyReload = 5f;
        RunSpeed = characterData.RunSpeed;
        shooting = SHOOTİNGRATE;
        MaxDefance = characterData.Defence;
    }

    private void GunChange(Collider2D collider, GameObject GunObject)
    {
        IsNewGun = true;
        Destroy(RightGunObject);
        RightGunObject = Instantiate(GunObject, RightWeaponObject.transform);
        gun = RightGunObject.GetComponent<Gun>();
        Destroy(collider.gameObject);
    }

    private void Moving(GameManager gameManager)
    {
        if (gameManager.isPause == false)
        {
            if (Input.GetKey(gameManager.UpEnum))
            {
                transform.Translate(Speed * Time.deltaTime, 0, 0);
                CharacterWay = 1;
            }

            if (Input.GetKey(gameManager.DownEnum))
            {
                transform.Translate(-Speed * Time.deltaTime, 0, 0);
                CharacterWay = 1;
            }

            if (Input.GetKey(gameManager.LeftEnum))
            {
                transform.Translate(0, Speed * Time.deltaTime, 0);
                CharacterWay = 3;
            }

            if (Input.GetKey(gameManager.RightEnum))
            {
                transform.Translate(0, -Speed * Time.deltaTime, 0);
                CharacterWay = 2;
            }

            if (Input.GetKey(gameManager.UpEnum) == false && Input.GetKey(gameManager.LeftEnum) == false && Input.GetKey(gameManager.DownEnum) == false && Input.GetKey(gameManager.RightEnum) == false)
            {
                CharacterWay = 0;
            }
        }
    }

    private void CharacterTurn(GameManager gameManager)
    {
        if (gameManager.isPause == false)
        {
            mousePositionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionVector.z = transform.position.z;
            BodyObject.transform.right = (mousePositionVector - transform.position);
        }
    }

    private void Run(GameManager gameManager)
    {
        if (gameManager.isPause == false)
        {
            if (Input.GetKey(gameManager.RunEnum))
            {
                if (Energy > 0)
                {
                    Speed = RunSpeed;
                    Energy -= DECELERATION;
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

                if (EnergyReload <= 0.2 && mMaxEnergy >= Energy)
                {
                    Energy += DECELERATION;

                    if (mMaxEnergy == Energy)
                    {
                        isTire = false;
                        EnergyReload = ENERGYRELOADTIME;
                    }
                }
            }
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") && gameManager.isPause == false)
        {
            if (gun == null && RightWeaponObject == null)
            {
                knife.isattack = true;
            }
            else
            {
                gun.Fire();
            }
        }
        else if (Input.GetButtonDown("Fire1") == false && gun == null)
        {
            knife.isattack = false;
        }
    }

    private void AutoClipReload()
    {
        if (gameManager.AutoClipReloadToggle.isOn == true && gameManager.isPause == false)
        {
            gun.AutoWeaponReloadEnum();
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

    public void SlowDown(bool inside, int power)
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
                    shooting = SHOOTİNGRATE;
                }
            }
        }
        else if (Input.GetKey(gameManager.RunEnum) == false && inside == false)
        {
            Speed = characterData.Speed;
        }
    }

    #endregion
}