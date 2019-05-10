using UnityEngine;

public class Zombies : MonoBehaviour
{
    #region Constants

    private const string mTAG_GAMEMANAGER = "GameManager";
    private const string mTAG_ENEMY = "Enemy";
    private const string mTAG_WALL = "wall";
    private const string mTAG_CHEST = "chest";
    private const float mWALLRADIUS = 0.2f;

    #endregion

    #region Fields

    public GameManager GameManager;
    public Zombie Zombie;
    public Room Room;

    private int mCurrrentHealth;
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

    private bool mIsAttack;
    private bool mIsHit;
    private bool mIsZombie;

    private RaycastHit2D mRaycastHit2D;

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

        if (mCurrrentHealth == 0)
        {
            Destroy(BodyObject);
            GameManager.Character.DeadEnemyCount++;

            if (Room != null)
            {
                Room.EnemyCount--;
            }
        }

        if (GameManager.Character != null)
        {
            if (ShootcoolDown > 0f)
            {
                ShootcoolDown -= Time.deltaTime;
            }

            if (GameManager.Character != null)
            {
                if (mIsAttack && mIsZombie == false && mIsHit == false)
                {
                    Following();
                }
                else
                {
                    RaycasLine();
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
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        ShootcoolDown = 0f;
        mCurrrentHealth = Zombie.Health;
        mCurrentDefance = Zombie.Defence;
        RotationSpeed = Zombie.RotationSpeed;
    }

    private void RaycasLine()
    {
        mRaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, CharacterMask);

        if (mRaycastHit2D.collider != null)
        {
            if (mRaycastHit2D.collider.CompareTag(mTAG_WALL) || mRaycastHit2D.collider.CompareTag(mTAG_ENEMY) || mRaycastHit2D.collider.CompareTag(mTAG_CHEST))
            {
                mIsAttack = false;
                Radar.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime);
                Debug.DrawLine(Radar.position, mRaycastHit2D.point, Color.red);
            }
            else
            {
                mIsAttack = true;
                Following();
            }
        }
        else
        {
            mRaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, RotationSpeed);
        }
    }

    private void Following()
    {
        if (GameManager.IsPlayerDead == false)
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
    }

    private void Attack()
    {
        if (attack)
        {
            ShootcoolDown = Zombie.ShootingRate;
            GameManager.Character.HealthDisCount(Zombie.AttackPower);
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
        else if (mCurrentDefance == 0 && mCurrrentHealth > 0)
        {
            if (power > mCurrrentHealth)
            {
                mCurrrentHealth = 0;
            }
            else
            {
                mCurrrentHealth -= power;
            }
        }

        if (remainingDamage != 0)
        {
            mCurrrentHealth -= remainingDamage;
        }
    }

    #endregion
}