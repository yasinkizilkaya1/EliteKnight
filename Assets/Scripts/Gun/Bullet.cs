using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Constants

    private const string TAG_SPAWN = "Spawn";
    private const string TAG_CHARACTER = "Body";
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
    public GameObject LocationObject1;
    public GameObject LocationObject2;
    public GameObject LocationObject3;
    public GameObject LocationObject4;
    public GameObject LocationObject5;
    public GameObject LocationObject6;
    public GameObject LocationObject7;
    public GameObject LocationObject8;
    public GameObject StandartTowerBulletObject;

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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_CHEST))
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag(TAG_WALL))
        {
            if (isEffectTowerBullet)
            {
                Instantiate(bulletObject, transform.position, LocationObject1.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject2.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject3.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject4.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject5.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject6.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject7.transform.rotation);
                Instantiate(bulletObject, transform.position, LocationObject8.transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
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