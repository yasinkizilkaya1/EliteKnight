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

    public int Health;
    private int MaxHealt;
    private int Defence;
    private int Speed;
    private int AttacPower;
    private int Range;
    private int Distance;

    public GameManager gameManager;

    public Transform Radar;

    public GameObject RoomObject;
    public GameObject GunObject;
    public GameObject BulletObjcet;
    public GameObject Body;

    public RaycastHit2D raycastHit2D;

    private bool isAim;
    private bool isTargetFind;

    private float shootCoolDown;

    #endregion

    #region Property

    public bool CanAttack
    {
        get
        {
            return shootCoolDown <= 0f;
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
        if (shootCoolDown > 0)
        {
            shootCoolDown -= Time.deltaTime;
        }

        if (Health == 0)
        {
            RoomObject.GetComponent<Room>().EnemyCount--;
            Destroy(gameObject);
            gameManager.spawn.CharacterList[0].DeadEnemyCount++;
        }

        if (gameManager.spawn.CharacterList[0] != null)
        {
            TargetFind();

            if (isTargetFind)
            {
                if (isAim)
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
        if (collision.gameObject.CompareTag(TAG_BULLET))
        {
            Destroy(collision.gameObject);
            DisHealth();
        }
        else if (collision.gameObject.CompareTag(TAG_KNIFE))
        {
            DisHealth();
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        gameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        Physics2D.queriesStartInColliders = false;
        shootCoolDown = 0f;
        Health = enemyWarrior.Health;
        MaxHealt = enemyWarrior.MaxHealth;
        Defence = enemyWarrior.Defence;
        Speed = enemyWarrior.Speed;
        AttacPower = enemyWarrior.AttackPower;
        Range = enemyWarrior.Range;
        Distance = enemyWarrior.Distance;
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
            shootCoolDown = CAN_ATTACK_TIME;
            Instantiate(BulletObjcet, GunObject.transform.position, GunObject.transform.rotation);
            Following();
        }
    }

    private void TargetFind()
    {
        raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, Range);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.CompareTag(TAG_WALL) || raycastHit2D.collider.CompareTag(TAG_ENEMY) || raycastHit2D.collider.CompareTag(TAG_TOWER_ENEMY_SLIDER))
            {
                isTargetFind = false;
                Radar.Rotate(Vector3.forward * Range * Time.deltaTime);
                Debug.DrawLine(Radar.position, raycastHit2D.point, Color.red);
            }
            else
            {
                isTargetFind = true;
                Aim();
            }
        }
        else
        {
            raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, Range);
        }
    }

    private void Following()
    {
        if (Vector2.Distance(Body.transform.position, gameManager.spawn.CharacterList[0].transform.position) > Distance)
        {
            Aim();
            Body.transform.Translate(Vector2.right * -Speed * Time.deltaTime);
            isAim = false;
        }
        else
        {
            isAim = true;
        }
    }

    private void DisHealth()
    {
        int remainingDamage = 0;

        if (Defence > 0)
        {
            if (gameManager.spawn.CharacterList[0].Power > Defence)
            {
                remainingDamage = gameManager.spawn.CharacterList[0].Power - Defence;
            }
            else
            {
                Defence -= gameManager.spawn.CharacterList[0].Power;
            }
        }
        else if (Health > 0)
        {
            if (Health >= gameManager.spawn.CharacterList[0].Power)
            {
                Health -= gameManager.spawn.CharacterList[0].Power;
            }
        }

        if (remainingDamage != 0)
        {
            Health -= remainingDamage;
        }
    }

    #endregion

    #region Public Method

    public void RoomEqual(GameObject gameObject)
    {
        RoomObject = gameObject;
    }

    #endregion
}