using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    #region SINGLETON
    public static Constants instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] int _cellDistance;
    public int cellDistance { get { return _cellDistance; } }
    [SerializeField] int _boardMaxCellCountForScale1;
    public int boardMaxCellCountForScale1 { get { return _boardMaxCellCountForScale1; } }
    [SerializeField] Vector2 _topLeftCellCoor;
    public Vector2 topLeftCellCoor { get { return _topLeftCellCoor; } }

    [SerializeField] Color _cellClosedColor;
    public Color cellClosedColor { get { return _cellClosedColor; } }

    [SerializeField] Color _cellOpenedColor;
    public Color cellOpenedColor { get { return _cellOpenedColor; } }

    [SerializeField] Color _cellTextClosedColor;
    public Color cellTextClosedColor { get { return _cellTextClosedColor; } }

    [SerializeField] Color _cellTextOpenedColor;
    public Color cellTextOpenedColor { get { return _cellTextOpenedColor; } }

    [SerializeField] int _wheelLetterDist;
    public int wheelLetterDist { get { return _wheelLetterDist; } }

    [SerializeField] Color _letterBackgNonSelectedColor;
    public Color letterBackgNonSelectedColor { get { return _letterBackgNonSelectedColor; } }
    [SerializeField] Color _letterBackgSelectedColor;
    public Color letterBackgSelectedColor { get { return _letterBackgSelectedColor; } }

    [SerializeField] Color _letterTextNonSelectedColor;
    public Color letterTextNonSelectedColor { get { return _letterTextNonSelectedColor; } }

    [SerializeField] Color _letterTextSelectedColor;
    public Color letterTextSelectedColor { get { return _letterTextSelectedColor; } }

    [SerializeField] float _letterPreviewDelay;
    public float letterPreviewDelay { get { return _letterPreviewDelay; } }

    [SerializeField] float _letterPreviewMinDelay;
    public float letterPreviewMinDelay { get { return _letterPreviewMinDelay; } }
}
