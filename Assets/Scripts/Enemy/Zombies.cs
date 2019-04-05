using UnityEngine;

public class Zombies : MonoBehaviour
{
    #region Constants

    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_WALL = "wall";
    private const float WALLRADIUS = 0.2f;

    #endregion

    #region Fields

    private bool mIsHit;
    private bool mIsZombie;

    public GameManager GameManager;
    public Zombie zombie;

    private int mCurrrentHealth;
    private int mCurrentDefance;

    public Transform Radar;
    public Transform wallTransformObject;
    public Transform zombieTransformObject;
    public LayerMask isWall;
    public LayerMask iszombie;
    public LayerMask CharacterMask;

    public GameObject RoomObject;
    public GameObject BodyObject;
    public Animator animator;

    public float distance;
    public float rotationSpeed;
    public float shootcoolDown;

    private bool mIsAttack;

    private RaycastHit2D mRaycastHit2D;

    #endregion

    #region Property

    public bool attack
    {
        get
        {
            return shootcoolDown <= 0;
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
        animator.SetBool("isAttack", attack);

        if (mCurrrentHealth == 0)
        {
            Destroy(BodyObject);
            GameManager.Character.DeadEnemyCount++;
            RoomObject.GetComponent<Room>().EnemyCount--;
        }

        if (shootcoolDown > 0f)
        {
            shootcoolDown -= Time.deltaTime;
        }

        if (mIsAttack && mIsZombie == false && mIsHit == false)
        {
            Following();
        }
        else
        {
            RaycasLine();
        }
    }

    private void FixedUpdate()
    {
        mIsHit = Physics2D.OverlapCircle(wallTransformObject.position, WALLRADIUS, isWall);
        mIsZombie = Physics2D.OverlapCircle(zombieTransformObject.position, WALLRADIUS, iszombie);
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        shootcoolDown = 0f;
        mCurrrentHealth = zombie.Health;
        mCurrentDefance = zombie.Defence;
    }

    private void RaycasLine()
    {
        mRaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, distance, CharacterMask);
        Radar.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        if (mRaycastHit2D.collider != null)
        {
            if (!mRaycastHit2D.collider.CompareTag(TAG_WALL) || !mRaycastHit2D.collider.CompareTag(TAG_ENEMY))
            {
                mIsAttack = true;
                Following();
            }
        }
    }

    private void Following()
    {
        if (GameManager.isPlayerDead == false)
        {
            if (Vector2.Distance(Radar.position, GameManager.Character.transform.position) > zombie.AttackRange)
            {
                BodyObject.transform.Translate(Vector2.right * -zombie.Speed * Time.deltaTime);
                BodyObject.transform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, BodyObject.transform);
            }
            else if (Vector2.Distance(Radar.position, GameManager.Character.transform.position) < zombie.AttackRange + 0.1f)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (attack)
        {
            shootcoolDown = zombie.ShootingRate;
            GameManager.Character.HealthDisCount(GameManager.Character.Power);
        }
    }

    #endregion

    #region Public Method

    public void RoomEqual(GameObject gameObject)
    {
        RoomObject = gameObject;
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