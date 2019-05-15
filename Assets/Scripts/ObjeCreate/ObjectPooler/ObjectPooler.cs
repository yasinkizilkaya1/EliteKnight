using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Constants 

    public static ObjectPooler SharedInstance;

    #endregion

    #region Fields

    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obje = Instantiate(item.objectToPool, transform);
                obje.SetActive(false);
                pooledObjects.Add(obje);
            }
        }
    }

    #endregion

    #region Public Methods

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag && item.shouldExpand)
            {
                GameObject obje = Instantiate(item.objectToPool, transform);
                obje.SetActive(false);
                pooledObjects.Add(obje);
                return obje;
            }
        }

        return null;
    }

    #endregion
}