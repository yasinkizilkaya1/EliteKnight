using System.Collections;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    #region Constant

    private const string mTAG_TARGET = "Character";
    private const string mTAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    public TowerWeapon TowerWeapon;
    public GameManager GameManager;

    public bool Inside;
    public bool IsStandartTower;

    public int SlowPower;

    #endregion

    #region Private Method

    private void Init()
    {
        GameManager = GameObject.FindWithTag(mTAG_GAMEMANAGER).GetComponent<GameManager>();
    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(mTAG_TARGET))
        {
            Inside = true;

            if (TowerWeapon.IsLinerenderer)
            {
                GameManager.Character.SlowDown(Inside, SlowPower, TowerWeapon.Tower.AttackTime);
            }

            if (IsStandartTower && TowerWeapon.CanAttack)
            {
                StartCoroutine(Fire());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.gameObject.CompareTag(mTAG_TARGET))
        {
            Inside = false;
            GameManager.Character.SlowDown(Inside, SlowPower, TowerWeapon.Tower.AttackTime);
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
                TowerWeapon.Attack(true);
                yield return new WaitForSeconds(1f);
            }
            else
                break;
        }
    }

    #endregion
}