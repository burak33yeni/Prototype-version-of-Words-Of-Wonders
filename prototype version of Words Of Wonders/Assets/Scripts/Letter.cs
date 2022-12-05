using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Letter : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] Image background;
    [SerializeField] TextMeshProUGUI character;

    Constants constants;
    public void SetMe(char character)
    {
        CacheMe();
        this.character.text = character.ToString();
        this.character.color = constants.letterTextNonSelectedColor;
        background.color = constants.letterBackgNonSelectedColor;
        transform.localScale = Vector3.one;
    }
    public string GetCharacter()
    {
        return this.character.text;
    }

    void CacheMe()
    {
        constants = Constants.instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Letter letter = this;
        InputManager.instance.TriggerPointerEnterLetter(ref letter);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Letter letter = this;
        InputManager.instance.TriggerFirstLetter(ref letter);
    }

    public void SelectLetter()
    {
        character.color = constants.letterTextSelectedColor;
        background.color = constants.letterBackgSelectedColor;
    }
    public void DeSelectLetter()
    {
        character.color = constants.letterTextNonSelectedColor;
        background.color = constants.letterBackgNonSelectedColor;
    }

    
}
