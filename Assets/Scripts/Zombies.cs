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

    private bool isHit;
    private bool isZombie;

    public GameManager gameManager;
    public Spawn spawn;
    public Zombie zombie;

    private int CurrrentHealth;
    private int CurrentDefance;

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

    private bool isAttack;

    private RaycastHit2D raycastHit2D;

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

        if (CurrrentHealth == 0)
        {
            Destroy(BodyObject);
            spawn.CharacterList[0].DeadEnemyCount++;
            RoomObject.GetComponent<Room>().EnemyCount--;
        }

        if (shootcoolDown > 0f)
        {
            shootcoolDown -= Time.deltaTime;
        }

        if (isHit)
        {
            RaycasLine();
        }
        else if (isAttack && isZombie == false)
        {
            Following();
        }
        else if (isZombie)
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
        isHit = Physics2D.OverlapCircle(wallTransformObject.position, WALLRADIUS, isWall);
        isZombie = Physics2D.OverlapCircle(zombieTransformObject.position, WALLRADIUS, iszombie);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_BULLET))
        {
            DisHealth();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag(TAG_KNIFE))
        {
            DisHealth();
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
        CurrrentHealth = zombie.Health;
        CurrentDefance = zombie.Defence;
    }

    private void RaycasLine()
    {
        raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, distance);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.CompareTag(TAG_WALL) || raycastHit2D.collider.CompareTag(TAG_TOWER_ENEMY_SLIDER) || raycastHit2D.collider.CompareTag(TAG_ENEMY))
            {
                Radar.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                Debug.DrawLine(Radar.position, raycastHit2D.point, Color.red);
            }
            else
            {
                isAttack = true;
                Following();
            }
        }
        else
        {
            raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, distance);
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

    private void DisHealth()
    {
        int remainingDamage = 0;
        if (CurrentDefance > 0)
        {
            if (spawn.CharacterList[0].Power > CurrentDefance)
            {
                remainingDamage = spawn.CharacterList[0].Power - CurrentDefance;
                CurrentDefance = 0;
            }
            else
            {
                CurrentDefance -= spawn.CharacterList[0].Power;
            }
        }
        else if (CurrentDefance == 0 && CurrrentHealth > 0)
        {
            if (spawn.CharacterList[0].Power > CurrrentHealth)
            {
                CurrrentHealth = 0;
            }
            else
            {
                CurrrentHealth -= spawn.CharacterList[0].Power;
            }
        }

        if (remainingDamage != 0)
        {
            CurrrentHealth -= remainingDamage;
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