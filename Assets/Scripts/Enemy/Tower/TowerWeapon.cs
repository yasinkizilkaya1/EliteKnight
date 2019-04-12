using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    #region Constants

    private const string TAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    private GameManager GameManager;

    public LineRenderer lineRenderer;
    public TowerEnemy towerEnemy;
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
            GameManager.Character.DeadEnemyCount++;
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        shootCoolDown = 0f;
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        CurrentHealth = tower.Health;
        CurrentDefence = tower.Defence;
    }

    private void TowerMoving()
    {
        if (GameManager.Character.isDead == false && towerEnemy.inside && lineRenderer == null)
        {
            Moving();
        }
        else if (GameManager.Character.isDead == false && lineRenderer != null)
        {
            Moving();
        }
    }

    private void TowerLinerender()
    {
        if (GameManager.Character.isDead == false && isLinerenderer == true && towerEnemy.inside)
        {
            lineRenderer.SetPosition(0, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
            lineRenderer.SetPosition(1, new Vector3(GameManager.Character.transform.position.x, GameManager.Character.transform.position.y, 1));
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
        shootTransformObject.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, shootTransformObject.transform);

        if (TowerObject.transform.rotation.z != shootTransformObject.rotation.z)
        {
            TowerObject.transform.Rotate(0, 0, TowerObject.transform.position.z);
        }
        else if (TowerObject.transform.rotation.z == shootTransformObject.rotation.z)
        {
            TowerObject.transform.rotation = shootTransformObject.rotation;
        }
    }

    #endregion

    #region Public Method

    public void Attack(bool Tower)
    {
        if (CanAttack)
        {
            shootCoolDown = tower.AttackTime;
            var shootTransformObject = Instantiate(shotPrefabTransform) as Transform;
            shootTransformObject.position = BarrelObject.transform.position;
            shootTransformObject.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, shootTransformObject.transform);
        }
    }

    public void RoomEqual(GameObject gameObject)
    {
        RoomObject = gameObject;
    }

    public void HealtDisCount(int power)
    {
        int remainingDamage = 0;
        if (CurrentDefence > 0)
        {
            if (power > CurrentDefence)
            {
                remainingDamage = power - CurrentDefence;
                CurrentDefence = 0;
            }
            else
            {
                CurrentDefence -= power;
            }
        }
        else if (CurrentDefence == 0 && CurrentHealth > 0)
        {
            if (power > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
            {
                CurrentHealth -= power;
            }

            if (remainingDamage != 0)
            {
                CurrentHealth -= remainingDamage;
            }
        }
    }

    #endregion
}