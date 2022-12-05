using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLineRenderer : MonoBehaviour
{
    [SerializeField] Transform wheelTransform;
    Container container;
    ObjectPooler objectPooler;
    List<LineRenderer> lines;

    public void AdjustLineRenderer(ref List<Letter> letters, bool pointerPosEnabled = false, Vector3 pointerPos = default)
    {
        if (!pointerPosEnabled)
        {
            if (letters.Count == 0)
            {
                RemoveAllLines();
                return;
            }

            // create lines
            CreateLine(letters.Count);

            // adjust positions
            for (int i = 0; i < letters.Count-1; i++)
            {
                SetPositionOfLines(i, letters[i].transform.localPosition, letters[i + 1].transform.localPosition);
            }

            //adjust last line letters position
            SetPositionOfLines(letters.Count - 1, letters[letters.Count - 1].transform.localPosition, 
                letters[letters.Count - 1].transform.localPosition);
        }
        else
        {
            SetPositionOfLines(letters.Count - 1, letters[letters.Count - 1].transform.localPosition, pointerPos);
        }

    }
    void SetPositionOfLines(int index, Vector3 pos1, Vector3 pos2)
    {
        lines[index].SetPosition(0, pos1);
        lines[index].SetPosition(1, pos2);
    }
    void RemoveAllLines()
    {
        foreach (var item in lines)
        {
            objectPooler.RemovePooledObject(container.line, item.gameObject);
        }
        lines.Clear();
    }
    void CreateLine(int amount)
    {
        RemoveAllLines();
        for (int i = 0; i < amount; i++)
        {
            LineRenderer lr = objectPooler.GetPooledObject(container.line, new Vector3(0,0,-5), Quaternion.identity, wheelTransform)
                .GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.transform.localScale = Vector3.one;
            lines.Add(lr);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lines = new List<LineRenderer>();
        objectPooler = ObjectPooler.instance;
        container = Container.instance;
    }

}
