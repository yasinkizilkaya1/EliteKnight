using System.Collections;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    #region Constant

    private const string TAG_TARGET = "Player";

    #endregion

    #region Fields

    public Spawn spawn;
    public TowerWeapon towerWeapon;

    public bool inside;
    public bool isStandartTower;

    #endregion

    #region Unity Methods

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_TARGET))
        {
            inside = true;

            if (isStandartTower && towerWeapon.CanAttack)
            {
                StartCoroutine(Fire());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_TARGET))
        {
            inside = false;
            StopAllCoroutines();
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Fire()
    {
        while (true)
        {
            if (spawn.listCharacterList[0].isDead == false)
            {
                towerWeapon.Attack(true);
                yield return new WaitForSeconds(1f);
            }
            else
                break;
        }
    }
    
    #endregion
}