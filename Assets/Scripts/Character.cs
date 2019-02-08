using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constants

    private const string CARD_DATA_BİLL = ".asset";
    private const string CARD_DATA_PATH = "Assets/Data/CharacterData/";
    private const string TAG_TOWERENEMYBULLET = "TowerEnemyBullet";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_TOWERBULLET = "TowerBullet";
    private const string TAG_HEALTH = "Health";
    private const string TAG_ARMOR = "Armor";
    private const string TAG_CLIP = "Clip";
    private const string TAG_AK47 = "Ak47";
    private const string TAG_SHOTGUN = "Shotgun";
    private const int RUN_SPEED = 10;
    private const int DECELERATİON = 1;
    private const int BULLET_LOSS = 1;
    private const int EXPLODEDBULLETLOSS = 8;
    private const int CLIPAMOUNT = 30;
    private const float ENERGYRELOADTİME = 5f;
    private const float SHOOTİNGRATE = 1f;

    #endregion

    #region Fields

    public GameManager gameManager;
    public Gun gun;
    public CharacterData characterData;

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

    public bool isDead;
    public bool isGun;
    public bool isAk47;
    public bool isShotgun;
    public bool isShotgunUse;
    public bool isTire;

    public float EnergyReload;
    public float shooting;

    public GameObject characterObject;
    public GameObject BodyObject;
    public GameObject RightWeaponObject;
    private Vector3 mousePositionVector;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Moving(gameManager);
        Run(gameManager);
        CharacterTurn(gameManager);
        SlowDown(gameManager.towerEnemy.inside);

        if (CurrentHP == 0)
        {
            Destroy(gameObject);
            isDead = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (collision.gameObject.CompareTag(TAG_AK47))
            {
                isAk47 = true;
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.CompareTag(TAG_SHOTGUN))
            {
                isShotgun = true;
                isShotgunUse = true;
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(TAG_TOWERBULLET))
        {
            HealthDisCount(BULLET_LOSS);
            Destroy(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag(TAG_TOWERENEMYBULLET))
        {
            HealthDisCount(EXPLODEDBULLETLOSS);
            Destroy(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag(TAG_HEALTH))
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
            gameManager.spawn.CharacterList[0].gun.tiroNew.SpareBulletCount += CLIPAMOUNT;
            Destroy(collider.gameObject);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        characterData = (WarriorData)AssetDatabase.LoadAssetAtPath(CARD_DATA_PATH + gameManager.SelectedCardNameString + CARD_DATA_BİLL, typeof(WarriorData));

        name = characterData.Name;
        CurrentHP = characterData.Health;
        MaxHP = CurrentHP;
        Energy = characterData.Energy;
        mMaxEnergy = Energy;
        CurrentDefence = characterData.Defence;
        Power = characterData.Power;

        mDefaultSpeed = characterData.Speed;
        EnergyReload = 5f;
        shooting = SHOOTİNGRATE;
        MaxDefance = characterData.Defence;
    }

    private void Moving(GameManager gameManager)
    {
        if (gameManager.isPause == false)
        {
            if (Input.GetKey(gameManager.UpEnum))
            {
                characterObject.transform.Translate(Speed * Time.deltaTime, 0, 0);
                CharacterWay = 1;
            }

            if (Input.GetKey(gameManager.DownEnum))
            {
                characterObject.transform.Translate(-Speed * Time.deltaTime, 0, 0);
                CharacterWay = 1;
            }

            if (Input.GetKey(gameManager.LeftEnum))
            {
                characterObject.transform.Translate(0, Speed * Time.deltaTime, 0);
                CharacterWay = 3;
            }

            if (Input.GetKey(gameManager.RightEnum))
            {
                characterObject.transform.Translate(0, -Speed * Time.deltaTime, 0);
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
                    Speed = RUN_SPEED;
                    Energy -= DECELERATİON;
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
                    Energy += DECELERATİON;

                    if (mMaxEnergy == Energy)
                    {
                        isTire = false;
                        EnergyReload = ENERGYRELOADTİME;
                    }
                }
            }
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

    public void SlowDown(bool inside)
    {
        if (inside == true)
        {
            Speed = 2;

            if (shooting > 0)
            {
                shooting -= Time.deltaTime;

                if (shooting <= 0)
                {
                    HealthDisCount(BULLET_LOSS);
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