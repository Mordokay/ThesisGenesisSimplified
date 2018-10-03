using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonGuardControler : MonoBehaviour {

    float messageTimeRemaining;
    bool showingMessage;

	void Start () {
        messageTimeRemaining = 0.0f;
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
	
    public void ShowMessage()
    {
        showingMessage = true;
        messageTimeRemaining = 10.0f;
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

	void Update () {
        if (showingMessage)
        {
            messageTimeRemaining -= Time.deltaTime;
            if(messageTimeRemaining < 0.0f)
            {
                showingMessage = false;
                this.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
	}
}
