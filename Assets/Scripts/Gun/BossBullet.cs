using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    #region Contants

    private const string TAG_BULLET = "bullet";
    private const string TAG_CHARACTER = "Character";
    private const string TAG_CHEST = "chest";
    private const string TAG_WALL = "wall";

    #endregion

    #region Fields

    public GameManager GameManager;

    public List<GameObject> Enemys;

    public bullet Bullet;
    public int Power;
    public int Speed;
    public float DropTime;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (Bullet.IsEnemyCreate)
        {
            EnemyCreate(Enemys[RandomValueEqual(0, Enemys.Count - 1)]);
        }
        else
        {
            transform.Translate(Vector2.right * -Speed * Time.deltaTime);
        }

        if (Bullet.IsFollow)
        {
            Aim(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        //Debug.Log(collider2D.tag);

        if (collider2D.CompareTag(TAG_WALL))
        {
            this.gameObject.SetActive(false);
        }
        else if (collider2D.CompareTag(TAG_CHEST))
        {
            this.gameObject.SetActive(false);
        }
        else if (collider2D.CompareTag(TAG_CHARACTER))
        {
            GameManager.Character.HealthDisCount(Power);
            this.gameObject.SetActive(false);
        }
        else if (collider2D.CompareTag(TAG_BULLET))
        {
            this.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        Power = Bullet.Power;
        Speed = Bullet.Speed;
        DropTime = RandomValueEqual(8, 16);
    }

    private void Aim(GameObject TurnedObject)
    {
        TurnedObject.transform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, TurnedObject.transform);
    }

    private void EnemyCreate(GameObject Enemy)
    {
        if (DropTime > 0)
        {
            DropTime -= Time.deltaTime;
            transform.Translate(Vector2.right * -DropTime * Time.deltaTime);
        }
        else
        {
            Instantiate(Enemy);
            Bullet.IsEnemyCreate = false;
        }
    }

    private int RandomValueEqual(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }

    #endregion
}