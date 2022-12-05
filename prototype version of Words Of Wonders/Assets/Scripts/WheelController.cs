using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JSONReader;

public class WheelController : MonoBehaviour
{

    [SerializeField] Transform lettersContainer;

    Dictionary<char, int> charsAndCounts;
    List<Letter> letters;
    float degreeChange;
    Vector2 initialLetterPos;

    ObjectPooler objectPooler;
    Container container;
    Constants constants;

    public void CreateNewWheel(Level level)
    {
        CleanWheel();

        // adjust dictionary
        AdjustLetterDic(level.wordsList);

        letters = new List<Letter>();

        // get degree change
        degreeChange = 360f / GetNumOfTotalLettersInDictionary();
        initialLetterPos = new Vector2(0, constants.wheelLetterDist);

        // create objects
        foreach (var item in charsAndCounts)
        {
            for (int i = 0; i < item.Value; i++)
            {
                Letter l = objectPooler.GetPooledObject(container.letter,
                    initialLetterPos, Quaternion.identity, lettersContainer)
                    .GetComponent<Letter>();
                l.SetMe(item.Key);
                letters.Add(l);

                RotateVector(ref initialLetterPos, degreeChange);
            }
        }
    }

    Dictionary<char, int> charsAndCountsTemp;
    void AdjustLetterDic(List<Word> words)
    {
        // reset chars
        charsAndCounts = new Dictionary<char, int>();

        // each word
        foreach (Word word in words)
        {
            // reset temp dictionary for each word
            charsAndCountsTemp = new Dictionary<char, int>();

            // each character
            foreach (char character in word.characters)
            {
                // adjust temp dictionary
                if (charsAndCountsTemp.ContainsKey(character))
                {
                    charsAndCountsTemp[character]++;
                }
                else
                {
                    charsAndCountsTemp.Add(character, 1);
                }

                // adjust real dictionary
                if (charsAndCounts.ContainsKey(character) &&
                    charsAndCountsTemp[character] > charsAndCounts[character]) {
                    charsAndCounts[character] = charsAndCountsTemp[character];
                }
                else if (!charsAndCounts.ContainsKey(character))
                {
                    charsAndCounts.Add(character, charsAndCountsTemp[character]);
                }
            }
        }
    }

    int GetNumOfTotalLettersInDictionary()
    {
        int totalLettersInDictionary = 0;
        foreach (var item in charsAndCounts)
        {
            totalLettersInDictionary += item.Value;
        }
        return totalLettersInDictionary;
    }

    void Awake()
    {
        CacheMe();
    }

    void CacheMe()
    {
        objectPooler = ObjectPooler.instance;
        container = Container.instance;
        constants = Constants.instance;
    }

    public static void RotateVector(ref Vector2 vectorToRotate, float degree)
    {
        float sin = Mathf.Sin(degree * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degree * Mathf.Deg2Rad);

        float tx = vectorToRotate.x;
        float ty = vectorToRotate.y;

        vectorToRotate.x = (cos * tx) - (sin * ty);
        vectorToRotate.y = (sin * tx) + (cos * ty);
    }

    void CleanWheel()
    {
        if(letters == null) return;

        foreach (var item in letters)
        {
            objectPooler.RemovePooledObject(container.letter, item.gameObject);
        }
    }
}
