using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    #region Contants

    private const string mTAG_BULLET = "bullet";
    private const string mTAG_CHARACTER = "Character";
    private const string mTAG_CHEST = "chest";
    private const string mTAG_WALL = "wall";
    private const string mTAG_ENEMY = "Enemy";

    #endregion

    #region Fields

    public GameManager GameManager;
    public Room Room;

    public List<GameObject> Enemys;
    public SpaceShip SpaceShip;

    public bullet Bullet;
    public int Power;
    public int Speed;
    public float DropTime;
    public bool IsEnemyCreate;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        Initialize();
    }

    private void Update()
    {
        if (SpaceShip == null)
        {
            this.gameObject.SetActive(false);
        }

        if (Bullet.IsEnemyCreate)
        {
            if (DropTime < 0f)
            {
                EnemyCreate(Enemys[Random.Range(0, Enemys.Count - 1)]);
            }
            else
            {
                DropTime -= Time.deltaTime;
                transform.Translate(Vector2.right * -DropTime * Time.deltaTime);
            }
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

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag(mTAG_WALL) || collider2D.CompareTag(mTAG_CHEST))
        {
            if (IsEnemyCreate)
            {
                transform.Rotate(0f, 0f, 10f);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        else if (collider2D.CompareTag(mTAG_CHARACTER) && IsEnemyCreate == false)
        {
            GameManager.Character.HealthDisCount(Power);
            this.gameObject.SetActive(false);
        }
        else if (collider2D.CompareTag(mTAG_BULLET) && Bullet.IsExplode == true)
        {
            this.gameObject.SetActive(false);
        }
        else if (collider2D.CompareTag(mTAG_ENEMY) && IsEnemyCreate)
        {
            transform.Rotate(0f, 0f, 10f);
            DropTime = 6;
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        IsEnemyCreate = Bullet.IsEnemyCreate;
        Power = Bullet.Power;
        Speed = Bullet.Speed;
        DropTime = Random.Range(6, 10);
    }

    private void Aim(GameObject TurnedObject)
    {
        if (GameManager.Character != null)
        {
            TurnedObject.transform.rotation = ScriptHelper.LookAt2D(GameManager.Character.transform, TurnedObject.transform);
        }
        else
        {
            TurnedObject.transform.Rotate(0, 0, 5);
        }
    }

    private void EnemyCreate(GameObject Enemy)
    {
        GameObject enemy = Instantiate(Enemy, Room.transform);
        enemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        SpaceShip.Enemys.Add(enemy);
        this.gameObject.SetActive(false);
        IsEnemyCreate = false;
    }

    #endregion
}