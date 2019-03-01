using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region Constants

    private const string TAG_SPAWN = "Spawn";
    private const string TAG_CHARACTER = "Character";
    private const string TAG_WALL = "wall";
    private const string TAG_CHEST = "chest";
    private const int VELOCIDADE = 10;

    #endregion

    #region Fields

    public Spawn spawn;
    public Transform TargetTransform;

    public bool isEffectTowerBullet;
    public bool isBullet;

    public GameObject bulletObject;
    public List<GameObject> BarrelList;
    public GameObject StandartTowerBulletObject;

    public int EffectTowerBulletPower;
    public int NormalBulletPower;
    public int FollowBulletPower;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (spawn.CharacterList[0] != null)
        {
            if (isEffectTowerBullet)
            {
                Destroy(gameObject, 3);
                transform.Translate(Vector2.right * -VELOCIDADE * Time.deltaTime);
            }
            else if (isBullet && spawn.CharacterList[0].isDead == false)
            {
                transform.Translate(Vector2.right * -VELOCIDADE * Time.deltaTime);
            }
            else if (isEffectTowerBullet == false && spawn.CharacterList[0].isDead == false)
            {
                transform.Translate(Vector2.right * -VELOCIDADE * Time.deltaTime);
                StandartTowerBulletObject.transform.rotation = ScriptHelper.LookAt2D(TargetTransform, transform);
                Destroy(gameObject, 1.5f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_CHEST))
        {
            Destroy(gameObject);
        }
        else if (collider.CompareTag(TAG_WALL))
        {
            if (isEffectTowerBullet)
            {
                for (int i = 0; i < BarrelList.Count; i++)
                {
                   GameObject bullet= Instantiate(bulletObject, BarrelList[i].transform.position, Quaternion.identity);
                    bullet.transform.rotation = BarrelList[i].transform.rotation;
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (collider.CompareTag(TAG_CHARACTER))
        {
            if (isEffectTowerBullet)
            {
                spawn.CharacterList[0].HealthDisCount(EffectTowerBulletPower);
            }
            else if (isBullet)
            {
                spawn.CharacterList[0].HealthDisCount(NormalBulletPower);
            }
            else
            {
                spawn.CharacterList[0].HealthDisCount(FollowBulletPower);
            }
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        spawn = GameObject.FindWithTag(TAG_SPAWN).GetComponent<Spawn>();
        TargetTransform = spawn.CharacterList[0].transform;
    }

    #endregion
}