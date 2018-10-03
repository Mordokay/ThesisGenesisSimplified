using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpeedController : MonoBehaviour {

    float minTimeSpeed;
    float maxTimeSpeed;

    public float currentTime = 1.0f;
    public Text timeSpeedText;
    EditorModeController em;

    UIManager uiManager;

    void Start()
    {
        em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditorModeController>();
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
    }

    public void decrementTimeSpeed()
    {
        if (!em.isEditorMode || uiManager.isWatchModeEnabled)
        {
            currentTime /= 2.0f;
            if (currentTime < 1)
            {
                currentTime = 1.0f;
            }
            timeSpeedText.text = currentTime.ToString() + "X";
            Time.timeScale = currentTime;
        }
    }

    public void incrementTimeSpeed()
    {
        if (!em.isEditorMode || uiManager.isWatchModeEnabled)
        {
            currentTime *= 2.0f;
            if (currentTime > 16.0f)
            {
                currentTime = 16.0f;
            }
            timeSpeedText.text = currentTime.ToString() + "X";
            Time.timeScale = currentTime;
        }
    }

}
