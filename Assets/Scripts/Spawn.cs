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

    public GameObject SupportObject;
    public GameObject SpealistObject;
    public GameObject AssaultObject;
    public GameManager gameManager;

    public List<Character> CharacterList;

    public Knife knife;

    public Slider HealthBarSlider;
    public Slider ArmorBarSlider;
    public Slider EnergyBarSlider;

    public GameObject HealthgameObject;
    public GameObject ArmorgameObject;
    public GameObject EnergygameObject;

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
            EnergyBarSlider.value = CharacterList[0].Energy;
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
            knife = GameObject.FindWithTag(TAG_WEAPON).GetComponent<Knife>();
        }
    }

    private void CharacterCreate()
    {
       GameObject TransformObject = gameObject;

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
        EnergygameObject.SetActive(true);
        HealthBarSlider.maxValue = CharacterList[0].MaxHP;
        HealthBarSlider.value = CharacterList[0].characterData.Health;
        ArmorBarSlider.maxValue = CharacterList[0].characterData.Defence;
        ArmorBarSlider.value = CharacterList[0].characterData.Defence;
        EnergyBarSlider.maxValue = CharacterList[0].characterData.MaxEnergy;
        EnergyBarSlider.value = CharacterList[0].characterData.Energy;
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