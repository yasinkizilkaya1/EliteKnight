using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Contants

    private const string TAG_WALL = "wall";
    private const string TAG_CHEST = "chest";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_TOWER = "TowerEnemySlider";

    #endregion

    #region Fields

    public Gun weapon;

    public int Speed;

    #endregion

    #region Unity Methods

    private void Update()
    {
        transform.Translate(Vector2.right * -Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_WALL) || collider.CompareTag(TAG_CHEST))
        {
            Destroy(gameObject);
        }
        else if (collider.CompareTag(TAG_ENEMY) || collider.CompareTag(TAG_TOWER))
        {
            if (collider.GetComponentInChildren<Zombies>())
            {
                collider.GetComponentInChildren<Zombies>().DisHealth(weapon.gun.Power);
            }
            else if (collider.GetComponent<TowerWeapon>())
            {
                collider.GetComponent<TowerWeapon>().HealtDisCount(weapon.gun.Power);
            }
            else if (collider.GetComponentInChildren<WarriorEnemy>())
            {
                collider.GetComponentInChildren<WarriorEnemy>().DisHealth(weapon.gun.Power);
            }

            Destroy(gameObject);
        }
    }

    #endregion
}