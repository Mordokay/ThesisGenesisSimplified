using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrolMovement : MonoBehaviour {

    public List<Transform> patrolMovementPoints;
    public NavMeshAgent agent;
    public float waitTime;
    GameObject patrolPointHolder;

    public int patrolIndex;

    public GameObject lineGoalFeedback;
    GameObject myLineGoalFeedback;
    GameObject myTalkLine;

    UIManager uiManager;

    public GameObject thinkingBalloon;
    public GameObject currentGoalObject;

    public bool isWaiting;
    public float remainingDistance;

    public bool stopped;
    public float velocity;

    GameObject player;
    public float remainingAtackTime;
    public bool isBeingAtacked;

    public void Start() {
        remainingAtackTime = 0.0f;
        isBeingAtacked = false;

        player = GameObject.FindGameObjectWithTag("Player");

        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        //isWaiting = true;
        stopped = false;

        currentGoalObject = null;

        myLineGoalFeedback = Instantiate(lineGoalFeedback);
        myLineGoalFeedback.GetComponent<PatrolGoalFeedback>().origin = this.transform;

        myTalkLine = Instantiate(lineGoalFeedback);
        myTalkLine.GetComponent<PatrolGoalFeedback>().origin = this.transform;
        myTalkLine.GetComponent<PatrolGoalFeedback>().isTalkArrow = true;

        //agent = GetComponent<NavMeshAgent>();
        //agent.ResetPath();
        waitTime = -1;

        GetNewGoal();
    }

    public void LookAtPatrolPoint()
    {
        this.transform.LookAt(currentGoalObject.transform);
    }

    public void setUpPatrolMovementPoints()
    {
        patrolIndex = 0;
        patrolMovementPoints = new List<Transform>();
        patrolPointHolder = GameObject.FindGameObjectWithTag("PatrolPointsHolder");
        foreach (Transform tr in patrolPointHolder.transform) patrolMovementPoints.Add(tr);
    }

    public void ResetPosition()
    {
        patrolIndex = 0;
        currentGoalObject = patrolMovementPoints[System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex])].gameObject;
        this.transform.position = currentGoalObject.transform.position;
    }

    void GetNewGoal()
    {
        if (this.GetComponentInParent<NPCData>().patrolPointIndex.Count > 0)
        {
            if (patrolIndex == this.GetComponentInParent<NPCData>().patrolPointIndex.Count)
            {
                patrolIndex = 0;
            }
            if (System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex]) >= patrolMovementPoints.Count)
            {
                this.GetComponentInParent<NPCData>().patrolPointIndex.RemoveAt(patrolIndex);
                GetNewGoal();
                return;
            }
            else if (patrolMovementPoints.Count > 0)
            {
                //Debug.Log(patrolMovementPoints[System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex])]);
                if (patrolMovementPoints[System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex])] != null)
                {
                    //agent.destination = patrolMovementPoints[System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex])].position;

                    currentGoalObject = patrolMovementPoints[System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex])].gameObject;
                    this.transform.LookAt(currentGoalObject.transform);
                    //myLineGoalFeedback.GetComponent<PatrolGoalFeedback>().destination =
                    //    patrolMovementPoints[System.Int32.Parse(this.GetComponentInParent<NPCData>().patrolPointIndex[patrolIndex])].position;
                }
                patrolIndex += 1;
            }
        }
        else
        {
            //agent.destination = this.transform.position;
        }
    }

    //When NPC reaches a position he waits for a couple of seconds before starting to move again
    float SetWaitTime()
    {
        if (currentGoalObject == null)
        {
            return 0;
        }

        waitTime = 0;

        foreach (NPCData.Interest interest in GetComponentInParent<NPCData>().interests)
        {
            foreach (Message patrolMessage in currentGoalObject.GetComponent<PatrolPointData>().messages)
            {
                Message.Tag foundTag = patrolMessage.tags.Find(t => t.name == interest.name);
                //If the message from patrol point contains a TAG that is of interest to the player
                if (foundTag != null)
                {
                    /*
                    Interests are normalized between 0 - 1
                    tags on messages from events are not normalized and values are usually high (Ex: Berrries,50  Wood,40  Gathering,10)
                    If, for example, NPC has an interest of 0.6 in Berries and there is an event on patrol point with Berries,40
                    we have (40 * 0.6) / 10 = 2.6 seconds wait time. 
                    */
                    waitTime += (foundTag.weight * interest.weight) / 10.0f;
                }
            }
        }
        return waitTime;
    }

    public void UpdatePatrolPoints()
    {
        patrolMovementPoints.Clear();
        patrolMovementPoints.RemoveAll(item => item == null);
        foreach (Transform tr in patrolPointHolder.transform) patrolMovementPoints.Add(tr);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.gameObject.tag.Equals("NPC"))
        {
            //Debug.Log("Collided with another NPC!!!");
            this.GetComponentInParent<Social>().NPC_Colision(other.transform.parent.gameObject);
        }
    }

    public void AtackedByPlayer()
    {
        //follows the player for 6 seconds
        remainingAtackTime = 6.0f;
        isBeingAtacked = true;

        if (this.transform.parent.gameObject.GetComponent<Social>().isTalking)
        {
            this.transform.parent.gameObject.GetComponent<Social>().InterruptConversation();
        }
        this.GetComponent<WizardController>().sawPlayerCanvas.SetActive(true);
        this.GetComponent<WizardController>().WatchingForPlayerCanvas.SetActive(false);
    }

    void Update() {
        if (this.GetComponentInParent<NPCData>().NPCType == 0)
        {
            if(this.transform.parent.GetComponent<Social>().tattling == true)
            {
                GameObject guardian = this.transform.parent.GetComponent<Social>().tattlingGuadian;
                Message msgToTattle = this.transform.parent.GetComponent<Social>().tattlingMessage;

                float step = velocity * 1.6f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, guardian.transform.GetChild(1).position, step);
                this.transform.LookAt(guardian.transform.GetChild(1));

                //Debug.Log("Distance: " + Vector3.Distance(this.transform.parent.GetComponent<Social>().tattlingGuadian.transform.GetChild(1).position, this.transform.position));

                if (Vector3.Distance(this.transform.parent.GetComponent<Social>().tattlingGuadian.transform.GetChild(1).position, this.transform.position) < 1.0f)
                {
                    //Ends tattling on both sides
                    this.transform.parent.GetComponent<Social>().tattling = false;
                    guardian.GetComponent<Social>().tattling = false;

                    //Interrupts guardian if he is talking with someone else!
                    if (guardian.GetComponent<Social>().isTalking)
                    {
                        guardian.GetComponent<Social>().InterruptConversation();
                    }

                    this.GetComponentInParent<Social>().choosedMessage = this.transform.parent.GetComponent<Social>().tattlingMessage;
                    guardian.GetComponent<Social>().choosedMessage = this.transform.parent.GetComponent<Social>().tattlingMessage;
                    this.GetComponentInParent<Social>().messageOfInterest = true;
                    guardian.GetComponent<Social>().messageOfInterest = true;
                    this.GetComponentInParent<Social>().talkPartner = guardian;
                    guardian.GetComponent<Social>().talkPartner = this.transform.parent.gameObject;
                    this.GetComponentInParent<Social>().isReceivingMessage = false;
                    guardian.GetComponentInParent<Social>().isReceivingMessage = true;

                    this.GetComponentInParent<Social>().remainingMessageTransmissionTime = msgToTattle.messageTransmissionTime;
                    guardian.GetComponentInParent<Social>().remainingMessageTransmissionTime = msgToTattle.messageTransmissionTime;

                    this.GetComponentInParent<Social>().isTalking = true;
                    guardian.GetComponentInParent<Social>().isTalking = true;

                    stopped = true;
                    guardian.GetComponentInChildren<NPCPatrolMovement>().stopped = true;
                }
            }
            else if (!stopped)
            {
                float step = velocity * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, currentGoalObject.transform.position, step);
                this.transform.LookAt(currentGoalObject.transform);

                //transform.Translate(currentGoalObject.transform.position.normalized * Time.deltaTime);
                //this.transform.position += currentGoalObject.transform.position.normalized * Time.deltaTime;
                if (Vector3.Distance(currentGoalObject.transform.position, this.transform.position) < 0.1f)
                {
                    GetNewGoal();
                }
            }
        }
        //The movement of Wizards
        else
        {
            if (isBeingAtacked)
            {
                //remainingAtackTime -= Time.deltaTime;
                if(/*remainingAtackTime < 0 || */
                    Vector3.Distance(this.transform.position, player.transform.position) > this.GetComponent<WizardController>().minimumFollowDistance)
                {
                    remainingAtackTime = 0;
                    isBeingAtacked = false;
                    this.GetComponent<WizardController>().sawPlayerCanvas.SetActive(false);
                    this.transform.LookAt(currentGoalObject.transform);
                }
            }
            if (!stopped)
            {
                if (this.GetComponent<WizardController>().isFollowingPlayer || isBeingAtacked)
                {
                    //float step = (1.1f + (1 - Vector3.Distance(this.transform.position, player.transform.position) / 5.0f)) * Time.deltaTime;
                    float step = velocity * Time.deltaTime;

                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                    this.transform.LookAt(player.transform);
                }
                else if(currentGoalObject != null)
                {
                    float step = velocity * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, currentGoalObject.transform.position, step);

                    //transform.Translate(currentGoalObject.transform.position.normalized * Time.deltaTime);
                    //this.transform.position += currentGoalObject.transform.position.normalized * Time.deltaTime;
                    if (Vector3.Distance(currentGoalObject.transform.position, this.transform.position) < 0.1f)
                    {
                        GetNewGoal();
                    }
                }
            }
        }
        /*
        remainingDistance = Vector3.Distance(currentGoalObject.transform.position, this.transform.position);
        if (uiManager.isGoalFeedbackEnabled)
        {
            if (this.GetComponentInParent<Social>().isTalking)
            {
                myLineGoalFeedback.SetActive(false);
                if (!this.GetComponentInParent<Social>().isReceivingMessage)
                {
                    if (!myTalkLine.activeSelf)
                    {
                        myTalkLine.SetActive(true);
                    }
                    //myTalkLine.GetComponent<PatrolGoalFeedback>().ClearAllArrows();
                    myTalkLine.GetComponent<PatrolGoalFeedback>().destination =
                    this.GetComponentInParent<Social>().talkPartner.transform.GetChild(1).position;
                }
                else
                {
                    myTalkLine.GetComponent<PatrolGoalFeedback>().ClearAllArrows();
                    myTalkLine.SetActive(false);
                }
            }
            else
            {
                if (!myLineGoalFeedback.activeSelf)
                {
                    myLineGoalFeedback.SetActive(true);
                    myLineGoalFeedback.GetComponent<PatrolGoalFeedback>().ClearAllArrows();
                }

                if (myTalkLine.activeSelf)
                {
                    myTalkLine.SetActive(false);
                    myTalkLine.GetComponent<PatrolGoalFeedback>().ClearAllArrows();
                }
            }
        }
        else
        {
            myLineGoalFeedback.SetActive(false);
        }

        if (isWaiting)
        {
            if (!thinkingBalloon.activeSelf)
            {
                //Forces the balloon to move to the NPC Object position. The balloon is updated on NPCFeedbackUpdater script
                thinkingBalloon.transform.localPosition = this.transform.localPosition;

                //Activates thinking ballon 
                thinkingBalloon.SetActive(true);
            }

            waitTime -= Time.deltaTime;
            if(waitTime <= 0)
            {
                waitTime = 0;
                isWaiting = false;
                GetNewGoal();
            }
        }
        else if (Vector3.Distance(currentGoalObject.transform.position, this.transform.position) < 0.5f)
        {
            if (SetWaitTime() != 0)
            {
                isWaiting = true;
                //Forces the balloon to move to the NPC Object position. The balloon is updated on NPCFeedbackUpdater script
                thinkingBalloon.transform.localPosition = this.transform.localPosition;

                //Activates thinking ballon 
                thinkingBalloon.SetActive(true);
            }
            else
            {
                GetNewGoal();
                thinkingBalloon.SetActive(false);
            }
        }
        else
        {
            thinkingBalloon.SetActive(false);
        }
        */
    }
}
