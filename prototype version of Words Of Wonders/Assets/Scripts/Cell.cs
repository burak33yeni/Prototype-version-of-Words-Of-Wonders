using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI character;
    public int rowIndex { get; private set; }
    public int columnIndex { get; private set; }
    public bool opened { get; private set; }

    public void SetMe(int rowIndex, int columnIndex, char character)
    {
        this.rowIndex = rowIndex;
        this.columnIndex = columnIndex;
        this.character.text = character.ToString();
        this.character.color = Color.clear;
        background.color = Constants.instance.cellClosedColor;
        opened = false;
        transform.localScale = Vector3.one;
    }

    public void OpenCellByPlayer()
    {
        opened = true;
        character.color = Constants.instance.cellTextOpenedColor;
        background.color = Constants.instance.cellOpenedColor;
    }
    public void OpenCellByHint()
    {
        opened = true;
        character.color = Constants.instance.cellTextClosedColor;
    }
    public string GetCharacter()
    {
        return character.text;
    }
}
