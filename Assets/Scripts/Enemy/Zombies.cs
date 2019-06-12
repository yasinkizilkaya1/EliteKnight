using UnityEngine;

public class Zombies : MonoBehaviour
{
    #region Constants

    private const string mTAG_GAMEMANAGER = "GameManager";
    private const float mWALLRADIUS = 0.2f;

    #endregion

    #region Fields

    public GameManager GameManager;
    public Zombie Zombie;
    public Room Room;

    private int mCurrentHealth;
    private int mCurrentDefance;

    public Transform Radar;
    public Transform WallTransformObject;
    public Transform ZombieTransformObject;
    public LayerMask IsWall;
    public LayerMask Iszombie;
    public LayerMask CharacterMask;

    public GameObject BodyObject;
    public Animator Animator;

    public float RotationSpeed;
    public float ShootcoolDown;

    public bool mIsAttack;
    public bool mIsHit;
    public bool mIsZombie;

    public RaycastHit2D mRaycastHit2D;

    #endregion

    #region Property

    public bool attack
    {
        get
        {
            return ShootcoolDown <= 0;
        }
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Animator.SetBool("isAttack", attack);

        if (GameManager.Character != null)
        {
            if (ShootcoolDown > 0f)
            {
                ShootcoolDown -= Time.deltaTime;
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
    }

    private void FixedUpdate()
    {
        mIsHit = Physics2D.OverlapCircle(WallTransformObject.position, mWALLRADIUS, IsWall);
        mIsZombie = Physics2D.OverlapCircle(ZombieTransformObject.position, mWALLRADIUS, Iszombie);
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        mIsAttack = true;
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        ShootcoolDown = 0f;
        mCurrentHealth = Zombie.Health;
        mCurrentDefance = Zombie.Defence;
        RotationSpeed = Zombie.RotationSpeed;
    }

    private void Following()
    {
        if (Vector2.Distance(Radar.position, GameManager.Character.transform.position) > Zombie.AttackRange)
        {
            BodyObject.transform.Translate(Vector2.right * -Zombie.Speed * Time.deltaTime);
            BodyObject.transform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, BodyObject.transform);
        }
        else if (Vector2.Distance(Radar.position, GameManager.Character.transform.position) < Zombie.AttackRange + 0.1f)
        {
            Attack();
        }
    }

    private void Aim()
    {
        Transform shootTransform = BodyObject.transform;
        shootTransform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, BodyObject.transform);

        if (BodyObject.transform.rotation.z != shootTransform.rotation.z)
        {
            BodyObject.transform.Rotate(0, 0, transform.position.z);
        }
        else if (BodyObject.transform.rotation.z == shootTransform.rotation.z)
        {
            BodyObject.transform.rotation = shootTransform.rotation;
        }
    }

    private void Attack()
    {
        if (attack)
        {
            ShootcoolDown = Zombie.ShootingRate;
            GameManager.Character.HealthDisCount(Zombie.AttackPower);
        }
    }

    private void Dead()
    {
        Destroy(BodyObject);
        GameManager.Character.DeadEnemyCount++;

        if (Room != null)
        {
            Room.EnemyCount--;
        }
    }

    #endregion

    #region Public Method

    public void RoomEqual(Room room)
    {
        Room = room;
    }

    public void DisHealth(int power)
    {
        int remainingDamage = 0;

        if (mCurrentDefance > 0)
        {
            if (power > mCurrentDefance)
            {
                remainingDamage = power - mCurrentDefance;
                mCurrentDefance = 0;
            }
            else
            {
                mCurrentDefance -= power;
            }
        }
        else if (mCurrentHealth > 0 && mCurrentHealth - power > 0)
        {
            mCurrentHealth -= power;
        }
        else
        {
            Dead();
        }

        if (remainingDamage != 0)
        {
            mCurrentHealth -= remainingDamage;

            if (mCurrentHealth <= 0)
            {
                Dead();
            }
        }

        if (mCurrentHealth > 0)
        {
            FloatingTextController.CreateFloatingText(power.ToString(), transform);
        }
    }

    #endregion
}