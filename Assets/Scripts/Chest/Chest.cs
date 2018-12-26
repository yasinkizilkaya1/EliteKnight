using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region Constants

    private const string TAG_KNIFE = "knife";
    private const string TAG_SUPPORT = "Support";
    private const string TAG_SPEALİST = "Spealist";
    private const string TAG_BULLET = "bullet";

    #endregion

    #region Fields

    private WarriorData warriorData;
    public ChestEntity chestEntity;
    public List<GameObject> ItemList;
    public GameManager gameManager;

    public GameObject HealthBarObject;
    public GameObject HealthBarObject1;
    public GameObject HealthBarObject2;
    public GameObject HealthBarObject3;
    public GameObject DefanceBarObject1;
    public GameObject DefanceBarObject2;
    public GameObject DefanceBarObject3;
    public GameObject DefanceBarObject4;
    public GameObject DefanceBarObject5;
    public GameObject ItemObject;

    public int ItemId;
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

                if (chestEntity.ItemDrop)
                {
                    ItemObject = Instantiate(ItemList[RandomItemId(ItemList.Count)], ItemObject.transform);
                }
                HealthBarObject.SetActive(false);
                Destroy(gameObject);
                break;
        }

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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(TAG_BULLET))
        {
            Destroy(col.gameObject);

            int remainingDamage = 0;
            if (Defence > 0)
            {
                if (warriorData.Power > Defence)
                {
                    remainingDamage = warriorData.Power - Defence;
                    Defence = 0;
                }
                else
                {
                    Defence -= warriorData.Power;
                }
            }
            else if (Health > 0)
            {
                if (warriorData.Power > Health)
                {
                    Health = 0;
                }
                else
                {
                    Health -= warriorData.Power;
                }

                if (remainingDamage != 0)
                {
                    Health -= remainingDamage;
                }
            }
        }
        else if (col.gameObject.CompareTag(TAG_KNIFE))
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
        warriorData = (WarriorData)AssetDatabase.LoadAssetAtPath("Assets/Data/CharacterData/" + gameManager.SelectedCardNameString + ".asset", typeof(WarriorData));
        name = chestEntity.Name;
        Defence = chestEntity.Defence;
        Health = chestEntity.Health;

        if (gameManager.SelectedCardNameString == TAG_SUPPORT)
        {
            ItemList.Remove(ItemList[3]);
            ItemList.Remove(ItemList[2]);
        }
        else if (gameManager.SelectedCardNameString == TAG_SPEALİST)
        {
            ItemList.Remove(ItemList[3]);
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

    private int RandomItemId(int end)
    {
        ItemId = Random.Range(0, end);
        return ItemId;
    }

    #endregion
}