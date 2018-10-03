using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject SidePanel;
    public GameObject mainPanel;
    public GameObject messageSequence_Panel;
    public GameObject drawTerrainPanel;
    public GameObject addElementPanel;
    public GameObject addNPC_Panel;
    public GameObject SpawnEvent_Panel;
    public GameObject inspectorPanel;
    public GameObject canvasBackroundBox;

    public GameObject AnswerQuestions_Panel;

    public GameObject playerMenu;

    public GameObject questsPanel;
    public GameObject stashPanel;
    public GameObject InstructionPanel;

    public GameObject spawnEventPlayModePanel;

    public GameObject listNatural;
    public GameObject listConstruct;

    public GameObject interestNPCList;
    public GameObject tagEventSpawnerList;
    public GameObject tagEventSpawnerListPlayMode;
    public GameObject friendsNPCList;
    public GameObject interestChangeTagButton;
    public GameObject interestChangeTagList;
    public GameObject interestWeightInputField;
    public GameObject friendNameInputField;
    public GameObject friendLevelInputField;
    public GameObject separator;

    public GameObject patrolPointNumber;
    public GameObject listPatrolPoints;

    public GameObject npcTypeList;
    public GameObject npcTypeButton;

    public GameObject npcHolder;
    public GameObject patrolPointHolder;

    GameObject gm;
    GameObject player;

    public bool isFeedbackEnabled;
    public bool isWatchModeEnabled;
    public bool isGoalFeedbackEnabled;

    public bool isPatrolPointLayerEnabled;
    public GameObject patrolPointLayerSelectedFeedback;

    public bool isMessageLayerEnabled;
    public InputField messageTrackingID;

    public GameObject MessageFeedbackLayer;

    public GameObject patrolPointInspectorPanel;
    public GameObject PatrolPointBeingUpdated;
    public GameObject patrolPointMessagesHolder;
    public GameObject patrolPointIdName;
    public List<int> messageIdPatrolPointToRemove;

    public GameObject npcUpdaterPanel;
    public GameObject npcUpdaterName;
    public GameObject npcUpdaterAssertiveness;
    public GameObject npcUpdaterCooperativeness;
    public GameObject npcUpdaterInterestHolder;
    public GameObject npcUpdaterMessagesHolder;
    public GameObject messageElement;
    public GameObject NPCBeingUpdated;
    public List<int> messageIdsToRemoveNPCUpdater;

    public GameObject watchModeSelectedFeedback;

    public GameObject linearDecaymentButton;
    public GameObject variableDecaymentButton;
    public bool isLinearDecayment;

    public GameObject messageSequenceSelectedFeedback;

    void Start()
    {
        SetVariableDecayment();

        messageIdsToRemoveNPCUpdater = new List<int>();
        messageIdPatrolPointToRemove = new List<int>();
        isFeedbackEnabled = false;
        isWatchModeEnabled = false;
        isPatrolPointLayerEnabled = false;
        gm = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void ShowNaturals()
    {
        listNatural.SetActive(true);
        listConstruct.SetActive(false);
    }

    public void ShowConstructs()
    {
        listNatural.SetActive(false);
        listConstruct.SetActive(true);
    }

    public void ShowMessageSequencePanel()
    {
        mainPanel.SetActive(false);
        messageSequence_Panel.SetActive(true);
        gm.GetComponent<Beacon>().RefreshMessageSequenceText();
    }

    public void SetLinearDecayment()
    {
        isLinearDecayment = true;
        linearDecaymentButton.GetComponent<Image>().color = Color.yellow;
        variableDecaymentButton.GetComponent<Image>().color = Color.white;
    }
    public void SetVariableDecayment()
    {
        isLinearDecayment = false;
        linearDecaymentButton.GetComponent<Image>().color = Color.white;
        variableDecaymentButton.GetComponent<Image>().color = Color.yellow;
    }

    public void ShuffleNPCInterests()
    {
        foreach (Transform npc in npcHolder.transform)
        {
            npc.gameObject.GetComponent<NPCData>().ShuffleInterests();
        }
    }

    public void ShuffleMessageSequence()
    {
        gm.GetComponent<Beacon>().GenerateMessageSequence();
    }

    public void ToggleMessagePlaySequence()
    {
        if (Time.timeSinceLevelLoad == 0.0f)
        {
            if (gm.GetComponent<Beacon>().isOnSequence)
            {
                gm.GetComponent<Beacon>().isOnSequence = false;
                messageSequenceSelectedFeedback.SetActive(false);
            }
            else
            {
                gm.GetComponent<Beacon>().isOnSequence = true;
                messageSequenceSelectedFeedback.SetActive(true);
            }
        }
    }

    public void ToggleWatchMode()
    {
        if (isWatchModeEnabled)
        {
            //Enables the quests panel
            questsPanel.SetActive(true);
            stashPanel.SetActive(true);

            gm.GetComponent<QuestsController>().UpdateQuestsBar();

            isWatchModeEnabled = false;

            SidePanel.SetActive(false);
            canvasBackroundBox.SetActive(false);
            gm.GetComponent<EditorModeController>().isEditorMode = false;
            gm.GetComponent<Zoom>().zoomToPlayMode = true;

            gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(false);

            Time.timeScale = this.GetComponent<TimeSpeedController>().currentTime;

            watchModeSelectedFeedback.SetActive(false);
        }
        else
        {
            //Disables the quests panel
            questsPanel.SetActive(false);
            stashPanel.SetActive(false);

            //Player must stop atacking
            player.GetComponent<PlayerMovement>().isAtacking = false;
            player.GetComponent<Animator>().SetBool("Attack", false);
            //If player was moving before it stops the player
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponent<Animator>().SetBool("Walk", false);

            //Stop player rotation
            player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            isWatchModeEnabled = true;

            Time.timeScale = this.GetComponent<TimeSpeedController>().currentTime;

            SidePanel.SetActive(true);
            canvasBackroundBox.SetActive(true);

            gm.GetComponent<EditorModeController>().isEditorMode = true;
            spawnEventPlayModePanel.SetActive(false);

            gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(false);

            watchModeSelectedFeedback.SetActive(true);
        }
    }

    public void ToggleFeedback()
    {
        if (isFeedbackEnabled)
        {
            foreach (Transform npc in npcHolder.transform)
            {
                npc.gameObject.GetComponent<NPCFeedbackUpdater>().feedbackCanvas.SetActive(false);
            }
            isFeedbackEnabled = false;
        }
        else
        {
            foreach (Transform npc in npcHolder.transform)
            {
                npc.gameObject.GetComponent<NPCFeedbackUpdater>().feedbackCanvas.SetActive(true);
                npc.gameObject.GetComponent<NPCFeedbackUpdater>().refreshFeedbackCanvas();
            }
            isFeedbackEnabled = true;
        }
    }

    public void ToggleGoalFeedback()
    {
        if (isGoalFeedbackEnabled)
        {
            //foreach (Transform npc in npcHolder.transform)
            //{
            //    npc.gameObject.GetComponent<NPCFeedbackUpdater>().feedbackCanvas.SetActive(false);
            //}
            isGoalFeedbackEnabled = false;
        }
        else
        {
            //foreach (Transform npc in npcHolder.transform)
            //{
            //    npc.gameObject.GetComponent<NPCFeedbackUpdater>().feedbackCanvas.SetActive(true);
            //    npc.gameObject.GetComponent<NPCFeedbackUpdater>().refreshFeedbackCanvas();
            //}
            isGoalFeedbackEnabled = true;
        }
    }

    public void ToggleMessageLayer()
    {
        if (isMessageLayerEnabled)
        {
            MessageFeedbackLayer.SetActive(false);
            isMessageLayerEnabled = false;

            foreach (Transform npc in npcHolder.transform)
            {
                npc.gameObject.GetComponent<NPCFeedbackUpdater>().feedbackMessageCanvas.SetActive(false); ;
            }
        }
        else
        {
            MessageFeedbackLayer.SetActive(true);
            isMessageLayerEnabled = true;

            foreach (Transform npc in npcHolder.transform)
            {
                npc.gameObject.GetComponent<NPCFeedbackUpdater>().checkMessageFeedback();
            }
        }
    }

    public void TogglePatrolPointLayer()
    {
        if (isPatrolPointLayerEnabled)
        {
            patrolPointLayerSelectedFeedback.SetActive(false);
            isPatrolPointLayerEnabled = false;

            foreach (Transform patrolPoint in patrolPointHolder.transform)
            {
                patrolPoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                patrolPoint.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            patrolPointLayerSelectedFeedback.SetActive(true);
            isPatrolPointLayerEnabled = true;

            foreach (Transform patrolPoint in patrolPointHolder.transform)
            {
                patrolPoint.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                patrolPoint.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void refreshMessageTrackID()
    {
        foreach (Transform npc in npcHolder.transform)
        {
            npc.gameObject.GetComponent<NPCFeedbackUpdater>().checkMessageFeedback();
        }
    }

    public void RefreshPatrolPointInspectorPanel(PatrolPointData data)
    {
        messageIdPatrolPointToRemove = new List<int>();
        patrolPointInspectorPanel.SetActive(true);
        npcUpdaterPanel.SetActive(false);
        PatrolPointBeingUpdated = data.gameObject;

        patrolPointIdName.GetComponent<Text>().text = "Patrol Point #" + PatrolPointBeingUpdated.transform.GetChild(0).GetComponent<TextMesh>().text;

        foreach (Transform child in patrolPointMessagesHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Message message in data.messages)
        {
            GameObject myMessageElement = Instantiate(messageElement, patrolPointMessagesHolder.transform);
            string messageText = "";
            messageText += "ID: " + message.id + "<TAGs> ";
            foreach (Message.Tag t in message.tags)
            {
                messageText += "(" + t.name + "," + t.weight + "),";
            }
            if (message.tags.Count > 0)
            {
                messageText = messageText.Substring(0, messageText.Length - 1);
            }
            myMessageElement.GetComponentInChildren<Text>().text = messageText;

            myMessageElement.GetComponent<SingleMessageController>().messageType = 1;
            myMessageElement.GetComponent<SingleMessageController>().messageId = message.id;
        }
    }


    public void RefreshNPCUpdater(NPCData data)
    {
        messageIdsToRemoveNPCUpdater = new List<int>();
        npcUpdaterPanel.SetActive(true);
        patrolPointInspectorPanel.SetActive(false);
        NPCBeingUpdated = data.gameObject;

        npcUpdaterName.GetComponent<InputField>().text = data.npcName;
        npcUpdaterAssertiveness.GetComponent<Slider>().value = data.assertiveness;
        npcUpdaterCooperativeness.GetComponent<Slider>().value = data.cooperativeness;

        foreach (Transform child in npcUpdaterInterestHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in npcUpdaterMessagesHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (NPCData.Interest i in data.interests)
        {
            GameObject myInterestChangeTagButton = Instantiate(interestChangeTagButton, npcUpdaterInterestHolder.transform);
            GameObject myInterestChangeTagList = Instantiate(interestChangeTagList, npcUpdaterInterestHolder.transform);
            myInterestChangeTagButton.GetComponent<TagListSelectorController>().listOfTags = myInterestChangeTagList;

            foreach (SingleTagController stc in myInterestChangeTagList.GetComponentsInChildren<SingleTagController>())
            {
                stc.tagButton = myInterestChangeTagButton;
            }

            GameObject myInterestWeight = Instantiate(interestWeightInputField, npcUpdaterInterestHolder.transform);
            Instantiate(separator, npcUpdaterInterestHolder.transform);

            myInterestChangeTagButton.GetComponentInChildren<Text>().text = i.name;
            myInterestWeight.GetComponent<InputField>().text = i.weight.ToString();
        }

        foreach (Message message in data.messages)
        {
            GameObject myMessageElement = Instantiate(messageElement, npcUpdaterMessagesHolder.transform);
            string messageText = "";
            messageText += "ID: " + message.id + " Decayment: " + message.messageDecayment + System.Environment.NewLine;
            foreach(Message.Tag t in message.tags)
            {
                messageText += "(" + t.name + "," + t.weight + "),";
            }
            if(message.tags.Count > 0)
            {
                messageText = messageText.Substring(0, messageText.Length - 1);
            }
            myMessageElement.GetComponentInChildren<Text>().text = messageText;

            myMessageElement.GetComponent<SingleMessageController>().messageType = 0;
            myMessageElement.GetComponent<SingleMessageController>().messageId = message.id;
        }
    }

    public void UpdatePatrolPoint()
    {
        foreach (int id in messageIdPatrolPointToRemove)
        {
            Message m = PatrolPointBeingUpdated.GetComponent<PatrolPointData>().messages.Find(x => x.id == id);
            if (m != null)
            {
                PatrolPointBeingUpdated.GetComponent<PatrolPointData>().messages.Remove(m);
            }
        }
    }

    public void UpdateNPC()
    {
        NPCBeingUpdated.GetComponent<NPCData>().npcName = npcUpdaterName.GetComponent<InputField>().text;
        NPCBeingUpdated.GetComponent<NPCData>().assertiveness = npcUpdaterAssertiveness.GetComponent<Slider>().value;
        NPCBeingUpdated.GetComponent<NPCData>().cooperativeness = npcUpdaterCooperativeness.GetComponent<Slider>().value;

        List<GameObject> Interests = new List<GameObject>();
        foreach (Transform npc in npcUpdaterInterestHolder.transform)
        {
            Interests.Add(npc.gameObject);
        }

        NPCBeingUpdated.GetComponent<NPCData>().interests = new List<NPCData.Interest>();

        if (Interests.Count > 0)
        {
            float interestTotalWeight = 0;
            for (int i = 0; i < Interests.Count; i = i + 4)
            {
                if (!Interests[i].GetComponentInChildren<Text>().text.Equals("Interest Name"))
                {
                    interestTotalWeight += float.Parse(Interests[i + 2].GetComponent<InputField>().text);
                }
            }

            for (int i = 0; i < Interests.Count; i = i + 4)
            {
                if (!Interests[i].GetComponentInChildren<Text>().text.Equals("Interest Name"))
                {
                    NPCBeingUpdated.GetComponent<NPCData>().interests.Add(
                        new NPCData.Interest(Interests[i].GetComponentInChildren<Text>().text,
                        float.Parse(Interests[i + 2].GetComponent<InputField>().text) / interestTotalWeight));
                    Interests[i + 2].GetComponent<InputField>().text = 
                        (float.Parse(Interests[i + 2].GetComponent<InputField>().text) / interestTotalWeight).ToString();
                }
            }
        }

        foreach (int id in messageIdsToRemoveNPCUpdater)
        {
            Message m = NPCBeingUpdated.GetComponent<NPCData>().messages.Find(x => x.id == id);
            if (m != null)
            {
                NPCBeingUpdated.GetComponent<NPCData>().messages.Remove(m);
            }
        }
    }

    public void addUpdatedInterestToNPC()
    {
        GameObject myInterestChangeTagButton = Instantiate(interestChangeTagButton, npcUpdaterInterestHolder.transform);
        GameObject myInterestChangeTagList = Instantiate(interestChangeTagList, npcUpdaterInterestHolder.transform);
        myInterestChangeTagButton.GetComponent<TagListSelectorController>().listOfTags = myInterestChangeTagList;

        foreach (SingleTagController stc in myInterestChangeTagList.GetComponentsInChildren<SingleTagController>())
        {
            stc.tagButton = myInterestChangeTagButton;
        }

        Instantiate(interestWeightInputField, npcUpdaterInterestHolder.transform);
        Instantiate(separator, npcUpdaterInterestHolder.transform);
    }

    public void removeMessageWithId(int id)
    {
        messageIdsToRemoveNPCUpdater.Add(id);
    }

    public void removePatrolPointMessageWithId(int id)
    {
        messageIdPatrolPointToRemove.Add(id);
    }

    public void addInterestToNPC()
    {
        GameObject myInterestChangeTagButton = Instantiate(interestChangeTagButton, interestNPCList.transform);
        GameObject myInterestChangeTagList = Instantiate(interestChangeTagList, interestNPCList.transform);
        myInterestChangeTagButton.GetComponent<TagListSelectorController>().listOfTags = myInterestChangeTagList;

        foreach(SingleTagController stc in myInterestChangeTagList.GetComponentsInChildren<SingleTagController>())
        {
            stc.tagButton = myInterestChangeTagButton;
        }

        Instantiate(interestWeightInputField, interestNPCList.transform);
        Instantiate(separator, interestNPCList.transform);
    }

    public void addTagToEvent(int mode)
    {
        Transform myTransform = tagEventSpawnerList.transform;

        if(mode == 1)
        {
            myTransform = tagEventSpawnerListPlayMode.transform;
        }

        GameObject myInterestChangeTagButton = Instantiate(interestChangeTagButton, myTransform);
        GameObject myInterestChangeTagList = Instantiate(interestChangeTagList, myTransform);
        myInterestChangeTagButton.GetComponent<TagListSelectorController>().listOfTags = myInterestChangeTagList;
        myInterestChangeTagButton.GetComponentInChildren<Text>().text = "Tag name";

        foreach (SingleTagController stc in myInterestChangeTagList.GetComponentsInChildren<SingleTagController>())
        {
            stc.tagButton = myInterestChangeTagButton;
        }

        GameObject tagWeight = Instantiate(interestWeightInputField, myTransform);
        tagWeight.GetComponent<InputField>().text = "0";
        tagWeight.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;

        Instantiate(separator, myTransform);
    }

    public void addFriendToNPC()
    {
        Instantiate(friendNameInputField, friendsNPCList.transform);
        Instantiate(friendLevelInputField, friendsNPCList.transform);
        Instantiate(separator, friendsNPCList.transform);
    }

    public void ToggleSpawnEventPlayModePanel()
    {
        if (!gm.GetComponent<EditorModeController>().isEditorMode)
        {
            if (spawnEventPlayModePanel.activeSelf)
            {
                spawnEventPlayModePanel.SetActive(false);
                canvasBackroundBox.SetActive(false);
                gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(false);
            }
            else
            {
                spawnEventPlayModePanel.SetActive(true);
                canvasBackroundBox.SetActive(true);
                gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(true);
            }
        }
    }

    public void addNumberToPatrolNPC()
    {
        Instantiate(patrolPointNumber, listPatrolPoints.transform);
        
    }

    public void RemoveLast(int type)
    {
        //TAGS
        if (type == 0)
        {
            List<GameObject> Interests = new List<GameObject>();
            foreach (Transform npc in interestNPCList.transform)
            {
                Interests.Add(npc.gameObject);
            }
            if (Interests.Count > 0)
            {
                for (int i = Interests.Count - 1; i >= Interests.Count - 4; i--)
                {
                    Destroy(Interests[i]);
                }
            }
        }
        //FRIENDS
        else if(type == 1)
        {
            List<GameObject> Friends = new List<GameObject>();
            foreach (Transform npc in friendsNPCList.transform)
            {
                Friends.Add(npc.gameObject);
            }
            if (Friends.Count > 0)
            {
                for (int i = Friends.Count - 1; i >= Friends.Count - 3; i--)
                {
                    Destroy(Friends[i]);
                }
            }
        }
        else if (type == 2)
        {
            List<GameObject> PatrolPointNumber = new List<GameObject>();
            foreach (Transform patrol in listPatrolPoints.transform)
            {
                PatrolPointNumber.Add(patrol.gameObject);
            }
            if (PatrolPointNumber.Count > 0)
            {
                Destroy(PatrolPointNumber[PatrolPointNumber.Count - 1]);
            }
        }
        //Removes last Interest from NPC Updater
        else if (type == 3)
        {
            List<GameObject> Interests = new List<GameObject>();
            foreach (Transform npc in npcUpdaterInterestHolder.transform)
            {
                Interests.Add(npc.gameObject);
            }
            if (Interests.Count > 0)
            {
                for (int i = Interests.Count - 1; i >= Interests.Count - 4; i--)
                {
                    Destroy(Interests[i]);
                }
            }
        }
        //Removes last tag from Event Spawner
        else if (type == 4)
        {
            List<GameObject> TagsEventSpawner = new List<GameObject>();
            foreach (Transform npc in tagEventSpawnerList.transform)
            {
                TagsEventSpawner.Add(npc.gameObject);
            }
            if (TagsEventSpawner.Count > 0)
            {
                for (int i = TagsEventSpawner.Count - 1; i >= TagsEventSpawner.Count - 4; i--)
                {
                    Destroy(TagsEventSpawner[i]);
                }
            }
        }
        else if (type == 5)
        {
            List<GameObject> TagsEventSpawner = new List<GameObject>();
            foreach (Transform npc in tagEventSpawnerListPlayMode.transform)
            {
                TagsEventSpawner.Add(npc.gameObject);
            }
            if (TagsEventSpawner.Count > 0)
            {
                for (int i = TagsEventSpawner.Count - 1; i >= TagsEventSpawner.Count - 4; i--)
                {
                    Destroy(TagsEventSpawner[i]);
                }
            }
        }
    }

    public void ShowTerrainPanel()
    {
        mainPanel.SetActive(false);
        drawTerrainPanel.SetActive(true);
        gm.GetComponent<EditorModeController>().isDrawingTerrain = true;
    }

    public void ShowElementPanel()
    {
        mainPanel.SetActive(false);
        addElementPanel.SetActive(true);

        ShowNaturals();

        gm.GetComponent<EditorModeController>().isPlacingElements = true;
        gm.GetComponent<EditorModeController>().removeElement = false;
        gm.GetComponent<EditorModeController>().removePatrolPoint = false;
        gm.GetComponent<EditorModeController>().removeElementButtonImage.color = Color.white;
        gm.GetComponent<EditorModeController>().removePatrolButtonImage.color = Color.white;
    }

    public void ShowSpawnEventPanel()
    {
        gm.GetComponent<EditorModeController>().isSpawningEvent = true;
        mainPanel.SetActive(false);
        SpawnEvent_Panel.SetActive(true);
        gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(true);
    }

    public void ShowAddNPCPanel()
    {
        gm.GetComponent<EditorModeController>().isPlacingNPC = true;
        mainPanel.SetActive(false);
        addNPC_Panel.SetActive(true);
    }

    public void ShowInspectorPanel()
    {
        mainPanel.SetActive(false);
        inspectorPanel.SetActive(true);

        gm.GetComponent<EditorModeController>().isInspectingElement = true;
    }

    public void ToggleRemoveTerrain()
    {
        gm.GetComponent<EditorModeController>().ToggleRemoveTerrain();
    }

    public void ToggleRemoveElement()
    {
        gm.GetComponent<EditorModeController>().ToggleRemoveElement();
    }

    public void ToggleRemovePatrol()
    {
        gm.GetComponent<EditorModeController>().ToggleRemovePatrol();
    }

    public void ToggleRemoveNPC()
    {
        gm.GetComponent<EditorModeController>().ToggleRemoveNPC();
    }

    public void SetNPCType(int x)
    {
        gm.GetComponent<EditorModeController>().selectedNPCType = x;
        npcTypeList.SetActive(false);
        switch (x)
        {
            case 0:
                npcTypeButton.GetComponentInChildren<Text>().text = "Fat NPC";
                break;
            case 1:
                npcTypeButton.GetComponentInChildren<Text>().text = "Wizard NPC";
                break;
        }
    }

    public void ToggleNPCTypeList()
    {
        if (npcTypeList.activeSelf)
        {
            npcTypeList.SetActive(false);
        }
        else
        {
            npcTypeList.SetActive(true);
        }
    }

    public void Play()
    {
        //If its in Watch mode ... removes it
        if (isWatchModeEnabled)
        {
            ToggleWatchMode();
        }

        //Enables the quests panel
        questsPanel.SetActive(true);
        stashPanel.SetActive(true);
        //gm.GetComponent<QuestsController>().UpdateQuestsBar();

        Time.timeScale = 1.0f;
        SidePanel.SetActive(false);
        canvasBackroundBox.SetActive(false);
        gm.GetComponent<EditorModeController>().isEditorMode = false;
        gm.GetComponent<Zoom>().zoomToPlayMode = true;

        gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(false);

        Time.timeScale = this.GetComponent<TimeSpeedController>().currentTime;

    }

    public void Pause()
    {
        ReturnToMainPanel();

        //Disables the quests panel
        questsPanel.SetActive(false);
        stashPanel.SetActive(false);

        //If its in Watch mode ... removes it
        if (isWatchModeEnabled)
        {
            ToggleWatchMode();
        }

        SidePanel.SetActive(true);
        canvasBackroundBox.SetActive(true);
        Time.timeScale = 0.0f;
        gm.GetComponent<EditorModeController>().isEditorMode = true;
        spawnEventPlayModePanel.SetActive(false);

        gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ReturnToMainPanel()
    {
        mainPanel.SetActive(true);
        messageSequence_Panel.SetActive(false);
        drawTerrainPanel.SetActive(false);
        addElementPanel.SetActive(false);
        addNPC_Panel.SetActive(false);

        SpawnEvent_Panel.SetActive(false);
        gm.GetComponent<MouseInputController>().eventSpawnerArea.SetActive(false);

        inspectorPanel.SetActive(false);
        npcUpdaterPanel.SetActive(false);
        patrolPointInspectorPanel.SetActive(false);
        gm.GetComponent<EditorModeController>().isDrawingTerrain = false;
        gm.GetComponent<EditorModeController>().isPlacingElements = false;
        gm.GetComponent<EditorModeController>().isPlacingNPC = false;
        gm.GetComponent<EditorModeController>().isPlacingPlayer = false;
        gm.GetComponent<EditorModeController>().isSpawningEvent = false;
        gm.GetComponent<EditorModeController>().removeElement = false;
        gm.GetComponent<EditorModeController>().removeTerrain = false;

        gm.GetComponent<EditorModeController>().isInspectingElement = false;

        gm.GetComponent<EditorModeController>().isPlacingNPC = false;
        gm.GetComponent<EditorModeController>().removeNPC = false;
        gm.GetComponent<EditorModeController>().removeNPCButtonImage.color = Color.white;

        NPCBeingUpdated = null;
    }

    public void AnswerQuestionsPanel()
    {
        Time.timeScale = 0.0f;
        AnswerQuestions_Panel.SetActive(true);
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void TogglePlayerMenu()
    {
        if (playerMenu.activeSelf)
        {
            playerMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            playerMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    private void Update()
    {
        /*
        if (InstructionPanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            InstructionPanel.SetActive(false);
            Time.timeScale = 1.0f;
        }
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePlayerMenu();
        }
    }
}