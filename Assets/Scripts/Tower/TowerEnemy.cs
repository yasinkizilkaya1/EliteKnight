using System.Collections;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    #region Constant

    private const string TAG_TARGET = "Player";
    private const string TAG_SPAWN = "Spawn";

    #endregion

    #region Fields

    public TowerWeapon towerWeapon;
    public Spawn spawn;

    public bool inside;
    public bool isStandartTower;

    #endregion

    #region Private Method

    private void Init()
    {
        spawn = GameObject.FindWithTag(TAG_SPAWN).GetComponent<Spawn>();
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_TARGET))
        {
            inside = true;

            if (towerWeapon.isLinerenderer)
            {
                spawn.CharacterList[0].SlowDown(inside);
            }

            if (isStandartTower && towerWeapon.CanAttack)
            {
                StartCoroutine(Fire());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.gameObject.CompareTag(TAG_TARGET))
        {
            inside = false;
            spawn.CharacterList[0].SlowDown(inside);
            StopAllCoroutines();
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Fire()
    {
        while (true)
        {
            if (spawn.CharacterList[0].isDead == false)
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