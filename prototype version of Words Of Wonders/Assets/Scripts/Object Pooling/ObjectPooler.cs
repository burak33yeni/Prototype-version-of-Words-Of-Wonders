using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{

    [Serializable]
    struct PooledObjectInfo
    {
        public PooledObjectSO pooledObjectSO;
        public int Amount;
    }
    [SerializeField]
    List<PooledObjectInfo>
        PooledObjects
    ;
    readonly Dictionary<PooledObjectSO, Queue<GameObject>> pooledObjectQueueDictionary = new Dictionary<PooledObjectSO, Queue<GameObject>>();
    GameObject pool;

    public static ObjectPooler instance;
    private void Awake()
    {
        #region Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        #endregion

        Initialize();
    }
    void Initialize() // call this on start to instantiate all pools
    {
        pool = new GameObject("Pool");
        MakePoolInInitialization(PooledObjects);  
    }

    void MakePoolInInitialization(List<PooledObjectInfo> pooledObjectInfos)
    {
        foreach (PooledObjectInfo pooledObjectInfo in pooledObjectInfos)
        {
            Queue<GameObject> pooledQueue = new Queue<GameObject>();

            // create instances
            for (int i = 0; i < pooledObjectInfo.Amount; i++)
            {
                GameObject pooledObject = Instantiate(pooledObjectInfo.pooledObjectSO.pooledObjectPrefab, pool.transform);
                pooledObject.SetActive(false);
                pooledQueue.Enqueue(pooledObject);
            }

            pooledObjectQueueDictionary.Add(pooledObjectInfo.pooledObjectSO, pooledQueue);
        }
    }

    public GameObject GetPooledObject(PooledObjectSO pooledObjectSO)
    {
        GameObject go = DequeueOrInstantiatePooledObject(pooledObjectSO);
        go.SetActive(true);
        return go;
    }
    public GameObject GetPooledObject(PooledObjectSO pooledObjectSO, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject go = DequeueOrInstantiatePooledObject(pooledObjectSO);
        go.transform.SetParent(parent);
        go.transform.rotation = rotation;
        go.transform.localPosition = position;
        go.SetActive(true);
        return go;
    }
    private GameObject DequeueOrInstantiatePooledObject(PooledObjectSO pooledObjectSO)
    {
        GameObject go;
        if (pooledObjectQueueDictionary[pooledObjectSO].Count > 0) // pool has objects
        {
            go = pooledObjectQueueDictionary[pooledObjectSO].Dequeue();
            if (go == null)
                Debug.LogError("ERROR when dequeue. Count: " + pooledObjectQueueDictionary[pooledObjectSO].Count);
        }
        else // pool is empty - instantiate new objects
        {
            Debug.Log(pooledObjectSO + ": newly Instantiated -> Pool Is Empty");
            go = Instantiate(pooledObjectSO.pooledObjectPrefab, pool.transform);
            if (go == null)
                Debug.LogError("ERROR when instantiation pool. Count: " + pooledObjectQueueDictionary[pooledObjectSO].Count);
        }
        return go;
    }
    public void RemovePooledObject(PooledObjectSO pooledObjectSO, GameObject pooledObject)
    {
        pooledObject.SetActive(false);
        pooledObject.transform.SetParent(pool.transform);
        pooledObjectQueueDictionary[pooledObjectSO].Enqueue(pooledObject);
    }
    public void RemovePooledObject(PooledObjectSO pooledObjectSO, GameObject pooledObject, Vector3 positionWhilePassive)
    {
        RemovePooledObject(pooledObjectSO, pooledObject);
        pooledObject.transform.position = positionWhilePassive;
    }
    public bool IsKeyValid(PooledObjectSO pooledObjectSO)
    {
        return pooledObjectQueueDictionary.ContainsKey(pooledObjectSO);
    }
}
