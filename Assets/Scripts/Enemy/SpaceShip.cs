using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Character";
    private const string TAG_ROCKET = "EnemyRocket";
    private const string TAG_BULLET = "EnemyBullet";
    private const string TAG_SPAWN_BULLET = "EnemySpawnBullet";

    #endregion

    #region Fields

    public Boss BossData;

    public int CurrentHealth;
    public int MaxHealth;
    public float AttackTime;
    private float MaxAttackTime;
    public float shootcoolDown;

    public List<GameObject> Barrels;

    public GameManager GameManager;

    #endregion

    #region Property

    public bool attack
    {
        get
        {
            return shootcoolDown <= 0f;
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
        Aim(this.gameObject);

        for (int i = 0; i < Barrels.Count; i++)
        {
            Aim(Barrels[i]);
        }

        Attack();

        if (shootcoolDown > 0f)
        {
            shootcoolDown -= Time.deltaTime;
        }

        if (AttackTime > 0)
        {
            AttackTime -= Time.deltaTime;
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        CurrentHealth = BossData.Health;
        MaxHealth = BossData.MaxHealth;
        AttackTime = BossData.AttackTime;
        MaxAttackTime = AttackTime;
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
                for (int i = 0; i < 2; i++)
                {
                    ShotBullet(TAG_BULLET, Barrels[i]);
                }
                shootcoolDown = BossData.shootcoolDown;
            }
        }
        else
        {
            for (int i = 3; i < 5; i++)
            {
                ShotBullet(TAG_ROCKET, Barrels[i]);
            }

            //ShotBullet(TAG_SPAWN_BULLET,Barrels[4]);
            AttackTime = MaxAttackTime;
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
        }
    }

    #endregion
}