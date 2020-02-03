using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockMinuteHand : MonoBehaviour
{
    public float deltaTimePerMinute = 1.0f;
    public int minutesTotal = 15;

    private float time;

    public void ResetTime()
    {
        time = (float) minutesTotal;
    }

    void Start()
    {
        ResetTime();
    }

    void Update()
    {
        if (Game.Instance.GetState() == GameState.Playing)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 6.0f * time));
            float timeStep = deltaTimePerMinute * Time.deltaTime;
            if (time - timeStep < 0.0f)
            {
                Game.Instance.SetState(GameState.RetryMenu);
            }
            else
            {
                time -= timeStep;
            }
        }
    }
}
