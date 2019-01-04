using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    #region Constants

    private const int HEALTH = 6;
    private const int DEFENCE = 4;
    private const float SHOOTİNGRATE = 0.75f;
    private const string TAG_CHARACTER = "Body";
    private const string TAG_BULLET = "bullet";
    private const string TAG_KNIFE = "knife";

    #endregion

    #region Fields

    public LineRenderer lineRenderer;
    public TowerEnemy towerEnemy;
    public Spawn spawn;

    public Transform shotPrefabTransform;
    public Transform OriginTransform;

    public GameObject TowerObject;

    public float shootCoolDown;

    public int Health;
    public int Defence;

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

        if (Health == 0)
        {
            towerEnemy.inside = false;
            Destroy(TowerObject);
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
        lineRenderer = GetComponent<LineRenderer>();
        Health = HEALTH;
        Defence = DEFENCE;
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
        Transform shootTransformObject = transform;
        shootTransformObject.rotation = ScriptHelper.LookAt2D(spawn.CharacterList[0].transform, shootTransformObject.transform);

        if (transform.rotation.z != shootTransformObject.rotation.z)
        {
            transform.Rotate(0, 0, transform.position.z);
        }
        else if (transform.rotation.z == shootTransformObject.rotation.z)
        {
            transform.rotation = shootTransformObject.rotation;
        }
    }

    private void HealtDisCount()
    {
        int remainingDamage = 0;
        if (Defence > 0)
        {
            if (spawn.CharacterList[0].Power > Defence)
            {
                remainingDamage = spawn.CharacterList[0].Power - Defence;
                Defence = 0;
            }
            else
            {
                Defence -= spawn.CharacterList[0].Power;
            }
        }
        else if (Defence == 0 && Health > 0)
        {
            if (spawn.CharacterList[0].Power > Health)
            {
                Health = 0;
            }
            else
            {
                Health -= spawn.CharacterList[0].Power;
            }

            if (remainingDamage != 0)
            {
                Health -= remainingDamage;
            }
        }
    }

    #endregion

    #region Public Method

    public void Attack(bool Tower)
    {
        if (CanAttack)
        {
            shootCoolDown = SHOOTİNGRATE;
            var shootTransformObject = Instantiate(shotPrefabTransform) as Transform;
            shootTransformObject.position = transform.position;
            shootTransformObject.rotation = ScriptHelper.LookAt2D(spawn.CharacterList[0].transform, shootTransformObject.transform);
        }
    }

    #endregion
}