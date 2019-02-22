using UnityEngine;

public class Zombies : MonoBehaviour
{
    #region Constants

    private const string TAG_TOWER_ENEMY_SLIDER = "TowerEnemySlider";
    private const string TAG_BULLET = "bullet";
    private const string TAG_KNIFE = "knife";
    private const string TAG_WALL = "wall";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_SPAWN = "Spawn";
    private const float WALLRADIUS = 0.2f;

    private const float SHOOTING_RATE = 0.75f;
    private const float AFFINITY_ATTACK = 1.1f;
    private const float AFFINITY = 1f;

    #endregion

    #region Fields

    private bool mIsHit;
    private bool mIsZombie;

    public GameManager gameManager;
    public Spawn spawn;
    public Zombie zombie;

    private int mCurrrentHealth;
    private int mCurrentDefance;

    public Transform Radar;
    public Transform wallTransformObject;
    public Transform zombieTransformObject;
    public LayerMask isWall;
    public LayerMask iszombie;

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
            spawn.CharacterList[0].DeadEnemyCount++;
            RoomObject.GetComponent<Room>().EnemyCount--;
        }

        if (shootcoolDown > 0f)
        {
            shootcoolDown -= Time.deltaTime;
        }

        if (mIsHit)
        {
            RaycasLine();
        }
        else if (mIsAttack && mIsZombie == false)
        {
            Following();
        }
        else if (mIsZombie)
        {
            RaycasLine();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_KNIFE))
        {
            DisHealth(1);                                   //look
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        spawn = GameObject.FindWithTag(TAG_SPAWN).GetComponent<Spawn>();
        Physics2D.queriesStartInColliders = false;
        shootcoolDown = 0f;
        mCurrrentHealth = zombie.Health;
        mCurrentDefance = zombie.Defence;
    }

    private void RaycasLine()
    {
        mRaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, distance);

        if (mRaycastHit2D.collider != null)
        {
            if (mRaycastHit2D.collider.CompareTag(TAG_WALL) || mRaycastHit2D.collider.CompareTag(TAG_TOWER_ENEMY_SLIDER) || mRaycastHit2D.collider.CompareTag(TAG_ENEMY))
            {
                Radar.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
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
            mRaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, distance);
        }
    }

    private void Following()
    {
        if (gameManager.isPlayerDead == false)
        {
            if (Vector2.Distance(Radar.position, spawn.CharacterList[0].transform.position) > AFFINITY)
            {
                BodyObject.transform.Translate(Vector2.right * -zombie.Speed * Time.deltaTime);
                BodyObject.transform.rotation = ScriptHelper.LookAt2D(spawn.CharacterList[0].transform, BodyObject.transform);
            }
            else if (Vector2.Distance(Radar.position, spawn.CharacterList[0].transform.position) < AFFINITY_ATTACK)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (attack)
        {
            shootcoolDown = SHOOTING_RATE;
            spawn.CharacterList[0].HealthDisCount(spawn.CharacterList[0].Power);
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