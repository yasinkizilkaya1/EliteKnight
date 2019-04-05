using System.Collections;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    #region Constant

    private const string TAG_TARGET = "Character";
    private const string TAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    public TowerWeapon towerWeapon;
    public GameManager GameManager;

    public bool inside;
    public bool isStandartTower;

    public int SlowPower;

    #endregion

    #region Private Method

    private void Init()
    {
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
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
                GameManager.Character.SlowDown(inside, SlowPower, towerWeapon.tower.AttackTime);
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
            GameManager.Character.SlowDown(inside, SlowPower, towerWeapon.tower.AttackTime);
            StopAllCoroutines();
        }
    }

    #endregion

    #region Enumerator Method

    IEnumerator Fire()
    {
        while (true)
        {
            if (GameManager.Character.isDead == false)
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