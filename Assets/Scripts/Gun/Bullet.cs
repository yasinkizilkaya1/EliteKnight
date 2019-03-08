using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Contants

    private const string TAG_WALL = "wall";
    private const string TAG_CHEST = "chest";
    private const string TAG_ENEMY = "Enemy";

    #endregion

    #region Fields

    public Weapon weapon;

    public int Speed;
    public float range;

    #endregion

    #region Unity Methods

    private void Update()
    {
        SetActiveObje();
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_WALL) || collider.CompareTag(TAG_CHEST))
        {
            gameObject.SetActive(false);
        }
        else if (collider.CompareTag(TAG_ENEMY))
        {
            if (collider.GetComponentInParent<Zombies>())
            {
                collider.GetComponentInParent<Zombies>().DisHealth(weapon.Power);
            }
            else if (collider.GetComponent<TowerWeapon>())
            {
                collider.GetComponent<TowerWeapon>().HealtDisCount(weapon.Power);  //look at here
            }
            else if (collider.GetComponentInParent<WarriorEnemy>())
            {
                collider.GetComponentInParent<WarriorEnemy>().DisHealth(weapon.Power);
            }
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Private Method

    private void SetActiveObje()
    {
        range -= Time.deltaTime;

        if (range <= 0)
        {
            range =weapon.Range;
            gameObject.SetActive(false);
        }
    }

    #endregion
}