using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Constants

    private const string TAG_WEAPON = "Barrel";
    private const string TAG_CHARACTER = "Assault";

    #endregion

    #region Fields

    public TiroNew tiroNew;
    public Character character;

    public GameObject PistolObject;
    public GameObject WoodObject;
    public GameObject Ak47Object;
    public GameObject RightWeaponObject;

    public bool isPistolGun;
    public bool isWoodGun;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (tiroNew == null)
        {
            tiroNew = GameObject.FindWithTag(TAG_WEAPON).GetComponent<TiroNew>();
        }

        if (character.isFindAk47 && character.name==TAG_CHARACTER)
        {
            Destroy(RightWeaponObject);
            RightWeaponObject = Instantiate(Ak47Object, transform);
            character.isFindAk47 = false;
        }
    }

    #endregion

    #region Private Method

    private void Initialize()
    {
        if (isPistolGun)
        {
            RightWeaponObject = Instantiate(PistolObject, transform);
        }
        else if (isWoodGun)
        {
            RightWeaponObject = Instantiate(WoodObject, transform);
        }
    }
    
    #endregion
}