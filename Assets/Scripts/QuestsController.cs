using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsController : MonoBehaviour {

    public int totalGoldenTreeGathered;
    public int totalGoldenRockGathered;
    public int totalGoldenBerriesGathered;
    public int totalGoldenCactusGathered;

    public int playerStash;
    public int totalGoldenObjectsGathered;
    public Slider progressBar;
    public Text progressBarText;
    GameObject canvas;

    public GameObject[] StashImages;
    bool sentData;
    public GameObject npcHolder;
    public GameObject player;
    public int[] stashTypes;
    public GameObject[] dropableItems;

    void Start () {
        sentData = false;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        player = GameObject.FindGameObjectWithTag("Player");
        stashTypes = new int[4];

        totalGoldenObjectsGathered = 0;
        UpdateQuestsBar();
        playerStash = 0;
        totalGoldenTreeGathered = 0;
        totalGoldenRockGathered = 0;
        totalGoldenBerriesGathered = 0;
        totalGoldenCactusGathered = 0;
    }

    public void UpdateQuestsBar()
    {
        //This prevents the player from droping any item on the temple before he goes to tutorial 10
        if(this.GetComponent<TutorialController>().tutorialStage < 11)
        {
            return;
        }
        if(playerStash > 0)
        {
            totalGoldenObjectsGathered += playerStash;
            playerStash = 0;
            if(this.GetComponent<TutorialController>().tutorialStage == 11)
            {
                this.GetComponent<TutorialController>().NextTutorial();
            }
            UpdateDificultyAllGuardians();
        }
        progressBar.value = totalGoldenObjectsGathered / 12.0f;
        progressBarText.text = totalGoldenObjectsGathered + "/12";

        foreach(GameObject imageStash in StashImages)
        {
            imageStash.SetActive(false);
        }
    }

    public void DropStash()
    {
        for (int i = 0; i < playerStash; i++)
        {
            StashImages[i].SetActive(false);
            foreach (Transform child in StashImages[i].transform)
            {
                child.gameObject.SetActive(false);
            }

            GameObject myObj = Instantiate(dropableItems[stashTypes[i]]) as GameObject;
            switch (i)
            {
                case 0:
                    myObj.transform.position = player.transform.position + Vector3.left * 0.2f;
                    break;
                case 1:
                    myObj.transform.position = player.transform.position + Vector3.right * 0.2f;
                    break;
                case 2:
                    myObj.transform.position = player.transform.position + Vector3.forward * 0.2f;
                    break;
                case 3:
                    myObj.transform.position = player.transform.position + Vector3.back * 0.2f;
                    break;
            }
            stashTypes[i] = 0;
        }
        playerStash = 0;
    }

    public void IncrementStash(int type)
    {
        StashImages[playerStash].SetActive(true);
        foreach(Transform child in StashImages[playerStash].transform)
        {
            child.gameObject.SetActive(false);
        }
        StashImages[playerStash].transform.GetChild(type).gameObject.SetActive(true);
        stashTypes[playerStash] = type;

        playerStash += 1;

        UpdateDificultyAllGuardians();

        switch (type)
        {
            case 0:
                totalGoldenTreeGathered += 1;
                //sends grabing data to database
                StartCoroutine(this.GetComponent<MySQLManager>().LogEventAtTime("tree"));
                break;
            case 1:
                totalGoldenRockGathered += 1;
                //sends grabing data to database
                StartCoroutine(this.GetComponent<MySQLManager>().LogEventAtTime("rock"));
                break;
            case 2:
                totalGoldenBerriesGathered += 1;
                //sends grabing data to database
                StartCoroutine(this.GetComponent<MySQLManager>().LogEventAtTime("berries"));
                break;
            case 3:
                totalGoldenCactusGathered += 1;
                //sends grabing data to database
                StartCoroutine(this.GetComponent<MySQLManager>().LogEventAtTime("cactus"));
                break;
        }
    }

    public void UpdateDificultyAllGuardians()
    {
        foreach (Transform npc in npcHolder.transform)
        {
            WizardController wc = npc.gameObject.GetComponentInChildren<WizardController>();
            if (wc != null)
            {
                NPCPatrolMovement pm = npc.gameObject.GetComponentInChildren<NPCPatrolMovement>();
                wc.UpdateDifficulty();
            }
        }
    }

    void Update () {
	    if(totalGoldenObjectsGathered >= 12 && !sentData)
        {
            //Player wins the game and shows panel!!!
            Time.timeScale = 0.0f;
            canvas.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(this.GetComponent<MySQLManager>().SendsDataToDatabase());
            sentData = true;
        }
	}
}