using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region Constants

    private const string mTAG_GAMEMANAGER = "GameManager";
    private const string mTAG_CHARACTER = "Character";
    private const string mTAG_WALL = "wall";
    private const string mTAG_CHEST = "chest";
    private const int mVELOCIDADE = 10;

    #endregion

    #region Fields

    public Transform TargetTransform;

    public bool IsEffectTowerBullet;
    public bool IsBullet;

    private GameManager mGameManager;

    public GameObject BulletObject;
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
        if (mGameManager.Character.isDead == false)
        {
            if (IsEffectTowerBullet)
            {
                Destroy(gameObject, 3);
                transform.Translate(Vector2.right * -mVELOCIDADE * Time.deltaTime);
            }
            else if (IsBullet && mGameManager.Character.isDead == false)
            {
                transform.Translate(Vector2.right * -mVELOCIDADE * Time.deltaTime);
            }
            else if (IsEffectTowerBullet == false && mGameManager.Character.isDead == false)
            {
                transform.Translate(Vector2.right * -mVELOCIDADE * Time.deltaTime);
                StandartTowerBulletObject.transform.rotation = ScriptHelper.LookAt2D(TargetTransform, transform);
                Destroy(gameObject, 1.5f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag(mTAG_CHEST) || collider.CompareTag(mTAG_WALL))
        {
            if (IsEffectTowerBullet)
            {
                for (int i = 0; i < Barrels.Count; i++)
                {
                    GameObject bullet = Instantiate(BulletObject, Barrels[i].transform.position, Barrels[i].transform.rotation);
                }
            }

            Destroy(gameObject);
        }
        else if (collider.CompareTag(mTAG_CHARACTER))
        {
            if (IsEffectTowerBullet)
            {
                mGameManager.Character.HealthDisCount(EffectTowerBulletPower);
            }
            else if (IsBullet)
            {
                mGameManager.Character.HealthDisCount(NormalBulletPower);
            }
            else
            {
                mGameManager.Character.HealthDisCount(FollowBulletPower);
            }
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        mGameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
        TargetTransform = mGameManager.Character.transform;
    }

    #endregion
}