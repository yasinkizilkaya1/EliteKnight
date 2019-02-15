using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Body";
    private const string TAG_BULLET = "bullet";
    private const string TAG_KNIFE = "knife";
    private const string TAG_SPAWN = "Spawn";
    private const float SHOOTINGRATE = 0.75f;
    private const int HEALTH = 6;
    private const int DEFENCE = 4;

    #endregion

    #region Fields

    public LineRenderer lineRenderer;
    public TowerEnemy towerEnemy;
    public Spawn spawn;
    public Tower tower;

    public Transform shotPrefabTransform;
    public Transform OriginTransform;

    public GameObject RoomObject;
    public GameObject TowerObject;
    public GameObject BarrelObject;

    public float shootCoolDown;

    public int CurrentHealth;
    public int CurrentDefence;

    public bool isLinerenderer;

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
        Initialize();
    }

    private void Awake()
    {
        shootCoolDown = 0f;
    }

    private void Update()
    {
        TowerMoving();
        TowerLinerender();

        if (shootCoolDown > 0)
        {
            shootCoolDown -= Time.deltaTime;
        }

        if (CurrentHealth == 0)
        {
            RoomObject.GetComponent<Room>().EnemyCount--;
            towerEnemy.inside = false;
            Destroy(gameObject);
            spawn.CharacterList[0].DeadEnemyCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_BULLET))
        {
            Destroy(col.gameObject);
            HealtDisCount();
        }
        else if (col.gameObject.CompareTag(TAG_KNIFE))
        {
            HealtDisCount();
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        spawn = GameObject.FindWithTag(TAG_SPAWN).GetComponent<Spawn>();
        CurrentHealth = tower.Health;
        CurrentDefence = tower.Defence;
    }

    private void TowerMoving()
    {
        if (spawn.CharacterList[0].isDead == false && towerEnemy.inside)
        {
            Moving();
        }
    }

    private void TowerLinerender()
    {
        if (spawn.CharacterList[0].isDead == false && isLinerenderer == true && towerEnemy.inside)
        {
            lineRenderer.SetPosition(0, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
            lineRenderer.SetPosition(1, new Vector3(spawn.CharacterList[0].transform.position.x, spawn.CharacterList[0].transform.position.y, 1));
        }
        else
        {
            if (isLinerenderer == true)
            {
                lineRenderer.SetPosition(0, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
                lineRenderer.SetPosition(1, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
            }
        }
    }

    private void Moving()
    {
        Transform shootTransformObject = TowerObject.transform;
        shootTransformObject.rotation = ScriptHelper.LookAt2D(spawn.CharacterList[0].transform, shootTransformObject.transform);

        if (TowerObject.transform.rotation.z != shootTransformObject.rotation.z)
        {
            TowerObject.transform.Rotate(0, 0, TowerObject.transform.position.z);
        }
        else if (TowerObject.transform.rotation.z == shootTransformObject.rotation.z)
        {
            TowerObject.transform.rotation = shootTransformObject.rotation;
        }
    }

    private void HealtDisCount()
    {
        int remainingDamage = 0;
        if (CurrentDefence > 0)
        {
            if (spawn.CharacterList[0].Power > CurrentDefence)
            {
                remainingDamage = spawn.CharacterList[0].Power - CurrentDefence;
                CurrentDefence = 0;
            }
            else
            {
                CurrentDefence -= spawn.CharacterList[0].Power;
            }
        }
        else if (CurrentDefence == 0 && CurrentHealth > 0)
        {
            if (spawn.CharacterList[0].Power > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
            {
                CurrentHealth -= spawn.CharacterList[0].Power;
            }

            if (remainingDamage != 0)
            {
                CurrentHealth -= remainingDamage;
            }
        }
    }

    #endregion

    #region Public Method

    public void Attack(bool Tower)
    {
        if (CanAttack)
        {
            shootCoolDown = SHOOTINGRATE;
            var shootTransformObject = Instantiate(shotPrefabTransform) as Transform;
            shootTransformObject.position = BarrelObject.transform.position;
            shootTransformObject.rotation = ScriptHelper.LookAt2D(spawn.CharacterList[0].transform, shootTransformObject.transform);
        }
    }

    public void RoomEqual(GameObject gameObject)
    {
        RoomObject = gameObject;
    }

    #endregion
}