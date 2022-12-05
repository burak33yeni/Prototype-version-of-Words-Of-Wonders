using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static JSONReader;

public class LevelCreator : MonoBehaviour
{
    #region SINGLETON
    public static LevelCreator instance { get; private set; }

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
        inputManager = InputManager.instance;
    }
    #endregion

    [SerializeField] BoardController boardController;
    [SerializeField] WheelController wheelController;
    [SerializeField] WordPreviewController wordPreviewController;
    [SerializeField] GameController gameController;
    [SerializeField] RectTransform nextLevelImageRT;

    InputManager inputManager;

    List<Level> levels;
    int currLevelIndex;

    float nextLevelImageInitHeight;

    float nextLevelImageTime = 0.5f;
    public void SetLevels(List<Level> levels)
    {
        this.levels = levels;
        currLevelIndex = 0;

        nextLevelImageInitHeight = nextLevelImageRT.anchoredPosition.y;
    }

    public void CreateNextLevel(bool createFirst)
    {
        if (createFirst)
        {
            MakeLevel();
        }
        else {
            if (currLevelIndex == levels.Count)
            {
                gameController.TriggerEndGame();
                return;
            }

            inputManager.DisablePlayerToPlay();

            // image
            LeanTween.value(nextLevelImageRT.gameObject, nextLevelImageInitHeight, -nextLevelImageInitHeight, nextLevelImageTime)
                .setEaseOutCubic().setOnUpdate((float value) => {
                nextLevelImageRT.anchoredPosition = new Vector2(0, value);
            });

            // operations
            FunctionTimer.Create(() => {
                MakeLevel();
            }, nextLevelImageTime, false);
        } 
    }

    public void RemoveImage()
    {
        // image
        LeanTween.value(nextLevelImageRT.gameObject, -nextLevelImageInitHeight, nextLevelImageInitHeight, nextLevelImageTime).setOnUpdate((float value) => {
            nextLevelImageRT.anchoredPosition = new Vector2(0, value);
        });

        // enable player
        FunctionTimer.Create(() => { inputManager.EnablePlayerToPlay(); }, nextLevelImageTime, false);
    }

    void MakeLevel()
    {
        

        boardController.CreateNewBoard(levels[currLevelIndex]);
        wheelController.CreateNewWheel(levels[currLevelIndex]);
        wordPreviewController.ResetPreview();

        currLevelIndex++;

        
    }
}
