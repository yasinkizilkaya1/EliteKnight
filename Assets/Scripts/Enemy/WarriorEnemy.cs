using UnityEngine;

public class WarriorEnemy : MonoBehaviour
{
    #region Contants 

    private const string mTAG_GAMEMANAGER = "GameManager";
    private const string mTAG_ENEMY = "Enemy";
    private const string mTAG_CHEST = "chest";
    private const string mTAG_WALL = "wall";

    #endregion

    #region Fields

    public EnemyWarrior EnemyWarrior;

    public int CurrentHealth;
    public int CurrentDefence;
    private int mSpeed;
    private int mRange;
    private int mDistance;

    public GameManager GameManager;

    public Transform Radar;

    public Room Room;
    public GameObject GunObject;
    public GameObject BulletObjcet;
    public GameObject Body;

    public RaycastHit2D RaycastHit2D;

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
            Destroy(gameObject);
            GameManager.Character.DeadEnemyCount++;

            if (Room != null)
            {
                Room.EnemyCount--;

            }
        }

        if (GameManager.Character != null)
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
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        mShootCoolDown = 0f;
        CurrentHealth = EnemyWarrior.Health;
        CurrentDefence = EnemyWarrior.Defence;
        mSpeed = EnemyWarrior.Speed;
        mRange = EnemyWarrior.Range;
        mDistance = EnemyWarrior.Distance;
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
            mShootCoolDown = EnemyWarrior.AttackTime;
            Instantiate(BulletObjcet, GunObject.transform.position, GunObject.transform.rotation);
            Following();
        }
    }

    private void TargetFind()
    {
        RaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, mRange);

        if (RaycastHit2D.collider != null)
        {
            if (RaycastHit2D.collider.CompareTag(mTAG_WALL) || RaycastHit2D.collider.CompareTag(mTAG_ENEMY) || RaycastHit2D.collider.CompareTag(mTAG_CHEST))
            {
                mIsTargetFind = false;
                Radar.Rotate(Vector3.forward * mRange * Time.deltaTime);
                Debug.DrawLine(Radar.position, RaycastHit2D.point, Color.red);
            }
            else
            {
                mIsTargetFind = true;
                Aim();
            }
        }
        else
        {
            RaycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, mRange);
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

    public void RoomEqual(Room room)
    {
        Room = room;
    }

    #endregion
}