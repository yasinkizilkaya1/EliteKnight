using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region Constants

    private const string TAG_BODY = "Player";

    #endregion

    #region Fields

    public Spawn spawn;
    public List<GameObject> DoorObjects;

    public GameObject LockObject;

    public int EnemyCount;
    public bool isLock;
    public bool isInside;

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (spawn.CharacterList[0].DeadEnemyCount == EnemyCount)
        {
            DoorOpen();
        }

        if (isLock && isInside == false)
        {
            DoorLock();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_BODY))
        {
            isLock = true;
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_BODY))
        {
            isInside = false;
        }
    }

    #endregion

    #region Private Methods

    private void DoorOpen()
    {
        for (int i = 0; i < DoorObjects.Count; i++)
        {
            DoorObjects[i].SetActive(false);
        }
        spawn.CharacterList[0].DeadEnemyCount = 0;
        Destroy(LockObject);
    }

    private void DoorLock()
    {
        for (int i = 0; i < DoorObjects.Count; i++)
        {
            DoorObjects[i].SetActive(true);
        }
    }

    #endregion
}