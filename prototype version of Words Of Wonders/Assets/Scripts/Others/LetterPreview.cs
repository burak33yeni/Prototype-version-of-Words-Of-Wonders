using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterPreview : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI letter;
    [SerializeField] CanvasGroup canvasGroup;

    ObjectPooler objectPooler;
    Container container;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.instance;
        container = Container.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMe(string letter)
    {

        this.letter.text = letter;
        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1;
    }
    public void MakeBackgroundDisabled(float time)
    {
        LeanTween.value(canvasGroup.gameObject, 1, 0, time).setOnUpdate((float value) => {
            canvasGroup.alpha = value;
        });
    }
    public void RemoveMeToPool(float delay)
    {
        FunctionTimer.Create(() => {
            objectPooler.RemovePooledObject(container.letterPreview, gameObject);
        }, delay, false);
        
    }
}
