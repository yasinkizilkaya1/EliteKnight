using UnityEngine;

public class Knife : MonoBehaviour
{
    #region Constants

    private const float ATTTACK_TIME = 0.4f;
    private const string TAG_CHARACTER = "Body";
    private const string TAG_ENEMY = "Enemy";
    private const string TAG_TOWER = "TowerEnemySlider";

    #endregion

    #region Fields

    public bool isAssault;
    public bool isattack;

    public int Power;
    public float AttackTime;

    public Animator Attack;
    public Animator custom;
    public Character character;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        custom.SetFloat("Legs", character.CharacterWay);
        Attack.SetBool("attack", isattack);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TAG_ENEMY) || collider.CompareTag(TAG_TOWER))
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

    #region Private Methods

    private void Initialize()
    {
        character = GameObject.FindWithTag(TAG_CHARACTER).GetComponent<Character>();
    }

    #endregion

    #region Public Method

    public void CharacterAnimationAttack()
    {
        if (Input.GetButtonDown("Fire1") == false)
        {
            isattack = false;
        }

        isattack = true;
        isAssault = true;
        AttackTime -= Time.deltaTime;

        if (AttackTime <= 0.1)
        {
            isAssault = false;
            AttackTime = ATTTACK_TIME;
        }

        if (isAssault == true)
        {
            AttackTime -= Time.deltaTime;
        }

        if (AttackTime <= 0.1)
        {
            isAssault = false;
            AttackTime = ATTTACK_TIME;
        }
    }

    #endregion
}