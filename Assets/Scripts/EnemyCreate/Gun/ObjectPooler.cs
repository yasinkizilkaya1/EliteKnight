using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjectList;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjectList = new List<GameObject>();

        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obje = Instantiate(item.objectToPool,transform);
                obje.SetActive(false);
                pooledObjectList.Add(obje);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjectList.Count; i++)
        {
            if (!pooledObjectList[i].activeInHierarchy && pooledObjectList[i].tag == tag)
            {
                return pooledObjectList[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag && item.shouldExpand)
            {
                GameObject obje = Instantiate(item.objectToPool,transform);
                obje.SetActive(false);
                pooledObjectList.Add(obje);
                return obje;
            }
        }

        return null;
    }
}