using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PooledObjectSO", menuName = "PooledObjectSOs/Create Pooled Object SO")]
public class PooledObjectSO : ScriptableObject
{
    public GameObject pooledObjectPrefab;
}
