using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCData : MonoBehaviour {

    public string npcName;
    public float assertiveness;
    public float cooperativeness;
    public int NPCType;

    public List<Interest> interests;
    public List<Aquaintance> aquaintances;
    public List<Message> messages;
    public List<string> patrolPointIndex;

    public SpriteRenderer Body;
    public SpriteRenderer Head;
    public SpriteRenderer LeftHand;
    public SpriteRenderer RightHand;

    float messageLimit = 3;

    public float currentAssertivenessLevel;
    public float currentCooperativenessLevel;

    UIManager uiManager;

    float ForestVilageMinX;
    float ForestVilageMaxX;
    float ForestVilageMinZ;
    float ForestVilageMaxZ;

    float SnowVilageMinX;
    float SnowVilageMaxX;
    float SnowVilageMinZ;
    float SnowVilageMaxZ;

    float DesertVilageMinX;
    float DesertVilageMaxX;
    float DesertVilageMinZ;
    float DesertVilageMaxZ;

    float IslandVilageMinX;
    float IslandVilageMaxX;
    float IslandVilageMinZ;
    float IslandVilageMaxZ;

    bool activatedCanvasWatchedEvent;
    float timeSinceCanvasActivation;
    float watchedCanvasDuration;
    public GameObject watchedEventCanvas;

    public Message lastMessageReceived;
    private void Start()
    {
        timeSinceCanvasActivation = 0.0f;
        //NPCs show that they watched the event during 4 seconds
        watchedCanvasDuration = 4.0f;

        ForestVilageMinX = -49.0f;
        ForestVilageMaxX = -8.0f;
        ForestVilageMinZ = 17.0f;
        ForestVilageMaxZ = 49.0f;

        SnowVilageMinX = -49.0f;
        SnowVilageMaxX = -23.0f;
        SnowVilageMinZ = -48.0f;
        SnowVilageMaxZ = 11.0f;

        DesertVilageMinX = 11.0f;
        DesertVilageMaxX = 48.0f;
        DesertVilageMinZ = 15.0f;
        DesertVilageMaxZ = 49.0f;

        IslandVilageMinX = 21.0f;
        IslandVilageMaxX = 48.0f;
        IslandVilageMinZ = -31.0f;
        IslandVilageMaxZ = 5.0f;

        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        currentAssertivenessLevel = 0.0f;
        currentCooperativenessLevel = 0.0f;
    }

    public List<GameObject> canvasWatchedEvents;

    private void FixedUpdate()
    {
        DecayMessages();
    }

    private void Update()
    {
        if (activatedCanvasWatchedEvent)
        {
            watchedEventCanvas.transform.position = this.transform.GetChild(1).gameObject.transform.position;
            timeSinceCanvasActivation += Time.deltaTime;
            if (timeSinceCanvasActivation > watchedCanvasDuration)
            {
                activatedCanvasWatchedEvent = false;
                DisableCanvasWatchedEvent();
                timeSinceCanvasActivation = 0.0f;
            }
        }
        //It takes 5 seconds to reach full assertiveness and cooperativeness
        if (!this.GetComponent<Social>().isTalking)
        {
            currentAssertivenessLevel += (Time.deltaTime / 5.0f )* assertiveness;
            currentCooperativenessLevel += (Time.deltaTime / 5.0f) * cooperativeness;
        }

        currentAssertivenessLevel = Mathf.Clamp(currentAssertivenessLevel, 0.0f, 1.0f);
        currentCooperativenessLevel = Mathf.Clamp(currentCooperativenessLevel, 0.0f, 1.0f);
    }

    [System.Serializable]
    public class Interest
    {
        public string name;
        public float weight;

        public Interest(string name, float weight)
        {
            this.name = name;
            this.weight = weight;
        }
    };

    [System.Serializable]
    public class Aquaintance
    {
        public string npcName;
        public int friendshipLevel;

        public Aquaintance(string n, int f)
        {
            npcName = n;
            friendshipLevel = f;
        }
    };

    public void ShuffleInterests()
    {
        if (NPCType == 0)
        {
            ShuffleAssCoop();
            interests.Clear();
            if (transform.position.x > ForestVilageMinX && transform.position.x < ForestVilageMaxX && transform.position.z > ForestVilageMinZ && transform.position.z < ForestVilageMaxZ)
            {
                Shuffle("Wood", "Rock", "Berries", "Cactus");
            }
            else if (transform.position.x > SnowVilageMinX && transform.position.x < SnowVilageMaxX && transform.position.z > SnowVilageMinZ && transform.position.z < SnowVilageMaxZ)
            {
                Shuffle("Rock", "Berries", "Cactus", "Wood");
            }
            else if (transform.position.x > IslandVilageMinX && transform.position.x < IslandVilageMaxX && transform.position.z > IslandVilageMinZ && transform.position.z < IslandVilageMaxZ)
            {
                Shuffle("Berries", "Cactus", "Wood", "Rock");
            }
            else if (transform.position.x > DesertVilageMinX && transform.position.x < DesertVilageMaxX && transform.position.z > DesertVilageMinZ && transform.position.z < DesertVilageMaxZ)
            {
                Shuffle("Cactus", "Wood", "Rock", "Berries");
            }
            else
            {
                ShuffleRandom();
            }
        }
    }

    public void ShuffleAssCoop()
    {
        assertiveness = Random.Range(0.2f, 0.8f);
        cooperativeness = Random.Range(0.2f, 0.8f);
    }

    public void Shuffle(string tag1, string tag2, string tag3, string tag4)
    {
        float randomValue = 0.0f;
        float totalWeight = 0.0f;

        randomValue = Random.Range(75.0f, 100.0f);
        totalWeight += randomValue;
        interests.Add(new Interest(tag1, randomValue));

        if (Random.Range(0, 100) < 60)
        {
            randomValue = Random.Range(50.0f, 75.0f);
            totalWeight += randomValue;
            interests.Add(new Interest(tag2, randomValue));

            if (Random.Range(0, 100) < 60)
            {
                randomValue = Random.Range(25.0f, 50.0f);
                totalWeight += randomValue;
                interests.Add(new Interest(tag3, randomValue));

                if (Random.Range(0, 100) < 60)
                {
                    randomValue = Random.Range(0.0f, 25.0f);
                    totalWeight += randomValue;
                    interests.Add(new Interest(tag4, randomValue));
                }
            }
        }
        foreach(Interest i in interests)
        {
            i.weight = i.weight / totalWeight;
        }

        Debug.Log("Shuffling " + tag1);
    }

    public void ShuffleRandom()
    {
        string[] TAGS = { "Wood", "Rock", "Berries", "Cactus" };
        ArrayList choosenTAGS = new ArrayList();

        float randomValue = 0.0f;
        float totalWeight = 0.0f;

        randomValue = Random.Range(75.0f, 100.0f);
        totalWeight += randomValue;
        string tag1 = TAGS[Random.Range(0, TAGS.Length)];
        choosenTAGS.Add(tag1);
        interests.Add(new Interest(tag1, randomValue));

        if (Random.Range(0, 100) < 70)
        {
            string tag2 = TAGS[Random.Range(0, TAGS.Length)];
            while (choosenTAGS.Contains(tag2))
            {
                tag2 = TAGS[Random.Range(0, TAGS.Length)];
            }
            choosenTAGS.Add(tag2);
            randomValue = Random.Range(50.0f, 75.0f);
            totalWeight += randomValue;
            interests.Add(new Interest(tag2, randomValue));

            if (Random.Range(0, 100) < 70)
            {
                string tag3 = TAGS[Random.Range(0, TAGS.Length)];
                while (choosenTAGS.Contains(tag3))
                {
                    tag3 = TAGS[Random.Range(0, TAGS.Length)];
                }
                choosenTAGS.Add(tag3);

                randomValue = Random.Range(25.0f, 80.0f);
                totalWeight += randomValue;
                interests.Add(new Interest(tag3, randomValue));

                if (Random.Range(0, 100) < 70)
                {
                    string tag4 = TAGS[Random.Range(0, TAGS.Length)];
                    while (choosenTAGS.Contains(tag4))
                    {
                        tag4 = TAGS[Random.Range(0, TAGS.Length)];
                    }
                    choosenTAGS.Add(tag4);
                    randomValue = Random.Range(0.0f, 25.0f);
                    totalWeight += randomValue;
                    interests.Add(new Interest(tag4, randomValue));
                }
            }
        }
        foreach (Interest i in interests)
        {
            i.weight = i.weight / totalWeight;
        }
    }

    public void DecayMessages()
    {
        //It takes 3 minutes for a message to reach zero
        foreach (Message m in messages)
        {
            if (uiManager.isLinearDecayment)
            {
                //Linear decayment
                m.messageDecayment -= Time.deltaTime / 180.0f;
                if (m.messageDecayment < 0.0f)
                {
                    //string logMessage = "Removed from " + npcName + " the message " + m.description + " with id " + m.id;
                    //GameObject.FindGameObjectWithTag("GameManager").GetComponent<SimulationDataLogger>().WriteTextToLog(logMessage);

                    //messages.Remove(m);

                    m.messageDecayment = 0.0f;
                }
            }
            else
            {
                //Variable decayment
                m.messageDecayment -= (m.messageDecayment / 32.0f) * Time.deltaTime;

               // m.messageDecayment = m.messageDecayment / (1 + Time.deltaTime);
                if (m.messageDecayment < 0.0001f)
                {
                    //string logMessage = "Removed from " + npcName + " the message " + m.description + " with id " + m.id;
                    //GameObject.FindGameObjectWithTag("GameManager").GetComponent<SimulationDataLogger>().WriteTextToLog(logMessage);

                    //messages.Remove(m);

                    m.messageDecayment = 0.0f;
                }
            }
        }
        /*
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            if(messages[i].messageDecayment < 0.0f)
            {
                messages.RemoveAt(i);
            }
        }
        */
    }

    public bool isMessageOfInterest(Message msg)
    {
        //Debug.Log("messages count: " + messages.Count);
        if (messages.Count < messageLimit)
        {
            //Debug.Log("Add new message");
            return true;
        }
        else
        {
            float recievedMessageScore = 0.0f;
            float lessInterestingMessageScore = Mathf.Infinity;
            Message lessInterestingMessage = null;
            foreach (Message.Tag tag in msg.tags)
            {
                Interest foundInterest = interests.Find(x => x.name == tag.name);
                if (foundInterest != null)
                {
                    recievedMessageScore += foundInterest.weight * tag.weight;
                }
            }

            //recievedMessageScore *= msg.messageDecayment;

            foreach (Message m in messages)
            {
                float totalScore = 0.0f;
                foreach (Message.Tag tag in m.tags)
                {
                    Interest foundInterest = interests.Find(x => x.name == tag.name);
                    if (foundInterest != null)
                    {
                        totalScore += foundInterest.weight * tag.weight;
                    }
                }

                //Total score is multiplied by the decayment of the message
                totalScore *= m.messageDecayment;

                if (totalScore < lessInterestingMessageScore)
                {
                    lessInterestingMessage = m;
                    lessInterestingMessageScore = totalScore;
                }
            }

            //If the message we recieved is more interesting than one of our messages ...
            //We replace the least interesting message with our new recieved message
            if (recievedMessageScore > lessInterestingMessageScore)
            {
                if (messages.Find(x => x.id == msg.id) == null)
                {
                    string logMessage = "Removed from " + npcName + " message ID: " + lessInterestingMessage.id + " <> " + lessInterestingMessage.description;
                    GameObject.FindGameObjectWithTag("GameManager").GetComponent<SimulationDataLogger>().WriteRemoveToLog(logMessage, lessInterestingMessage.id);

                    messages.Remove(lessInterestingMessage);
                }
                return true;
            }
        }
        return false;
    }

    public bool ReceiveMessage(Message msg)
    {
        if (isMessageOfInterest(msg))
        {
            messages.Add(msg);
            lastMessageReceived = msg;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ActivateWatchedEvent(int id)
    {
        DisableCanvasWatchedEvent();
        canvasWatchedEvents[id].SetActive(true);

        activatedCanvasWatchedEvent = true;
    }

    public void DisableCanvasWatchedEvent()
    {
        foreach(GameObject canvas in canvasWatchedEvents)
        {
            canvas.SetActive(false);
        }
    }

    public void InitializeNPCData(string npcName, string thisInterests, 
        string aquaintancesText, string messagesText, string patrolPointIndexText,
        float assertivenessLevel, float cooperativenessLevel, int NPCType,
        Color bodyColor, Color headColor, Color handsColor)
    {
        Body.color = bodyColor;
        Head.color = headColor;
        LeftHand.color = handsColor;
        RightHand.color = handsColor;
        
        //Debug.Log("npcName: " + npcName);
        //Debug.Log("thisInterests:<" + thisInterests + ">");
        //Debug.Log("aquaintancesText:<" + aquaintancesText + ">");
        //Debug.Log("messagesText:<" + messagesText + ">");
        this.npcName = npcName;
        this.NPCType = NPCType;
        cooperativeness = cooperativenessLevel;
        assertiveness = assertivenessLevel;
        messages = new List<Message>();

        if (thisInterests != "" && thisInterests != " ")
        {
            string[] interestsList = thisInterests.Split(',');

            //Normalize interest so the total is equal to 100
            float interestTotalWeight = 0;
            foreach (string i in interestsList)
            {
                string[] interestData = i.Split(' ');
                interestTotalWeight += float.Parse(interestData[1]);
            }

            foreach (string i in interestsList)
            {
                string[] interestData = i.Split(' ');
                //All interests are normalized
                interests.Add(new Interest(interestData[0], float.Parse(interestData[1]) / interestTotalWeight));
            }
        }
        if (aquaintancesText != "" && aquaintancesText != " ")
        {
            string[] aquaintancesList = aquaintancesText.Split(',');
            foreach (string j in aquaintancesList)
            {
                string[] aquaintanceData = j.Split(' ');
                aquaintances.Add(new Aquaintance(aquaintanceData[0], System.Int32.Parse(aquaintanceData[1])));
            }
        }
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
        if (patrolPointIndexText != "" && patrolPointIndexText != " ")
        {
            string[] patrolPointIndexList = patrolPointIndexText.Split(',');
            foreach (string j in patrolPointIndexList)
            {
                patrolPointIndex.Add(j);
            }
        }
    }
}
