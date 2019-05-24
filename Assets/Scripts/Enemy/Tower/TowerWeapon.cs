using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    #region Constants

    private const string mTAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    public GameManager GameManager;

    public SpriteRenderer SpriteRenderer;
    private Color mColor;
    public LineRenderer LineRenderer;
    public TowerEnemy TowerEnemy;
    public Tower Tower;

    public Transform ShotPrefabTransform;
    public Transform OriginTransform;

    public Room Room;
    public GameObject TowerObject;
    public GameObject BarrelObject;

    public float ShootCoolDown;

    public int CurrentHealth;
    public int CurrentDefence;

    public bool IsLinerenderer;

    #endregion

    #region Property

    public bool CanAttack
    {
        get
        {
            return ShootCoolDown <= 0f;
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

        if (ShootCoolDown > 0)
        {
            ShootCoolDown -= Time.deltaTime;
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        mColor = SpriteRenderer.color;
        ShootCoolDown = this.Tower.AttackTime;
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        CurrentHealth = Tower.Health;
        CurrentDefence = Tower.Defence;
    }

    private void TowerMoving()
    {
        if (GameManager.Character.isDead == false && TowerEnemy.Inside && LineRenderer == null)
        {
            Moving();
        }
        else if (GameManager.Character.isDead == false && LineRenderer != null)
        {
            Moving();
        }
    }

    private void TowerLinerender()
    {
        if (GameManager.Character.isDead == false && IsLinerenderer == true && TowerEnemy.Inside)
        {
            LineRenderer.SetPosition(0, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
            LineRenderer.SetPosition(1, new Vector3(GameManager.Character.transform.position.x, GameManager.Character.transform.position.y, 1));
        }
        else
        {
            if (IsLinerenderer == true)
            {
                LineRenderer.SetPosition(0, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
                LineRenderer.SetPosition(1, new Vector3(OriginTransform.position.x, OriginTransform.position.y, 1));
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
            ShootCoolDown = this.Tower.AttackTime;

            if (IsLinerenderer)
            {
                GameManager.Character.HealthDisCount(TowerEnemy.SlowPower);
            }
            else
            {
                var shootTransformObject = Instantiate(ShotPrefabTransform) as Transform;
                shootTransformObject.position = BarrelObject.transform.position;
                shootTransformObject.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, shootTransformObject.transform);
            }
        }
    }

    public void RoomEqual(Room room)
    {
        Room = room;
    }

    public void DisHealt(int power)
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
        else if (CurrentHealth > 0)
        {
            if (power > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
            {
                CurrentHealth -= power;
            }

        }

        if (remainingDamage != 0)
        {
            CurrentHealth -= remainingDamage;
        }

        if (CurrentHealth == 0)
        {
            TowerEnemy.Inside = false;
            Destroy(gameObject);
            GameManager.Character.DeadEnemyCount++;

            if (Room != null)
            {
                Room.EnemyCount--;
            }
        }
        else
        {
            FloatingTextController.CreateFloatingText(power.ToString(), transform);
        }
    }

    #endregion
}