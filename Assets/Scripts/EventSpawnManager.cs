using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSpawnManager : MonoBehaviour {

    PlayModeManager pm;
    EditorModeController emc;
    UIManager uiManager;

    public int eventId;

    public List<Message.Tag> tags;
    public float messageSendDistance;
    public float messageTime;
    public string description;
    public bool initialized;

    private void Start()
    {
        emc = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditorModeController>();
        pm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayModeManager>();
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        eventId = pm.getMessageId();
    }

    public void InitializeSpawnEvent(List<Message.Tag> myTags, float distance, float time, string desc)
    {
        this.tags = myTags;
        this.messageSendDistance = distance;
        this.messageTime = time;
        this.description = desc;

        string tagString = "";
        foreach (Message.Tag t in myTags)
        {
            tagString += t.name + "-" + t.weight + ",";
        }
        if (tags.Count > 0)
        {
            tagString = tagString.Substring(0, tagString.Length - 1);
        }

        this.transform.GetChild(1).GetComponentInChildren<Text>().text = desc + System.Environment.NewLine;
        this.transform.GetChild(1).GetComponentInChildren<Text>().text += "Transmission time: " + time + System.Environment.NewLine;
        this.transform.GetChild(1).GetComponentInChildren<Text>().text += tagString;
    }

    private void Update()
    {
        bool found_NPC_or_Patrol_Point_Close = false;

        if (!description.Equals("") && Time.timeScale != 0.0f)
        {
            foreach (Transform npc in emc.npcHolder.transform)
            {
                //Debug.Log(npc.name + " messageSendDistance: " + messageSendDistance + " Distance: " + Vector3.Distance(npc.GetChild(1).position, this.transform.position) + " description: " + description);
                //check if NPC is at a close distance;
                if (Vector3.Distance(npc.GetChild(1).position, this.transform.position) < messageSendDistance)
                {
                    found_NPC_or_Patrol_Point_Close = true;
                    string tagString = "";
                    foreach (Message.Tag t in tags)
                    {
                        tagString += t.name + " " + t.weight + ",";
                    }
                    if (tags.Count > 0)
                    {
                        tagString = tagString.Substring(0, tagString.Length - 1);
                    }
                    //Debug.Log("tagString: " + tagString);
                    npc.gameObject.GetComponent<NPCData>().ReceiveMessage(new Message(eventId, messageTime, description, tagString));

                    uiManager.messageTrackingID.text = eventId.ToString();
                    npc.gameObject.GetComponent<NPCFeedbackUpdater>().checkMessageFeedback();
                }
                //npc.gameObject.GetComponentInChildren<NPCPatrolMovement>().UpdatePatrolPoints();
            }
            foreach (Transform patrolPoint in emc.patrolPointsHolder.transform)
            {
                //check if Patrol Points are at a close distance;
                if (Vector3.Distance(patrolPoint.position, this.transform.position) < messageSendDistance)
                {
                    found_NPC_or_Patrol_Point_Close = true;
                    string tagString = "";
                    foreach (Message.Tag t in tags)
                    {
                        tagString += t.name + " " + t.weight + ",";
                    }
                    if (tags.Count > 0)
                    {
                        tagString = tagString.Substring(0, tagString.Length - 1);
                    }
                    patrolPoint.gameObject.GetComponent<PatrolPointData>().ReceiveEvent(new Message(eventId, messageTime, description, tagString));
                }
            }
        }
        if (found_NPC_or_Patrol_Point_Close)
        {
            Destroy(this.gameObject);
        }
    }
}
