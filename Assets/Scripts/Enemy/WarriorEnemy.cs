using UnityEngine;

public class WarriorEnemy : MonoBehaviour
{
    #region Contants 

    private const string mTAG_GAMEMANAGER = "GameManager";
    private const float mWALLRADIUS = 0.2f;
    
    #endregion

    #region Fields

    public EnemyWarrior EnemyWarrior;

    public int CurrentHealth;
    public int CurrentDefence;
    private int mSpeed;
    private int mDistance;

    public GameManager GameManager;

    public Transform Radar;

    public Transform WallTransformObject;
    public Transform ZombieTransformObject;
    public LayerMask IsWall;
    public LayerMask Iszombie;

    public Room Room;
    public GameObject GunObject;
    public GameObject BulletObjcet;
    public GameObject Body;

    public RaycastHit2D RaycastHit2D;

    public bool mIsAttack;
    public bool mIsHit;
    public bool mIsZombie;
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

        if (GameManager.Character != null)
        {

            Aim();

            if (mIsAttack)
            {
                Following();

                if (mIsZombie == true || mIsHit == true)
                {
                    mIsAttack = false;
                }
            }
            else
            {
                if (mIsZombie == false && mIsHit == false)
                {
                    mIsAttack = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        mIsHit = Physics2D.OverlapCircle(WallTransformObject.position, mWALLRADIUS, IsWall);
        mIsZombie = Physics2D.OverlapCircle(ZombieTransformObject.position, mWALLRADIUS, Iszombie);
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        mShootCoolDown = 0f;
        CurrentHealth = EnemyWarrior.Health;
        CurrentDefence = EnemyWarrior.Defence;
        mSpeed = EnemyWarrior.Speed;
        mDistance = EnemyWarrior.Distance;
        mIsAttack = true;
    }

    private void Aim()
    {
        Transform shootTransform = Body.transform;
        shootTransform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, Body.transform);

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
            mShootCoolDown = EnemyWarrior.AttackTime;
            Instantiate(BulletObjcet, GunObject.transform.position, GunObject.transform.rotation);
            Following();
        }
    }

    private void Following()
    {
        if (Vector2.Distance(Body.transform.position, GameManager.Character.transform.position) > mDistance)
        {
            Body.transform.Translate(Vector2.right * -mSpeed * Time.deltaTime);
        }
        else
        {
            Attack();
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
        GameManager.Character.DeadEnemyCount++;

        if (Room != null)
        {
            Room.EnemyCount--;
        }
    }

    #endregion

    #region Public Method

    public void DisHealth(int power)
    {
        int remainingDamage = 0;

        if (CurrentDefence > 0)
        {
            if (power > CurrentDefence)
            {
                remainingDamage = power - CurrentDefence;
                CurrentDefence = 0;
            }
            else
            {
                CurrentDefence -= power;
            }
        }
        else if (CurrentHealth > 0 && CurrentHealth - power > 0)
        {
            CurrentHealth -= power;
        }
        else
        {
            Dead();
        }

        if (remainingDamage != 0)
        {
            CurrentHealth -= remainingDamage;

            if (CurrentHealth <= 0)
            {
                Dead();
            }
        }

        if (CurrentHealth > 0)
        {
            FloatingTextController.CreateFloatingText(power.ToString(), transform);
        }
    }

    public void RoomEqual(Room room)
    {
        Room = room;
    }

    #endregion
}