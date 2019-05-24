using System.Collections;
using UnityEngine;

public class TowerEnemy : MonoBehaviour
{
    #region Constant

    private const string mTAG_TARGET = "Character";

    #endregion

    #region Fields

    public TowerWeapon TowerWeapon;
    public GameManager GameManager;

    public bool Inside;
    public bool IsStandartTower;

    public int SlowPower;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag(mTAG_TARGET))
        {
            Inside = true;

            if (TowerWeapon.LineRenderer)
            {
                GameManager.Character.Speed = 2;
            }

            StartCoroutine(Fire());
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag(mTAG_TARGET))
        {
            Inside = false;
            GameManager.Character.Speed = GameManager.Character.characterData.Speed;
            StopAllCoroutines();
        }
    }

    #endregion

    #region Private Method

    private void Init()
    {
        GameManager = TowerWeapon.GameManager;
    }

    #endregion

    #region Enumerator Method

    IEnumerator Fire()
    {
        while (true)
        {
            if (GameManager.Character.isDead == false && TowerWeapon.CanAttack)
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