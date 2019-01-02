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
    public GameObject ShotgunObject;
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
        else
        {
            if (tiroNew.SpareBulletCount == 0)
            {
                Destroy(RightWeaponObject);
                RightWeaponObject = Instantiate(PistolObject, transform);
                character.isAk47 = false;
                character.isShotgun = false;
                character.isShotgunUse = false;
                character.isGun = true;
            }
        }

        if (character.isAk47 && character.name == TAG_CHARACTER)
        {
            Destroy(RightWeaponObject);
            RightWeaponObject = Instantiate(Ak47Object, transform);
            character.isAk47 = false;
            character.isShotgun = false;
            character.isShotgunUse = false;
            character.isGun = false;
        }
        else if (character.isShotgun && character.name == TAG_CHARACTER)
        {
            Destroy(RightWeaponObject);
            RightWeaponObject = Instantiate(ShotgunObject, transform);
            character.isShotgun = false;
            character.isAk47 = false;
            character.isGun = false;
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