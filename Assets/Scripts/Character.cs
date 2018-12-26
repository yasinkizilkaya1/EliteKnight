using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Constants

    private const string CARD_DATA_PATH = "Assets/Data/CharacterData/";
    private const string CARD_DATA_BİLL = ".asset";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_TOWERENEMYBULLET = "TowerEnemyBullet";
    private const string TAG_TOWERBULLET = "TowerBullet";
    private const string TAG_HEALTH = "Health";
    private const string TAG_ARMOR = "Armor";
    private const string TAG_CLİP = "Clip";
    private const string TAG_AK47 = "Ak47";
    private const int RUNSPEED = 10;
    private const int DECELERATİON = 1;
    private const int BULLETLOSS = 1;
    private const int EXPLODEDBULLETLOSS = 8;
    private const int CLIPAMOUNT = 20;
    private const float ENERGYRELOADTİME = 5f;
    private const float SHOOTİNGRATE = 1f;

    #endregion

    #region Fields

    public GameManager gameManager;
    public Gun gun;
    public CharacterData characterData;

    public int Health;
    public int MaxHealth;
    public int Defence;
    public int Speed;
    public int Power;
    public int CharacterWay;
    public int run;
    public int Energy;
    public int MaxDefance;
    private int mMaxEnergy;
    private int mDefaultSpeed;

    public bool isDead;
    public bool isFindAk47;
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

        if (Health == 0)
        {
            Destroy(gameObject);
            isDead = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_TOWERBULLET))
        {
            HealthDisCount(BULLETLOSS);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag(TAG_TOWERENEMYBULLET))
        {
            HealthDisCount(EXPLODEDBULLETLOSS);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag(TAG_HEALTH))
        {
            if (Health < MaxHealth)
            {
                Health++;
                Destroy(col.gameObject);
            }
        }
        else if (col.gameObject.CompareTag(TAG_ARMOR))
        {
            if (Defence < MaxDefance)
            {
                Defence++;
                Destroy(col.gameObject);
            }
        }
        else if (col.gameObject.CompareTag(TAG_CLİP))
        {
            gameManager.spawn.listCharacterList[0].gun.tiroNew.clipAmmount += CLIPAMOUNT;
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag(TAG_AK47) && Input.GetKey(KeyCode.E))
        {
            isFindAk47 = true;
            Destroy(col.gameObject);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        characterData = (WarriorData)AssetDatabase.LoadAssetAtPath(CARD_DATA_PATH + gameManager.SelectedCardNameString + CARD_DATA_BİLL, typeof(WarriorData));

        name = characterData.Name;
        Health = characterData.Health;
        MaxHealth = Health;
        Energy = characterData.Energy;
        mMaxEnergy = Energy;
        Defence = characterData.Defence;
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
                    Speed = RUNSPEED;
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

        if (Defence > 0)
        {
            if (value > Defence)
            {
                remainingDamage = value - Defence;
                Defence = 0;
            }
            else
            {
                Defence -= value;
            }
        }
        else if (Health > 0)
        {
            if (value > Health)
            {
                Destroy(gameObject);
                isDead = true;
            }
            else
            {
                Health -= value;
            }

            if (remainingDamage != 0)
            {
                Health -= remainingDamage;
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
                    HealthDisCount(BULLETLOSS);
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