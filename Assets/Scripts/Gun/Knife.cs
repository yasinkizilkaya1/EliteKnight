using UnityEngine;

public class Knife : MonoBehaviour
{
    #region Constants

    private const string TAG_ENEMY = "Enemy";

    #endregion

    #region Fields

    public bool isattack;

    public int Power;

    public Animator Attack;
    public Animator custom;
    public Character character;

    #endregion

    public int test;

    #region Unity Methods

    private void Update()
    {
        test = character.CharacterWay;

        if (custom != null && Attack != null)
        {
            custom.SetFloat("Legs", character.CharacterWay);
            Attack.SetBool("attack", isattack);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_ENEMY))
        {
            if (collider.GetComponentInChildren<Zombies>())
            {
                collider.GetComponentInChildren<Zombies>().DisHealth(Power);
            }
            else if (collider.GetComponent<TowerWeapon>())
            {
                collider.GetComponent<TowerWeapon>().HealtDisCount(Power);
            }
            else if (collider.GetComponentInChildren<WarriorEnemy>())
            {
                collider.GetComponentInChildren<WarriorEnemy>().DisHealth(Power);
            }
        }
    }

    #endregion
}