using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Contants

    private const string mTAG_WALL = "wall";
    private const string mTAG_CHEST = "chest";
    private const string mTAG_ENEMY = "Enemy";

    #endregion

    #region Fields

    public Weapon Weapon;

    public int Speed;
    public float Range;
    public SpriteRenderer SpriteRenderer;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        SetActiveObje();
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(mTAG_WALL))
        {
            gameObject.SetActive(false);
        }
        else if (collider.CompareTag(mTAG_CHEST))
        {
            collider.GetComponent<Chest>().DisHealth(Weapon.Power);
            gameObject.SetActive(false);
        }
        else if (collider.CompareTag(mTAG_ENEMY))
        {
            if (collider.GetComponentInParent<Zombies>())
            {
                collider.GetComponentInParent<Zombies>().DisHealth(Weapon.Power);
            }
            else if (collider.GetComponent<TowerWeapon>())
            {
                collider.GetComponent<TowerWeapon>().HealtDisCount(Weapon.Power);  //look at here
            }
            else if (collider.GetComponentInParent<WarriorEnemy>())
            {
                collider.GetComponentInParent<WarriorEnemy>().DisHealth(Weapon.Power);
            }
            else if(collider.GetComponentInParent<SpaceShip>())
            {
                collider.GetComponentInParent<SpaceShip>().DisHealth(Weapon.Power);
            }
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        if (Weapon != null)
        {
            SpriteRenderer.sprite = Weapon.Bullet;
            Range = Weapon.Range;
        }
    }

    private void SetActiveObje()
    {
        Range -= Time.deltaTime;

        if (Range <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    #endregion
}