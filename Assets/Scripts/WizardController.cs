using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour {

    GameObject player;

    public float minimumFollowDistance;
    public bool wantsToFollowPlayer;
    public bool isFollowingPlayer;
    string myInterest;
    public GameObject sawPlayerCanvas;
    public GameObject WatchingForPlayerCanvas;

    GameObject canvas;
    GameObject gm;

    GameObject prisonGuard;

    void Start()
    {
        sawPlayerCanvas = this.transform.parent.transform.GetChild(6).gameObject;
        WatchingForPlayerCanvas = this.transform.parent.transform.GetChild(7).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        prisonGuard = GameObject.FindGameObjectWithTag("PrisonGuard");

        gm = GameObject.Find("GameManager");
        myInterest = this.GetComponentInParent<NPCData>().interests[0].name;
        InvokeRepeating("CheckInterests", 0.0f, 0.1f);
    }

    public void UpdateDifficulty()
    {
        minimumFollowDistance = 2.5f + ((gm.GetComponent<QuestsController>().totalGoldenObjectsGathered + gm.GetComponent<QuestsController>().playerStash) / 12.0f) * 2.0f;
        this.GetComponent<NPCPatrolMovement>().velocity = 1.3f + ((gm.GetComponent<QuestsController>().totalGoldenObjectsGathered + gm.GetComponent<QuestsController>().playerStash) / 12.0f) * 1.3f;
    }

    void CheckInterests()
    {
        if (!wantsToFollowPlayer)
        {
            foreach(Message m in this.GetComponentInParent<NPCData>().messages)
            {
                //Message.Tag t = m.tags.Find(x => x.name == this.GetComponentInParent<NPCData>().interests[0].name);
                if (/*t != null &&*/ m.description.Contains("Golden") && m.id != -99 && m.id != -999)
                {
                    wantsToFollowPlayer = true;
                    WatchingForPlayerCanvas.SetActive(true);
                    //Debug.Log("Wants to follow player!!!");
                    break;
                }
            }
        }
    }

    void Update () {
        if (sawPlayerCanvas.activeSelf)
        {
            sawPlayerCanvas.transform.position = this.transform.position;
        }
        else if(WatchingForPlayerCanvas.activeSelf)
        {
            WatchingForPlayerCanvas.transform.position = this.transform.position;
        }
        if (wantsToFollowPlayer)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) < minimumFollowDistance)
            {
                if (!isFollowingPlayer)
                {
                    isFollowingPlayer = true;
                    sawPlayerCanvas.SetActive(true);
                    WatchingForPlayerCanvas.SetActive(false);
                    Debug.Log("I started following the player!!!");
                }
            }
            else if (this.GetComponentInParent<Social>().isTalking)
            {
                WatchingForPlayerCanvas.SetActive(false);
            }
            else
            {
                if (isFollowingPlayer || !WatchingForPlayerCanvas.activeSelf)
                {

                    isFollowingPlayer = false;
                    sawPlayerCanvas.SetActive(false);
                    WatchingForPlayerCanvas.SetActive(true);

                    //If player manages to get away from wizard, the wizards stops being threatened
                    this.GetComponent<NPCPatrolMovement>().isBeingAtacked = false;
                    this.GetComponent<NPCPatrolMovement>().remainingAtackTime = 0.0f;

                    this.transform.LookAt(this.GetComponent<NPCPatrolMovement>().currentGoalObject.transform);
                    //Debug.Log("I stopped following the player!!!");
                }
            }
            
        }
	}

    public void UpdateColor(Color c)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player") && (wantsToFollowPlayer || this.GetComponent<NPCPatrolMovement>().isBeingAtacked))
        {
            //Has to drop items on ground
            gm.GetComponent<QuestsController>().DropStash();

            prisonGuard.GetComponent<PrisonGuardControler>().ShowMessage();
            player.transform.position = new Vector3(-13.0f, 0.0f, 17.0f);

            this.GetComponent<NPCPatrolMovement>().ResetPosition();

            gm.GetComponent<QuestsController>().UpdateDificultyAllGuardians();

            StartCoroutine(gm.GetComponent<MySQLManager>().LogEventAtTime("Killed by " + this.transform.parent.name));
            //Time.timeScale = 0.0f;
            //canvas.transform.GetChild(0).gameObject.SetActive(true);
            //StartCoroutine(gm.GetComponent<MySQLManager>().SendsDataToDatabase());
        }
    }
}
