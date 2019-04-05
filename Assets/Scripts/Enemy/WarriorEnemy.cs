using UnityEngine;

public class WarriorEnemy : MonoBehaviour
{
    #region Contants 

    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_CHEST = "chest";
    private const string TAG_WALL = "wall";

    #endregion

    #region Fields

    public EnemyWarrior enemyWarrior;

    public int CurrentHealth;
    public int CurrentDefence;
    private int mSpeed;
    private int mRange;
    private int mDistance;

    public GameManager GameManager;

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
            GameManager.Character.DeadEnemyCount++;
        }              
                       
        if (GameManager.Character!= null)
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

    #endregion

    #region Private Methods

    private void Init()
    {
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        mShootCoolDown = 0f;
        CurrentHealth = enemyWarrior.Health;
        CurrentDefence = enemyWarrior.Defence;
        mSpeed = enemyWarrior.Speed;
        mRange = enemyWarrior.Range;
        mDistance = enemyWarrior.Distance;
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
            Aim();
            mShootCoolDown = enemyWarrior.AttackTime;
            Instantiate(BulletObjcet, GunObject.transform.position, GunObject.transform.rotation);
            Following();
        }
    }

    private void TargetFind()
    {
        raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, mRange);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.CompareTag(TAG_WALL) || raycastHit2D.collider.CompareTag(TAG_ENEMY) || raycastHit2D.collider.CompareTag(TAG_CHEST))
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
        if (Vector2.Distance(Body.transform.position, GameManager.Character.transform.position) > mDistance)
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

        if (CurrentDefence > 0)
        {
            if (power > CurrentDefence)
            {
                remainingDamage = power - CurrentDefence;
            }
            else
            {
                CurrentDefence -= power;
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