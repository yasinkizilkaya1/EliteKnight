using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region Constants

    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_CHARACTER = "Character";
    private const string TAG_WALL = "wall";
    private const string TAG_CHEST = "chest";
    private const int VELOCIDADE = 10;

    #endregion

    #region Fields

    public Transform TargetTransform;

    public bool isEffectTowerBullet;
    public bool isBullet;

    private GameManager GameManager;

    public GameObject bulletObject;
    public List<GameObject> Barrels;
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
        if (GameManager.Character.isDead == false)
        {
            if (isEffectTowerBullet)
            {
                Destroy(gameObject, 3);
                transform.Translate(Vector2.right * -VELOCIDADE * Time.deltaTime);
            }
            else if (isBullet && GameManager.Character.isDead == false)
            {
                transform.Translate(Vector2.right * -VELOCIDADE * Time.deltaTime);
            }
            else if (isEffectTowerBullet == false && GameManager.Character.isDead == false)
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
                for (int i = 0; i < Barrels.Count; i++)
                {
                   GameObject bullet= Instantiate(bulletObject, Barrels[i].transform.position, Quaternion.identity);
                    bullet.transform.rotation = Barrels[i].transform.rotation;
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
                GameManager.Character.HealthDisCount(EffectTowerBulletPower);
            }
            else if (isBullet)
            {
                GameManager.Character.HealthDisCount(NormalBulletPower);
            }
            else
            {
                GameManager.Character.HealthDisCount(FollowBulletPower);
            }
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        TargetTransform = GameManager.Character.transform;
    }

    #endregion
}