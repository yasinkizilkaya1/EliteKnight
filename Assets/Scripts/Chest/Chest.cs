using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region Constants

    private const string TAG_KNIFE = "knife";
    private const string TAG_GAMEMANAGER = "GameManager";

    #endregion

    #region Fields

    public GameManager GameManager;
    public ChestEntity ChestEntity;

    public List<GameObject> ItemObjects;
    public List<GameObject> HealthBars;
    public List<GameObject> DefenceBars;

    public int CurrentHealth;
    public int Defence;

    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
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
                CurrentHealth--;
            }
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        GameManager = GameObject.FindWithTag(TAG_GAMEMANAGER).GetComponent<GameManager>();
        name = ChestEntity.Name;
        CurrentHealth = ChestEntity.Health;
        Defence = ChestEntity.Defence;
        GameManager.Chests.Add(this);
        CharacterHavingGunsUnload();

        if (GameManager.Character.knife != null && GameManager.Character.Gun == null)
        {
            int Count = ItemObjects.Count;

            for (int i = 0; i < Count; i++)
            {
                if (ItemObjects[i].GetComponent<Gun>())
                {
                    Count--;
                    ItemObjects.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private void ObjectsSetActive(int MaxValue, int value, List<GameObject> gameObjects)
    {
        int Count = MaxValue - value;

        if (Count != MaxValue)
        {
            for (int i = MaxValue - 1; i >= Count; i--)
            {
                gameObjects[i].SetActive(false);
            }
        }
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
                ObjectsSetActive(Defence, power, DefenceBars);
                Defence -= power;
            }
        }
        else if (CurrentHealth > 0)
        {
            if (power > CurrentHealth)
            {
                CurrentHealth = 0;
            }
            else
            {
                ObjectsSetActive(CurrentHealth, power, HealthBars);
                CurrentHealth -= power;

                if (CurrentHealth == 0 && ChestEntity.ItemDrop)
                {
                    Instantiate(ItemObjects[Random.Range(0, ItemObjects.Count - 1)], transform.position, transform.rotation);
                    GameManager.Chests.Remove(this);
                    Destroy(gameObject);
                }
            }

            if (remainingDamage != 0)
            {
                CurrentHealth -= remainingDamage;
            }
        }
    }

    public void CharacterHavingGunsUnload()
    {
        foreach (Gun gun in GameManager.Character.Guns)
        {
            for (int i = 0; i < ItemObjects.Count; i++)
            {
                if (ItemObjects[i].GetComponent<Gun>() && ItemObjects[i].GetComponent<Gun>().Weapon.Id == gun.Weapon.Id)
                {
                    ItemObjects.Remove(ItemObjects[i]);
                }
            }
        }
    }

    #endregion
}