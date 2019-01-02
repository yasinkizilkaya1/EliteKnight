using UnityEngine;

public class Zombies : MonoBehaviour
{
    #region Constants

    private const string TAG_BULLET = "bullet";
    private const string TAG_KNİFE = "knife";
    private const string TAG_WALL = "wall";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_TOWER_ENEMY_SLİDER = "TowerEnemySlider";
    private const float WALLRADİUS = 0.2f;
    private const int SPEED = 4;

    private const float SHOOTİNG_RATE = 0.75f;
    private const float AFFİNİTY_ATTACK = 1.1f;
    private const float AFFİNİTY = 1f;
    private const int ATTACK_POWER = 1;
    private const int HEALTH = 5;
    private const int CHARACTER_POWER = 1;
    private const int DEFENCE = 3;

    #endregion

    #region Fields

    private bool isHit;
    private bool isZombie;

    public GameManager gameManager;
    public Spawn spawn;

    public Transform Radar;
    public Transform wallTransformObject;
    public Transform zombieTransformObject;
    public LayerMask isWall;
    public LayerMask iszombie;
    public GameObject BodyObject;
    public Animator animator;

    public float distance;
    public float rotationSpeed;
    public float shootcoolDown;
    public int Health;
    public int Defence;

    private Zombie zombie;
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

        if (Health == 0)
        {
            Destroy(BodyObject);
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
        isHit = Physics2D.OverlapCircle(wallTransformObject.position, WALLRADİUS, isWall);
        isZombie = Physics2D.OverlapCircle(zombieTransformObject.position, WALLRADİUS, iszombie);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_BULLET))
        {
            DisHealth();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag(TAG_KNİFE))
        {
            DisHealth();
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        Physics2D.queriesStartInColliders = false;
        shootcoolDown = 0f;
        Health = HEALTH;
        Defence = DEFENCE;
    }

    private void RaycasLine()
    {
        raycastHit2D = Physics2D.Raycast(Radar.position, Radar.up, distance);

        if (raycastHit2D.collider != null)
        {
            if (raycastHit2D.collider.CompareTag(TAG_WALL) || raycastHit2D.collider.CompareTag(TAG_TOWER_ENEMY_SLİDER) || raycastHit2D.collider.CompareTag(TAG_ENEMY))
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
            if (Vector2.Distance(Radar.position, spawn.listCharacterList[0].transform.position) > AFFİNİTY)
            {
                BodyObject.transform.Translate(Vector2.right * -SPEED * Time.deltaTime);
                BodyObject.transform.rotation = ScriptHelper.LookAt2D(spawn.listCharacterList[0].transform, BodyObject.transform);
            }
            else if (Vector2.Distance(Radar.position, spawn.listCharacterList[0].transform.position) < AFFİNİTY_ATTACK)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (attack)
        {
            shootcoolDown = SHOOTİNG_RATE;
            spawn.listCharacterList[0].HealthDisCount(CHARACTER_POWER);
        }
    }

    private void DisHealth()
    {
        int remainingDamage = 0;
        if (Defence > 0)
        {
            if (spawn.listCharacterList[0].Power > Defence)
            {
                remainingDamage = spawn.listCharacterList[0].Power - Defence;
                Defence = 0;
            }
            else
            {
                Defence -= spawn.listCharacterList[0].Power;
            }
        }
        else if (Defence == 0 && Health > 0)
        {
            if (spawn.listCharacterList[0].Power > Health)
            {
                Health = 0;
            }
            else
            {
                Health -= spawn.listCharacterList[0].Power;
            }
        }

        if (remainingDamage != 0)
        {
            Health -= remainingDamage;
        }
    }

    #endregion
}