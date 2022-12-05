using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InputManager : MonoBehaviour
{
    #region SINGLETON
    public static InputManager instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion
    [SerializeField] WordPreviewController previewController;
    [SerializeField] BoardController boardController;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] RectTransform wheelRectTransform;
    [SerializeField] MyLineRenderer myLineRenderer;

    int requestOfBlockPlayers = 0;
    List<Letter> letters;
    string currWord;
    Vector2 pointerPos;

    public void EnablePlayerToPlay() => requestOfBlockPlayers--;
    public void DisablePlayerToPlay() => requestOfBlockPlayers++;

    public bool CanPlayerPlay() => requestOfBlockPlayers <= 0f;

    // Start is called before the first frame update
    void Start()
    {
        letters = new List<Letter>();
        pointerPos = new Vector2(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        // if mouse up
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var item in letters)
            {
                item.DeSelectLetter();
            }
            letters = new List<Letter>();
            // update preview
            previewController.AdjustPreview(GetStringFromLettersSoFar());
            // line renderer
            myLineRenderer.AdjustLineRenderer(ref letters);
        }

        // pointer pos when letter contains items
        if(letters != null && letters.Count > 0)
        {
            pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointerPos /= canvasRectTransform.lossyScale;
            pointerPos -= new Vector2(wheelRectTransform.localPosition.x, wheelRectTransform.localPosition.y);

            myLineRenderer.AdjustLineRenderer(ref letters, true, pointerPos);
        }
    }
    public void TriggerFirstLetter(ref Letter letter)
    {
        if (!CanPlayerPlay())
            return;

        // add letter
        letters.Add(letter);
        currWord = GetStringFromLettersSoFar();

        // update preview
        previewController.AdjustPreview(currWord);
        // line renderer
        myLineRenderer.AdjustLineRenderer(ref letters);
        // adjust letter
        letter.SelectLetter();
    }
    public void TriggerPointerEnterLetter(ref Letter letter)
    {
        if (letters.Count == 0)
            return;

        if (!CanPlayerPlay())
            return;

        // remove last letter
        if (letters.Count >= 2 && letter == letters[letters.Count - 2])
        {
            // adjust letter
            letters[letters.Count - 1].DeSelectLetter();
            // remove letter
            letters.RemoveAt(letters.Count - 1);
            // update preview
            previewController.AdjustPreview(GetStringFromLettersSoFar());
            // line renderer
            myLineRenderer.AdjustLineRenderer(ref letters);

            return;
        }
        // last latter and curr letter must NOT be same
        else if (letters.Count >= 1)
        {
            foreach (var item in letters)
            {
                if(item == letter)
                {
                    return;
                }
            }
        }

        // add letter
        letters.Add(letter);
        currWord = GetStringFromLettersSoFar();

        // adjust letter
        letter.SelectLetter();

        // check new word       
        if (boardController.CheckWordCanBeOpened(currWord))
        {
            DisablePlayerToPlay();

            // deselect letters
            for (int i = 0; i < letters.Count; i++)
            {
                letters[i].DeSelectLetter();
            }

            // update preview
            previewController.AdjustPreview(currWord);

            // timer and animation
            float delay = previewController.SendLetterPreviewsToBoardCellsAndOpenWord(boardController.GetCells(currWord), currWord);
            FunctionTimer.Create(() => { EnablePlayerToPlay();  }, delay, false);
            
            letters = new List<Letter>();
            currWord = string.Empty;
        }
        else
        {
            // update preview
            previewController.AdjustPreview(currWord);
        }

        // line renderer
        myLineRenderer.AdjustLineRenderer(ref letters);
    }

    string GetStringFromLettersSoFar()
    {
        string soFar = "";
        foreach (var item in letters)
        {
            soFar += item.GetCharacter();
        }
        return soFar;
    }
    public void OpenRandomCell()
    {
        boardController.OpenRandomCell();
    }
}
