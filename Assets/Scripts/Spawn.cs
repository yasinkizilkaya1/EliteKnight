using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    #region Constants

    private const string TAG_CHARACTER = "Body";
    private const string TAG_GAMEMANAGER = "selectname";
    private const string TAG_CHARACTERNAME = "Custom";
    private const string TAG_WEAPON = "knife";
    private const string TAG_ASSAULT = "Assault";
    private const string TAG_SPEALIST = "Spealist";
    private const string TAG_SUPPORT = "Support";

    #endregion

    #region Fields

    public GameObject TransformObject;
    public GameObject SupportObject;
    public GameObject SpealistObject;
    public GameObject AssaultObject;
    public GameManager gameManager;

    public List<Character> CharacterList;

    public Weapon weapons;

    public Slider HealthBarSlider;
    public Slider ArmorBarSlider;

    public GameObject HealthgameObject;
    public GameObject ArmorgameObject;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (CharacterList[0].isDead == false)
        {
            HealthBarSlider.value = CharacterList[0].CurrentHP;
            ArmorBarSlider.value = CharacterList[0].CurrentDefence;
            StartCoroutine(SetActiveBars());
        }
        else
        {
            StopAllCoroutines();
            HealthgameObject.SetActive(false);
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        CharacterCreate();
        StartCoroutine(Bars());
        CharacterList.Add(GameObject.FindWithTag(TAG_CHARACTER).GetComponent<Character>());

        if (CharacterList[0].name == TAG_CHARACTERNAME)
        {
            weapons = GameObject.FindWithTag(TAG_WEAPON).GetComponent<Weapon>();
        }
    }

    private void CharacterCreate()
    {
        switch (gameManager.SelectedCardNameString)
        {
            case TAG_ASSAULT:
                TransformObject = Instantiate(AssaultObject, transform);
                break;
            case TAG_SPEALIST:
                TransformObject = Instantiate(SpealistObject, transform);
                break;
            case TAG_SUPPORT:
                TransformObject = Instantiate(SupportObject, transform);
                break;
            default:
                Debug.Log("Spawn Null!!!");
                break;
        }
    }

    #endregion

    #region Enumerator Methods

    IEnumerator Bars()
    {
        yield return new WaitForSeconds(0.1f);
        HealthBarSlider.maxValue = CharacterList[0].MaxHP;
        HealthBarSlider.value = CharacterList[0].characterData.Health;
        ArmorBarSlider.maxValue = CharacterList[0].characterData.Defence;
        ArmorBarSlider.value = CharacterList[0].characterData.Defence;
        StopAllCoroutines();
    }

    IEnumerator SetActiveBars()
    {
        yield return new WaitForSeconds(0.2f);
        if (CharacterList[0].CurrentDefence != CharacterList[0].MaxDefance)
        {
            HealthgameObject.SetActive(true);
            ArmorgameObject.SetActive(true);
        }

        if (ArmorBarSlider.value == 0)
        {
            ArmorgameObject.SetActive(false);
        }
    }

    #endregion
}