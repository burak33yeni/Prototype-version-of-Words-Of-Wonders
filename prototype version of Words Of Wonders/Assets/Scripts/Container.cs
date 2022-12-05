using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    #region SINGLETON
    public static Container instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] PooledObjectSO _cell;
    public PooledObjectSO cell { get { return _cell; } }

    [SerializeField] PooledObjectSO _letter;
    public PooledObjectSO letter { get { return _letter; } }
    [SerializeField] PooledObjectSO _letterPreview;
    public PooledObjectSO letterPreview { get { return _letterPreview; } }
    [SerializeField] PooledObjectSO _line;
    public PooledObjectSO line { get { return _line; } }
}
