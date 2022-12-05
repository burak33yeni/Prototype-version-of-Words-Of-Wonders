using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FunctionTimer
{
    private static List<FunctionTimer> activeTimerList;
    private static GameObject initGameObject; // global game object used for initializind class, is destroyed on scene change

    private static void InitIfNeeded()
    {
        if (initGameObject == null)
        {
            initGameObject = new GameObject("FunctionTimer_InitGameObject");
            activeTimerList = new List<FunctionTimer>();
        }
    }

    public static FunctionTimer Create(Action action, float timer, bool isRepeatEnabled, string timerName = null)
    {
        InitIfNeeded();
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
        FunctionTimer functionTimer = new FunctionTimer(action, timer, timerName, gameObject, isRepeatEnabled);


        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;
        activeTimerList.Add(functionTimer);
        return functionTimer;
    }
    public static FunctionTimer CreateUnscaled(Action action, float timer, bool isRepeatEnabled, string timerName = null)
    {
        InitIfNeeded();
        GameObject gameObject = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
        FunctionTimer functionTimer = new FunctionTimer(action, timer, timerName, gameObject, isRepeatEnabled);


        gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.UpdateUnscaled;
        activeTimerList.Add(functionTimer);
        return functionTimer;
    }
    public static void StopTimer(string timerName)
    {
        InitIfNeeded();
        for (int i = 0; i < activeTimerList.Count; i++)
        {
            if (activeTimerList[i].timerName == timerName)
            {
                // stop timer
                activeTimerList[i].DestroySelf();
                i--;
            }
        }
    }
    public static void PauseOrResumeTimer(string timerName, bool isResumed)
    {
        InitIfNeeded();
        for (int i = 0; i < activeTimerList.Count; i++)
        {
            if (activeTimerList[i].timerName == timerName)
            {
                // pause timer
                activeTimerList[i].isPaused = !isResumed;
            }
        }
    }
    private static void RemoveTimer(FunctionTimer functionTimer)
    {
        InitIfNeeded();
        activeTimerList.Remove(functionTimer);
    }
    private class MonoBehaviourHook : MonoBehaviour
    {
        public Action onUpdate;
        private void Update()
        {
            if (onUpdate != null)
                onUpdate();
        }
    }

    private Action action;
    private float timer, initialTimer;
    string timerName;
    private GameObject gameObject;
    private bool isDestroyed, isRepeatEnabled, isPaused;

    private FunctionTimer(Action action, float timer, string timerName, GameObject gameObject, bool isRepeatEnabled)
    {
        this.action = action;
        this.timer = timer;
        initialTimer = timer;
        this.gameObject = gameObject;
        this.timerName = timerName;
        isDestroyed = false;
        this.isRepeatEnabled = isRepeatEnabled;
        isPaused = false;
    }
    public void Update()
    {
        if (!isDestroyed)
        {
            if (!isPaused)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    action();
                    if (isRepeatEnabled)
                    {
                        timer = initialTimer;
                    }
                    else
                    {
                        DestroySelf();
                    }
                }
            }
        }

    }
    public void UpdateUnscaled()
    {
        if (!isDestroyed)
        {
            timer -= Time.unscaledDeltaTime;
            if (timer < 0)
            {
                action();
                if (isRepeatEnabled)
                {
                    timer = initialTimer;
                }
                else
                {
                    DestroySelf();
                }
            }
        }

    }
    private void DestroySelf()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
        RemoveTimer(this);
    }

    public float GetPercentageOfRemainingTime()
    {
        return timer / initialTimer;
    }
    public bool GetIsDestroyed()
    {
        return isDestroyed;
    }
}
