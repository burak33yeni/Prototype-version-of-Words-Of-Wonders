using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static JSONReader;

public class BoardController : MonoBehaviour
{
    [SerializeField] RectTransform cellParentObj;
    [SerializeField] GameController gameController;

    ObjectPooler objectPooler;
    Constants constants;
    Container container;

    Dictionary<string, List<Cell>> wordsOnBoard;

    // Start is called before the first frame update
    void Start()
    {
        Cache();
    }


    public void CreateNewBoard(Level level)
    {
        CleanBoard();

        // reset dictionary
        wordsOnBoard = new Dictionary<string, List<Cell>>();

        // make pivot center
        cellParentObj.pivot = new Vector2(0.5f, 0.5f);

        // add cells of words
        foreach (Word word in level.wordsList)
        {
            // add cells of each character
            for (int i = 0; i < word.characters.Length; i++)
            {
                if (word.isHorizontal)
                    AddCellToBoard(word.startRow, word.startColumn + i, word.characters[i], word.characters);
                else
                    AddCellToBoard(word.startRow + i, word.startColumn, word.characters[i], word.characters);
            }

        }

        // make pivot top left
        cellParentObj.pivot = new Vector2(0f, 1f);

        // adjust board scale
        cellParentObj.localScale = (constants.boardMaxCellCountForScale1 * 1f
            / Mathf.Max(int.Parse(level.row), int.Parse(level.column))) * Vector3.one;

    }

    // adds cell to board if same cell is not on board
    void AddCellToBoard(int rowIndex, int columnIndex, char character, string word)
    {
        Cell cell = GetCellInGivenIndex(rowIndex, columnIndex, character.ToString());

        if (cell == null)
        {
            // create cell
            cell = objectPooler.GetPooledObject(container.cell,
                constants.topLeftCellCoor + new Vector2(columnIndex * constants.cellDistance, -rowIndex * constants.cellDistance)
                , Quaternion.identity, cellParentObj).GetComponent<Cell>();

            // cell specs
            cell.SetMe(rowIndex, columnIndex, character);
        }

        // add cell to dictionary
        if (wordsOnBoard.ContainsKey(word))
            wordsOnBoard[word].Add(cell);
        else
        {
            wordsOnBoard.Add(word, new List<Cell> { cell });
        }

    }

    Cell GetCellInGivenIndex(int rowIndex, int columnIndex, string character)
    {
        foreach (var word in wordsOnBoard)
        {
            foreach (var cell in word.Value)
            {
                if (cell.rowIndex == rowIndex && cell.columnIndex == columnIndex && cell.GetCharacter() == character)
                    return cell;
            }
        }
        return null;
    }

    public bool CheckWordCanBeOpened(string newWord)
    {
        if (wordsOnBoard.ContainsKey(newWord))
        {
            foreach (var item in wordsOnBoard[newWord])
            {
                if (!item.opened)
                    return true;
            }
        }
        return false;
    }
    public void OpenNewWord(string newWord, Cell cell, bool controlForCompletion, float delay)
    {
        FunctionTimer.Create(() =>
        {
            if (wordsOnBoard.ContainsKey(newWord))
            {
                foreach (var item in wordsOnBoard[newWord])
                {
                    if (item == cell)
                        item.OpenCellByPlayer();
                }
                if (controlForCompletion)
                    ControlBoardForLevelCompletion();
            }
        }, delay, false);

    }
    void Cache()
    {
        objectPooler = ObjectPooler.instance;
        constants = Constants.instance;
        container = Container.instance;

    }

    List<Cell> cells;
    public void OpenRandomCell()
    {
        cells = new List<Cell>();
        foreach (var words in wordsOnBoard)
        {
            foreach (var cell in words.Value)
            {
                if (!cell.opened)
                {
                    cells.Add(cell);
                }
            }
        }
        if (cells.Count == 0)
            return;

        cells = cells.Distinct().ToList();
        cells[UnityEngine.Random.Range(0, cells.Count)].OpenCellByHint();

        ControlBoardForLevelCompletion();
    }
    void ControlBoardForLevelCompletion()
    {
        foreach (var words in wordsOnBoard)
        {
            foreach (var cell in words.Value)
            {
                if (!cell.opened)
                {
                    return;
                }
            }
        }
        gameController.TriggerNewLevel();
    }

    void CleanBoard()
    {
        if (wordsOnBoard == null)
            return;

        foreach (var item in wordsOnBoard)
        {
            foreach (var cell in item.Value)
            {
                if (cell.isActiveAndEnabled)
                    objectPooler.RemovePooledObject(container.cell, cell.gameObject);
            }
        }
    }
    public List<Cell> GetCells(string word)
    {
        if (wordsOnBoard.ContainsKey(word))
        {
            return wordsOnBoard[word];
        }
        return null;
    }
}