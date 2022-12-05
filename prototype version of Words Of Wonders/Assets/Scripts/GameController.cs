using System.Collections.Generic;
using UnityEngine;
using static JSONReader;

public class GameController : MonoBehaviour
{

    [SerializeField] TextAsset gameData;
    [SerializeField] RectTransform nextLevelImageRT;
    [SerializeField] RectTransform canvasRT;
    [SerializeField] GameObject endGameImage;

    LevelCreator levelCreator;

    // Start is called before the first frame update
    void Start()
    {
        // cache instances
        CacheNow();

        // adjust next level image size
        nextLevelImageRT.sizeDelta = new Vector2(0, canvasRT.sizeDelta.y);
        nextLevelImageRT.anchoredPosition = new Vector2(0, -nextLevelImageRT.sizeDelta.y/2);

        // Get Levels From Json File
        List<Level> levels = JSONReader.GetLevelsFromJson(gameData);

        // Send levels to level creator
        LevelCreator.instance.SetLevels(levels);

        // create first level
        levelCreator.CreateNextLevel(true);
    }

    void CacheNow()
    {
        levelCreator = LevelCreator.instance;
    }

    public void TriggerNewLevel()
    {
        levelCreator.CreateNextLevel(false);
    }
    public void TriggerEndGame()
    {
        endGameImage.SetActive(true);
    }
}
