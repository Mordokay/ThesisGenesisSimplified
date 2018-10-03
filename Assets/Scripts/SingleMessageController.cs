using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMessageController : MonoBehaviour {

    public int messageId;
    public int messageType;
    UIManager uiManager;

    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
    }

    public void RemoveThisMessage()
    {
        if(messageType == 0)
        {
            uiManager.removeMessageWithId(messageId);
        }
        else
        {
            uiManager.removePatrolPointMessageWithId(messageId);
        }
        Destroy(this.gameObject);
    }
}