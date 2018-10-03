using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeManager : MonoBehaviour {

    public int messageID;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Application.runInBackground = true;
    }

    void Start () {
        //messageID = 0;		
	}
	
	public int getMessageId()
    {
        return messageID++;
    }
}
