using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointData : MonoBehaviour {

    public List<Message> messages;

    public void ReceiveEvent(Message m)
    {
        if(messages.Count == 3)
        {
            messages.RemoveAt(0);
        }
        messages.Add(m);
    }

    public void InitializePatrolPointData(string messagesText)
    {
        if (messagesText != "")
        {
            string[] messagesList = messagesText.Split(';');
            foreach (string m in messagesList)
            {
                //Debug.Log("message: " + m);
                string[] messageInfo = m.Split('&');

                string[] messageBasicData = messageInfo[0].Split(' ');
                int id = System.Int32.Parse(messageBasicData[0]);
                float timeOfLife = float.Parse(messageBasicData[1]);
                string description = messageInfo[1];
                string tagsText = messageInfo[2];

                messages.Add(new Message(id, timeOfLife, description, tagsText));
            }
        }
    }
}