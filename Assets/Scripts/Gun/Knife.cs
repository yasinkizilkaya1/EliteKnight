using UnityEngine;

public class Knife : MonoBehaviour
{
    #region Constants

    private const string mTAG_ENEMY = "Enemy";

    #endregion

    #region Fields

    public bool IsAttack;

    public int Power;

    public Animator Attack;
    public Animator Custom;
    public Character Character;

    #endregion

    public int test;

    #region Unity Methods

    private void Update()
    {
        test = Character.CharacterWay;

        if (Custom != null && Attack != null)
        {
            Custom.SetFloat("Legs", Character.CharacterWay);
            Attack.SetBool("attack", IsAttack);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(mTAG_ENEMY))
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