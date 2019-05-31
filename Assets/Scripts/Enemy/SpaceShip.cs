using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShip : MonoBehaviour
{
    #region Constants

    private const string mTAG_ROCKET = "EnemyRocket";
    private const string mTAG_ENEMYBULLET = "EnemyBullet";
    private const string mTAG_CHARACTER = "Character";
    private const string mTAG_SPAWN_BULLET = "EnemySpawnBullet";
    private const string mTAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    public Boss BossData;

    public int CurrentHealth;
    public int MaxHealth;
    public float AttackTime;
    private float mMaxAttackTime;
    public float ShootcoolDown;
    public float ShootAngle;
    private float mWaitTime;
    private int mAttackId;

    public List<GameObject> Barrels;
    public List<GameObject> Enemys;
    public List<int> AttackIds;

    public GameManager GameManager;
    public UIManager UIManager;
    public Slider HealthSlider;
    public Room Room;

    #endregion

    #region Property

    public bool attack
    {
        get
        {
            return ShootcoolDown <= 0f;
        }
    }

    public bool wait
    {
        get
        {
            return mWaitTime <= 0;

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
        if (HealthSlider != null)
        {
            HealthSlider.value = CurrentHealth;
        }

        if (GameManager.Character != null)
        {
            Aim(this.gameObject);

            Attack();

            if (ShootcoolDown > 0f)
            {
                ShootcoolDown -= Time.deltaTime;
            }

            if (AttackTime > 0)
            {
                AttackTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("knife"))
        {
            DisHealth(1);
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        UIManager = GameManager.UIManager;
        GameManager.IsBossSpawn = true;
        HealthSlider = UIManager.BossHealthBarSlider;
        CurrentHealth = BossData.Health;
        MaxHealth = BossData.MaxHealth;
        AttackTime = BossData.AttackTime;
        mMaxAttackTime = AttackTime;
        HealthSlider.value = CurrentHealth;
        HealthSlider.maxValue = MaxHealth;
        ShootAngle = BossData.ShootAngle / 2;
    }

    private void Aim(GameObject TurnedObject)
    {
        TurnedObject.transform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, TurnedObject.transform);
    }

    private void Attack()
    {
        if (AttackTime > 0f)
        {
            if (attack)
            {
                switch (mAttackId)
                {
                    case 0:
                        Shoot(mTAG_ENEMYBULLET);
                        ShootcoolDown = BossData.ShootcoolDowns[mAttackId];
                        break;
                    case 1:
                        Shoot(mTAG_ROCKET);
                        ShootcoolDown = BossData.ShootcoolDowns[mAttackId];
                        break;
                    default:
                        foreach (GameObject barrel in Barrels)
                        {
                            BarrelAngleSetting(barrel);
                            ShotBullet(mTAG_SPAWN_BULLET, barrel);
                        }
                        ShootcoolDown = BossData.ShootcoolDowns[mAttackId];
                        break;
                }
            }
        }
        else
        {
            mWaitTime -= Time.deltaTime;

            if (mWaitTime <= 0f)
            {
                if (AttackIds.Count > 2)
                {
                    int attackId = Random.Range(0, BossData.ShootcoolDowns.Count);
                    mAttackId = attackId == AttackIds[AttackIds.Count - 1] ? Random.Range(0, BossData.ShootcoolDowns.Count) : attackId;
                }
                else
                {
                    mAttackId = Random.Range(0, BossData.ShootcoolDowns.Count);
                }

                AttackIds.Add(mAttackId);
                AttackTime = mMaxAttackTime;
                mWaitTime = BossData.WaitTime;
            }
        }
    }

    private void BarrelAngleSetting(GameObject barrel)
    {
        bool IsPositif = Random.value < 0.5 ? true : false;
        float RandomAngle = Random.Range(0, ShootAngle / 2);
        Aim(barrel);

        if (IsPositif)
        {
            barrel.transform.Rotate(0, 0, barrel.transform.rotation.z + RandomAngle);
        }
        else
        {
            barrel.transform.Rotate(0, 0, barrel.transform.rotation.z - RandomAngle);
        }
    }

    private void Shoot(string Tag)
    {
        foreach (GameObject barrel in Barrels)
        {
            BarrelAngleSetting(barrel);
            ShotBullet(Tag, barrel);
        }
    }

    private void ShotBullet(string tag, GameObject barrel)
    {
        GameObject Bullet = ObjectPooler.SharedInstance.GetPooledObject(tag);

        if (Bullet != null)
        {
            Bullet.transform.position = barrel.transform.position;
            Bullet.transform.rotation = barrel.transform.rotation;
            Bullet.SetActive(true);
            Bullet.GetComponent<BossBullet>().GameManager = GameManager;
            Bullet.GetComponent<BossBullet>().SpaceShip = this;
            Bullet.GetComponent<BossBullet>().Room = Room;
        }
    }

    private void Dead()
    {
        foreach (GameObject Object in Enemys)
        {
            Destroy(Object);
        }

        Room.EnemyCount--;
        UIManager.BossHealthBarSlider.gameObject.SetActive(false);
        GameManager.IsBossSpawn = false;
        Destroy(this.gameObject);
    }

    #endregion

    #region Public Method

    public void DisHealth(int power)
    {
        if (CurrentHealth > 0 && CurrentHealth - power > 0)
        {
            FloatingTextController.CreateFloatingText(power.ToString(), transform);
            CurrentHealth -= power;
        }
        else
        {
            Dead();
        }
    }

    public void RoomEqual(Room room)
    {
        Room = room;
    }

    #endregion
}