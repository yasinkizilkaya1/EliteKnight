using UnityEngine;

public class WarriorEnemy : MonoBehaviour
{
    #region Contants 

    private const string TAG_TOWER_ENEMY_SLIDER = "TowerEnemySlider";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_BULLET = "bullet";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_KNIFE = "knife";
    private const string TAG_WALL = "wall";
    private const float CAN_ATTACK_TIME = 0.75f;

    #endregion

    #region Fields

    public EnemyWarrior enemyWarrior;

    public int CurrentHealth;
    private int mMaxHealt;
    private int mCurrentDefence;
    private int mSpeed;
    private int mAttacPower;
    private int mRange;
    private int mDistance;

    public GameManager gameManager;

    public Transform Radar;

    public GameObject RoomObject;
    public GameObject GunObject;
    public GameObject BulletObjcet;
    public GameObject Body;

    public RaycastHit2D raycastHit2D;

    private bool mIsAim;
    private bool mIsTargetFind;

    private float mShootCoolDown;

    #endregion

    #region Property

    public bool CanAttack
    {
        get
        {
            return mShootCoolDown <= 0f;
        }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (mShootCoolDown > 0)
        {
            mShootCoolDown -= Time.deltaTime;
        }

        if (CurrentHealth == 0)
        {
            RoomObject.GetComponent<Room>().EnemyCount--;
            Destroy(gameObject);
            gameManager.spawn.CharacterList[0].DeadEnemyCount++;
        }

        if (gameManager.spawn.CharacterList[0] != null)
        {
            TargetFind();

            if (mIsTargetFind)
            {
                if (mIsAim)
                {
                    Attack();
                }
                else
                {
                    Following();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_KNIFE))
        {
            DisHealth(1); //look
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        mShootCoolDown = 0f;
        CurrentHealth = enemyWarrior.Health;
        mMaxHealt = enemyWarrior.MaxHealth;
        mCurrentDefence = enemyWarrior.Defence;
        mSpeed = enemyWarrior.Speed;
        mAttacPower = enemyWarrior.AttackPower;
        mRange = enemyWarrior.Range;
        mDistance = enemyWarrior.Distance;
    }

    private void Aim()
    {
        Transform shootTransform = Body.transform;
        shootTransform.rotation = ScriptHelper.LookAt2D(gameManager.spawn.CharacterList[0].transform, Body.transform);

        if (Body.transform.rotation.z != shootTransform.rotation.z)
        {
            Body.transform.Rotate(0, 0, transform.position.z);
        }
        else if (Body.transform.rotation.z == shootTransform.rotation.z)
        {
            Body.transform.rotation = shootTransform.rotation;
        }
    }

    private void Attack()
    {
        if (CanAttack)
        {
            Aim();
            mShootCoolDown = CAN_ATTACK_TIME;
            Instantiate(BulletObjcet, GunObject.transform.position, GunObject.transform.rotation);
            Following();
        }
    }

    private void TargetFind()
    {
        raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, mRange);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.CompareTag(TAG_WALL) || raycastHit2D.collider.CompareTag(TAG_ENEMY) || raycastHit2D.collider.CompareTag(TAG_TOWER_ENEMY_SLIDER))
            {
                mIsTargetFind = false;
                Radar.Rotate(Vector3.forward * mRange * Time.deltaTime);
                Debug.DrawLine(Radar.position, raycastHit2D.point, Color.red);
            }
            else
            {
                mIsTargetFind = true;
                Aim();
            }
        }
        else
        {
            raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, mRange);
        }
    }

    private void Following()
    {
        if (Vector2.Distance(Body.transform.position, gameManager.spawn.CharacterList[0].transform.position) > mDistance)
        {
            Aim();
            Body.transform.Translate(Vector2.right * -mSpeed * Time.deltaTime);
            mIsAim = false;
        }
        else
        {
            mIsAim = true;
        }
    }

    #endregion

    #region Public Method

    public void DisHealth(int power)
    {
        int remainingDamage = 0;

        if (mCurrentDefence > 0)
        {
            if (power > mCurrentDefence)
            {
                remainingDamage = power - mCurrentDefence;
            }
            else
            {
                mCurrentDefence -= power;
            }
        }
        else if (CurrentHealth > 0)
        {
            if (CurrentHealth >= power)
            {
                CurrentHealth -= power;
            }
        }

        if (remainingDamage != 0)
        {
            CurrentHealth -= remainingDamage;
        }
    }

    public void RoomEqual(GameObject gameObject)
    {
        RoomObject = gameObject;
    }

    #endregion
}