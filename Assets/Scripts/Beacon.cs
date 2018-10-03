using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beacon : MonoBehaviour {

    public List<string> eventTags;
    public float timeBetweenEventsMin;
    public float timeBetweenEventsMax;
    public int tagValueMin;
    public int tagValueMax;
    public GameObject npcHolder;
    public float messageTime;
    public float timeToNextEvent;

    public bool isOnSequence;

    UIManager uiManager;
    EditorModeController emc;

    public Message[] messageSequence;
    public float[] messageTimeOfSpawn;

    int currentMessageOnSequence;

    bool sequenceInitialized = false;

    public Text messageSequenceText;

    float timeToNextElementalBeaconEvent;

    public float timebetweenElementalEventsMin;
    public float timebetweenElementalEventsMax;

    string[] tags = { "Rock", "Wood", "Berries", "Cactus"};

    void Start () {
        emc = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditorModeController>();
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();

        timeToNextEvent = 60.0f;
        currentMessageOnSequence = 0;

        timebetweenElementalEventsMin = 5.0f;
        timebetweenElementalEventsMax = 10.0f;

        if (!sequenceInitialized)
        {
            sequenceInitialized = true;
            messageSequence = new Message[15];
            messageTimeOfSpawn = new float[15];
        }
        //messageSequence = new Message[10];
        //messageTimeOfSpawn = new float[10];
        //GenerateMessageSequence();
        isOnSequence = false;


        //InvokeRepeating("SpawnElementalEvent", 10.0f, 20.0f);
        timeToNextElementalBeaconEvent = UnityEngine.Random.Range(timebetweenElementalEventsMin, timebetweenElementalEventsMax);
        //Debug.Log("timeToNextElementalBeaconEvent: " + timeToNextElementalBeaconEvent);

        InitializeNPCMessages();
    }

    void InitializeNPCMessages()
    {
        foreach (Transform npc in npcHolder.transform)
        {
            npc.GetComponent<NPCData>().messages.Clear();

            //There is a chance of 50% than an NPC has an initial message
            if (UnityEngine.Random.Range(0, 100) < 50)
            {
                Message msg = msg = CreateMessage(-99, false, 1);
                npc.gameObject.GetComponent<NPCData>().ReceiveMessage(msg);
            }
        }
    }

    public void SpreadInitialGoldenMessages()
    {
        //Goes through all the golden objects
        foreach (EditorModeController.Element element in emc.GoldenElementList)
        {
            //There is a chance of 50%  that a golden object spreads a message
            if (UnityEngine.Random.Range(0, 100) < 50)
            {
                //Debug.Log(element.elementObject.name);
                element.elementObject.GetComponent<ElementController>().BeaconPulse(true);
            }
        }
    }

    void SpawnMode3()
    {
        if (UnityEngine.Random.Range(0, 100) < 60)
        {
            EditorModeController.Element element =
                this.GetComponent<EditorModeController>().GoldenElementList[UnityEngine.Random.Range(0, this.GetComponent<EditorModeController>().GoldenElementList.Count)];
            element.elementObject.GetComponent<ElementController>().BeaconPulse(true);
        }
        else
        {
            EditorModeController.Element element =
                this.GetComponent<EditorModeController>().NormalElementList[UnityEngine.Random.Range(0, this.GetComponent<EditorModeController>().NormalElementList.Count)];
            element.elementObject.GetComponent<ElementController>().BeaconPulse(false);
        }
    }

    void SpawnElementalEvent()
    {
        // 10% of times, every 5 to 15 seconds, a pulse will happen on a normal object (NOT golden)
        //On Mode=3 messages from gold and normal objects are considered
        if (UnityEngine.Random.Range(0, 100) < 40 && this.GetComponent<MySQLManager>().playerMode == "3")
        {
            EditorModeController.Element element =
                this.GetComponent<EditorModeController>().NormalElementList[UnityEngine.Random.Range(0, this.GetComponent<EditorModeController>().NormalElementList.Count)];
            element.elementObject.GetComponent<ElementController>().BeaconPulse(false);
        }
        //Messages on Mode=2 are always gold messages
        else
        {
            EditorModeController.Element element =
                this.GetComponent<EditorModeController>().GoldenElementList[UnityEngine.Random.Range(0, this.GetComponent<EditorModeController>().GoldenElementList.Count)];
            element.elementObject.GetComponent<ElementController>().BeaconPulse(true);
        }
    }

    public void RefreshMessageSequenceText()
    {
        string outputText = "";
        for(int i = 0; i < messageSequence.Length; i++)
        {
            if (messageTimeOfSpawn[i] - Time.timeSinceLevelLoad > 0.0f)
            {
                outputText += "TimeOfSpawn: " + (messageTimeOfSpawn[i] - Time.timeSinceLevelLoad) + " ID: " + messageSequence[i].id + System.Environment.NewLine;
                foreach (Message.Tag t in messageSequence[i].tags)
                {
                    outputText += "< " + t.name + " " + t.weight + "> ";
                }
                outputText += System.Environment.NewLine;
            }
        }
        messageSequenceText.text = outputText;
    }

    public void InitializeSequenceMessages()
    {
        sequenceInitialized = true;
        messageSequence = new Message[15];
        messageTimeOfSpawn = new float[15];
    }

    public void GenerateMessageSequence()
    {
        float rangeMin = 0.0f;
        float rangeMax = 50.0f;

        for (int i = 0; i < 15; i++)
        {
            //int eventID = this.GetComponent<PlayModeManager>().getMessageId();
            Message msg = CreateMessage(i, false);
            messageSequence[i] = msg;
            messageTimeOfSpawn[i] = UnityEngine.Random.Range(rangeMin, rangeMax);

            rangeMin += 50.0f;
            rangeMax += 50.0f;
        }
    }

    private void SpawnEvent()
    {
        //selectedNPC.GetComponent<NPCData>().isMessageOfInterest(Message msg);

        int eventID = this.GetComponent<PlayModeManager>().getMessageId();
        Message msg = CreateMessage(eventID, false);
        GameObject selectedNPC = npcHolder.transform.GetChild(UnityEngine.Random.Range(0, npcHolder.transform.childCount)).gameObject;

        //rotates between random NPCs until it finds one who has interest in the message
        while (!selectedNPC.GetComponent<NPCData>().isMessageOfInterest(msg))
        {
            selectedNPC = npcHolder.transform.GetChild(UnityEngine.Random.Range(0, npcHolder.transform.childCount)).gameObject;
        }

        selectedNPC.GetComponent<NPCData>().ReceiveMessage(msg);

        uiManager.messageTrackingID.text = eventID.ToString();
        selectedNPC.GetComponent<NPCFeedbackUpdater>().checkMessageFeedback();

        timeToNextEvent = UnityEngine.Random.Range(timeBetweenEventsMin, timeBetweenEventsMax);

        Debug.Log("Spawned event " + msg.ToString() + "  " + selectedNPC.name);
    }

    public Message CreateMessage(int eventID, bool isGolden, int weight = 0)
    {
        string tagA = eventTags[UnityEngine.Random.Range(0, eventTags.Count)];
        string tagB = eventTags[UnityEngine.Random.Range(0, eventTags.Count)];


        int valueTagA = UnityEngine.Random.Range(tagValueMin, tagValueMax);
        int valueTagB = UnityEngine.Random.Range(tagValueMin, tagValueMax);

        if (weight != 0)
        {
            valueTagA = weight;
            valueTagB = weight;
        }

        while (tagA == tagB)
        {
            tagB = eventTags[UnityEngine.Random.Range(0, eventTags.Count)];
        }
        string TAGS = tagA + " " + valueTagA + "," + tagB + " " + valueTagB;

        if (isGolden)
        {
            return new Message(eventID, messageTime, "This is Golden event " + tagA + " and " + tagB, TAGS);
        }
        else
        {
            return new Message(eventID, messageTime, "This is an event " + tagA + " and " + tagB, TAGS);
        }
    }

    void Update () {
        timeToNextElementalBeaconEvent -= Time.deltaTime;
        if(timeToNextElementalBeaconEvent <= 0.0f && this.GetComponent<MySQLManager>().playerMode != ""
            /*&& this.GetComponent<MySQLManager>().playerMode != "1"*/)
        {
            //SpawnElementalEvent();
            SpawnMode3();

            timeToNextElementalBeaconEvent = UnityEngine.Random.Range(timebetweenElementalEventsMin, timebetweenElementalEventsMax);
            //Debug.Log("timeToNextElementalBeaconEvent: " + timeToNextElementalBeaconEvent);
        }

        if (messageSequenceText.gameObject.transform.parent.gameObject.activeSelf)
        {
            RefreshMessageSequenceText();
        }
        if (isOnSequence && sequenceInitialized)
        {
            //Check if its time for the message to me spawned
            if(messageTimeOfSpawn[currentMessageOnSequence] <= Time.timeSinceLevelLoad)
            {
                GameObject selectedNPC = npcHolder.transform.GetChild(UnityEngine.Random.Range(0, npcHolder.transform.childCount)).gameObject;

                //rotates between random NPCs until it finds one who has interest in the message
                while (!selectedNPC.GetComponent<NPCData>().isMessageOfInterest(messageSequence[currentMessageOnSequence]))
                {
                    selectedNPC = npcHolder.transform.GetChild(UnityEngine.Random.Range(0, npcHolder.transform.childCount)).gameObject;
                }

                int eventID = this.GetComponent<PlayModeManager>().getMessageId();
                messageSequence[currentMessageOnSequence].id = eventID;

                selectedNPC.GetComponent<NPCData>().ReceiveMessage(new Message(messageSequence[currentMessageOnSequence]));

                uiManager.messageTrackingID.text = messageSequence[currentMessageOnSequence].id.ToString();
                selectedNPC.GetComponent<NPCFeedbackUpdater>().checkMessageFeedback();

                currentMessageOnSequence += 1;
            }
        }
        else
        {
            //Temporary removal of beacon ... only premade message sequence works

            /*
            timeToNextEvent -= Time.deltaTime;
            if (timeToNextEvent <= 0)
            {
                if (npcHolder.transform.childCount > 0)
                {
                    SpawnEvent();
                }
                else
                {
                    timeToNextEvent = UnityEngine.Random.Range(timeBetweenEventsMin, timeBetweenEventsMax);
                }
            }
            */
        }
    }
}
