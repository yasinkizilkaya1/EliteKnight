using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Constants

    private const float ATTTACK_TİME = 0.4f;
    private const string TAG_CHARACTER = "Body";

    #endregion

    #region Fields

    public bool isAssault;
    public bool isattack;

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
        CharacterAnimationAttack();
        custom.SetFloat("Legs", character.CharacterWay);
        Attack.SetBool("attack", isattack);
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        character = GameObject.FindWithTag(TAG_CHARACTER).GetComponent<Character>();
    }

    private void CharacterAnimationAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isattack = true;
            isAssault = true;
            AttackTime -= Time.deltaTime;

            if (AttackTime <= 0.1)
            {
                isAssault = false;
                AttackTime = ATTTACK_TİME;
            }
        }
        else
        {
            isattack = false;
        }

        if (isAssault == true)
        {
            AttackTime -= Time.deltaTime;
        }

        if (AttackTime <= 0.1)
        {
            isAssault = false;
            AttackTime = ATTTACK_TİME;
        }
    }

    #endregion
}