using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region Constants

    private const string TAG_KNIFE = "knife";
    private const string TAG_SUPPORT = "Support";
    private const string TAG_GAMEMANAGER = "GameManager";
    private const string TAG_BULLET = "bullet";

    #endregion

    #region Fields

    public GameManager GameManager;
    public ChestEntity ChestEntity;
    public List<GameObject> ItemObjects;
    public List<Item> Items;

    public GameObject HealthBarObject;
    public GameObject HealthBarObject1;
    public GameObject HealthBarObject2;
    public GameObject HealthBarObject3;
    public GameObject DefanceBarObject1;
    public GameObject DefanceBarObject2;
    public GameObject DefanceBarObject3;
    public GameObject DefanceBarObject4;
    public GameObject DefanceBarObject5;

    public int Health;
    public int Defence;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (Defence)
        {
            case 5:
                ArmorObjectsSetActive(true, true, true, true, true, true);
                break;
            case 4:
                ArmorObjectsSetActive(false, true, true, true, true, true);
                break;
            case 3:
                ArmorObjectsSetActive(false, false, true, true, true, true);
                break;
            case 2:
                ArmorObjectsSetActive(false, false, false, true, true, true);
                break;
            case 1:
                ArmorObjectsSetActive(false, false, false, false, true, true);
                break;
            case 0:
                ArmorObjectsSetActive(false, false, false, false, false, true);
                break;
        }

        switch (Health)
        {
            case 3:
                HealthObjectsSetActive(true, true, true, true);
                break;
            case 2:
                HealthObjectsSetActive(false, true, true, true);
                break;
            case 1:
                HealthObjectsSetActive(false, false, true, true);
                break;
            case 0:
                HealthObjectsSetActive(false, false, false, false);

                if (ChestEntity.ItemDrop)
                {
                    HealthBarObject.SetActive(false);
                    Instantiate(ItemObjects[Random.Range(0, ItemObjects.Count - 1)], transform.position, transform.rotation);
                    GameManager.Chests.Remove(this);
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_KNIFE))
        {
            if (Defence > 0)
            {
                Defence--;
            }
            else if (Defence == 0)
            {
                Health--;
            }
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        name = ChestEntity.Name;
        Defence = ChestEntity.Defence;
        Health = ChestEntity.Health;
        GameManager.Chests.Add(this);
        CharacterHavingGunsUnload();

        if (GameManager.CharacterData.Name == TAG_SUPPORT)
        {
            foreach(GameObject item in ItemObjects)
            {
                if(item.GetComponent<Gun>())
                {
                    ItemObjects.Remove(item);
                }
            }
        }
    }

    private void HealthObjectsSetActive(bool istruefalse1, bool istruefalse2, bool istruefalse3, bool isdead)
    {
        HealthBarObject.SetActive(isdead);
        HealthBarObject3.SetActive(istruefalse1);
        HealthBarObject2.SetActive(istruefalse2);
        HealthBarObject1.SetActive(istruefalse3);
    }

    private void ArmorObjectsSetActive(bool istruefalse1, bool istruefalse2, bool istruefalse3, bool istruefalse4, bool istruefalse5, bool isdead)
    {
        DefanceBarObject1.SetActive(istruefalse1);
        DefanceBarObject2.SetActive(istruefalse2);
        DefanceBarObject3.SetActive(istruefalse3);
        DefanceBarObject4.SetActive(istruefalse4);
        DefanceBarObject5.SetActive(istruefalse5);
    }

    #endregion

    #region Public Method

    public void DisHealth(int power)
    {
        int remainingDamage = 0;
        if (Defence > 0)
        {
            if (power > Defence)
            {
                remainingDamage = power - Defence;
                Defence = 0;
            }
            else
            {
                Defence -= power;
            }
        }
        else if (Health > 0)
        {
            if (power > Health)
            {
                Health = 0;
            }
            else
            {
                Health -= power;
            }

            if (remainingDamage != 0)
            {
                Health -= remainingDamage;
            }
        }
    }

    public void CharacterHavingGunsUnload()
    {
        foreach (Gun gun in GameManager.Character.Guns)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == gun.Weapon.Id)
                {
                    Items.Remove(Items[i]);
                    ItemObjects.Remove(ItemObjects[i]);
                }
            }
        }
    }

    #endregion
}