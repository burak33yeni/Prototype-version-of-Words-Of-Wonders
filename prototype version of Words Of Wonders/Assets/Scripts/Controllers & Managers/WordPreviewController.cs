using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class WordPreviewController : MonoBehaviour
{
    [SerializeField] Transform cellContainer;
    [SerializeField] BoardController boardController;
    [SerializeField] Transform letterPreviewConainerTr;

    List<LetterPreview> letterPreviews;
    ObjectPooler objectPooler;
    Container container;
    Constants constants;


    // Start is called before the first frame update
    void Awake()
    {
        letterPreviews = new List<LetterPreview>();
        objectPooler = ObjectPooler.instance;
        container = Container.instance;
        constants = Constants.instance;
    }

    public void ResetPreview()
    {
        foreach (var item in letterPreviews)
        {
            objectPooler.RemovePooledObject(container.letterPreview, item.gameObject);
        }
        letterPreviews.Clear();
    }

    int i = 0;
    public void AdjustPreview(string newPreview)
    {
        ResetPreview();
        i = 0;
        foreach (char item in newPreview)
        {
            LetterPreview lp = objectPooler.GetPooledObject(container.letterPreview, Vector3.zero, Quaternion.identity, letterPreviewConainerTr)
                .GetComponent<LetterPreview>();
            lp.SetMe(item.ToString());
            letterPreviews.Add(lp);

            lp.transform.localPosition += new Vector3(75 * i,0,0);
            i++;
        }
        letterPreviewConainerTr.localPosition = Vector3.zero - new Vector3((newPreview.Length-1) / 2f * 75f, 0, 0);
    }

    float delay;
    // returns max delay
    public float SendLetterPreviewsToBoardCellsAndOpenWord(List<Cell> cells, string word)
    {
        for (int i = 0; i < letterPreviews.Count; i++)
        {
            delay = constants.letterPreviewMinDelay + (constants.letterPreviewDelay * i);

            letterPreviews[i].MakeBackgroundDisabled(delay/2f);

            LeanTween.move(letterPreviews[i].gameObject, cells[i].gameObject.transform.position, delay).setEaseInExpo();
            LeanTween.scale(letterPreviews[i].gameObject, cellContainer.localScale, delay).setEaseInExpo();

            letterPreviews[i].RemoveMeToPool(delay);

            boardController.OpenNewWord(word, cells[i], i == letterPreviews.Count - 1, delay);
        }
        letterPreviews = new List<LetterPreview>();

        return delay;
    }

}
