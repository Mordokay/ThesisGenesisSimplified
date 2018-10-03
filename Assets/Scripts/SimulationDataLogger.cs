using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SimulationDataLogger : MonoBehaviour {

    public bool isWritingStuff;
    StreamWriter writer;
    //StreamWriter writerLocal;

    public int[] messageCounter;

    int[] messageForestCurrent;
    int[] messageForestPrevious;
    int[] messageSnowCurrent;
    int[] messageSnowPrevious;
    int[] messageDesertCurrent;
    int[] messageDesertPrevious;
    int[] messageIslandCurrent;
    int[] messageIslandPrevious;
    int[] messageRoadsCurrent;
    int[] messageRoadsPrevious;

    int[] aliveIDs;
    int[] existsIDs;
    int[] removedCount;

    int[] graphPointsMessagePrevious;
    int[] graphPointsMessageCurrent;

    int[] graphPointsAlivePrevious;
    int[] graphPointsExistsPrevious;

    public int repeatedMessageCount;

    int removedTotalMessages;
    public Color[] GraphColors;
    public GameObject GraphPoint;
    public GameObject GraphLine;
    public GameObject GraphHolderMessage;
    public GameObject GraphHolderAlive;
    public GameObject GraphHolderExists;
    public GameObject GraphHolderForest;
    public GameObject GraphHolderSnow;
    public GameObject GraphHolderDesert;
    public GameObject GraphHolderIsland;
    public GameObject GraphHolderRoads;
    public GameObject GraphHolderResumed;

    public int resWidth = 5760;
    public int resHeight = 3240;
    string nameForSave;
    public Camera graphCameraMessage;
    public Camera graphCameraAlive;
    public Camera graphCameraExists;
    public Camera graphCameraForestVilage;
    public Camera graphCameraSnowVilage;
    public Camera graphCameraDesertVilage;
    public Camera graphCameraIslandVilage;
    public Camera graphCameraRoads;
    public Camera graphCameraResumed;
    public GameObject[] graphLabelsMessage;
    public GameObject[] graphLabelsAlive;
    public GameObject[] graphLabelsExists;
    public GameObject[] graphLabelsForestVilage;
    public GameObject[] graphLabelsSnowVilage;
    public GameObject[] graphLabelsDesertVilage;
    public GameObject[] graphLabelsIslandVilage;
    public GameObject[] graphLabelsRoads;

    public float ForestVilageMinX;
    public float ForestVilageMaxX;
    public float ForestVilageMinZ;
    public float ForestVilageMaxZ;

    public float SnowVilageMinX;
    public float SnowVilageMaxX;
    public float SnowVilageMinZ;
    public float SnowVilageMaxZ;

    public float DesertVilageMinX;
    public float DesertVilageMaxX;
    public float DesertVilageMinZ;
    public float DesertVilageMaxZ;

    public float IslandVilageMinX;
    public float IslandVilageMaxX;
    public float IslandVilageMinZ;
    public float IslandVilageMaxZ;

    int postDataSubmission;

    public int totalMessages;
    void Start () {

        repeatedMessageCount = 0;
        totalMessages = 0;

        /*
        postDataSubmission = 0;
        //Draws a point every 10 seconds on the graphs
        InvokeRepeating("AddPoints", 0.0f, 10.0f);

        //graphCameraMessage.gameObject.SetActive(false);
        //graphCameraAlive.gameObject.SetActive(false);
        //graphCameraExists.gameObject.SetActive(false);

        nameForSave = "";
        
        removedTotalMessages = 0;

        repeatedMessageCount = 0;

        messageCounter = new int[20];
        foreach(int i in messageCounter)
        {
            messageCounter[i] = 0;
        }

        messageForestCurrent = new int[20];
        foreach (int i in messageForestCurrent)
        {
            messageForestCurrent[i] = 0;
        }
        messageForestPrevious = new int[20];
        foreach (int i in messageForestPrevious)
        {
            messageForestPrevious[i] = 0;
        }
       
        messageSnowCurrent = new int[20];
        foreach (int i in messageSnowCurrent)
        {
            messageSnowCurrent[i] = 0;
        }
        messageSnowPrevious = new int[20];
        foreach (int i in messageSnowPrevious)
        {
            messageSnowPrevious[i] = 0;
        }

        messageDesertCurrent = new int[20];
        foreach (int i in messageDesertCurrent)
        {
            messageDesertCurrent[i] = 0;
        }
        messageDesertPrevious = new int[20];
        foreach (int i in messageDesertPrevious)
        {
            messageDesertPrevious[i] = 0;
        }

        messageIslandCurrent = new int[20];
        foreach (int i in messageIslandCurrent)
        {
            messageIslandCurrent[i] = 0;
        }
        messageIslandPrevious = new int[20];
        foreach (int i in messageIslandPrevious)
        {
            messageIslandPrevious[i] = 0;
        }

        messageRoadsCurrent = new int[20];
        foreach (int i in messageRoadsCurrent)
        {
            messageRoadsCurrent[i] = 0;
        }
        messageRoadsPrevious = new int[20];
        foreach (int i in messageRoadsPrevious)
        {
            messageRoadsPrevious[i] = 0;
        }

        aliveIDs = new int[20];
        foreach (int i in aliveIDs)
        {
            aliveIDs[i] = 0;
        }
        existsIDs = new int[20];
        foreach (int i in existsIDs)
        {
            existsIDs[i] = 0;
        }
        removedCount = new int[20];
        foreach (int i in removedCount)
        {
            removedCount[i] = 0;
        }
        graphPointsMessagePrevious = new int[20];
        foreach (int i in graphPointsMessagePrevious)
        {
            graphPointsMessagePrevious[i] = 0;
        }
        graphPointsMessageCurrent = new int[20];
        foreach (int i in graphPointsMessageCurrent)
        {
            graphPointsMessageCurrent[i] = 0;
        }

        graphPointsAlivePrevious = new int[20];
        foreach (int i in graphPointsAlivePrevious)
        {
            graphPointsAlivePrevious[i] = 0;
        }
        graphPointsExistsPrevious = new int[20];
        foreach (int i in graphPointsExistsPrevious)
        {
            graphPointsExistsPrevious[i] = 0;
        }

        if (!PlayerPrefs.GetString("mapToLoad").Equals("default")){
            isWritingStuff = true;

            string cenarioName = PlayerPrefs.GetString("mapToLoad");
            string timestamp = System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year + "___" +
                System.DateTime.Now.TimeOfDay.Hours + "-" + System.DateTime.Now.TimeOfDay.Minutes + "-" + System.DateTime.Now.TimeOfDay.Seconds;
            nameForSave = cenarioName + "___" + timestamp;

            string path = Application.persistentDataPath + "/CenarioTests/" + nameForSave + ".txt";
            string localPath = "Assets/CenarioTests/" + nameForSave + ".txt";

            //Write some text to the test.txt file
            writer = new StreamWriter(path, false);
            //writerLocal = new StreamWriter(localPath, false);

            //Debug.Log(nameForSave);
        }
        else
        {
            isWritingStuff = false;
        }
        */
    }

    public string getCurrentTime()
    {
        string minutesText = "";
        string secondsText = "";

        if (Time.timeSinceLevelLoad / 60 < 10)
        {
            minutesText = "0" + Mathf.FloorToInt(Time.timeSinceLevelLoad / 60).ToString();
        }
        else
        {
            minutesText = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60).ToString();
        }

        if (Time.timeSinceLevelLoad % 60 < 10)
        {
            secondsText = "0" + Mathf.FloorToInt(Time.timeSinceLevelLoad % 60).ToString();
        }
        else
        {
            secondsText = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60).ToString();
        }

        string SimulationTime = "[" + minutesText + ":" + secondsText + "]  ";

        return SimulationTime;
    }

    public void WriteMessageToLog(string line, int id, bool wasRepeated, Vector3 position)
    {

        totalMessages += 1;
        if (wasRepeated)
        {
            repeatedMessageCount += 1;
        }
        /*
        if (id != -99)
        {
            //Debug.Log("position message" + position);
            if (isWritingStuff)
            {
                if (id < 20)
                {
                    graphPointsMessageCurrent[id] += 1;

                    //check inside forest zone
                    if (position.x > ForestVilageMinX && position.x < ForestVilageMaxX && position.z > ForestVilageMinZ && position.z < ForestVilageMaxZ)
                    {
                        messageForestCurrent[id] += 1;
                    }
                    //check inside snow zone
                    else if (position.x > SnowVilageMinX && position.x < SnowVilageMaxX && position.z > SnowVilageMinZ && position.z < SnowVilageMaxZ)
                    {
                        messageSnowCurrent[id] += 1;
                    }
                    //check inside desert zone
                    else if (position.x > DesertVilageMinX && position.x < DesertVilageMaxX && position.z > DesertVilageMinZ && position.z < DesertVilageMaxZ)
                    {
                        messageDesertCurrent[id] += 1;
                    }
                    //check inside island zone
                    else if (position.x > IslandVilageMinX && position.x < IslandVilageMaxX && position.z > IslandVilageMinZ && position.z < IslandVilageMaxZ)
                    {
                        messageIslandCurrent[id] += 1;
                    }
                    //of nothing else applies the message is on the roads
                    else
                    {
                        messageRoadsCurrent[id] += 1;
                    }
                }

                messageCounter[id] += 1;
                if (wasRepeated)
                {
                    repeatedMessageCount += 1;
                }

                string currentTime = getCurrentTime();

                writer.WriteLine(currentTime + line);
                //writerLocal.WriteLine(currentTime + line);
            }
        }
        */
    }

    public void WriteTextToLog(string line)
    {
        /*
        if (isWritingStuff)
        {
            string currentTime = getCurrentTime();

            writer.WriteLine(currentTime + line);
           // writerLocal.WriteLine(currentTime + line);
        }
        */
    }

    public void WriteRemoveToLog(string line, int id)
    {
        /*
        if (isWritingStuff)
        {
            removedTotalMessages += 1;
            removedCount[id] += 1;

            string currentTime = getCurrentTime();
            writer.WriteLine(currentTime + " " + line + System.Environment.NewLine);
        }
        */
    }

    public void AddPoints()
    {
        //Draws a point every 10 seconds
        int currentTimePoint = Mathf.FloorToInt(Time.timeSinceLevelLoad / 10);

        foreach (Transform npc in this.GetComponent<EditorModeController>().npcHolder.transform)
        {
            foreach (Message m in npc.gameObject.GetComponent<NPCData>().messages)
            {
                if (m.id < 20 && m.id != -99)
                {
                    if (m.messageDecayment > 0.0f)
                    {
                        aliveIDs[m.id] += 1;
                    }
                    existsIDs[m.id] += 1;
                }
            }
        }

        for (int i = 0; i < this.GetComponent<PlayModeManager>().messageID && i < 20; i++)
        {
            graphLabelsMessage[i].SetActive(true);
            graphLabelsAlive[i].SetActive(true);
            graphLabelsExists[i].SetActive(true);
            graphLabelsForestVilage[i].SetActive(true);
            graphLabelsSnowVilage[i].SetActive(true);
            graphLabelsDesertVilage[i].SetActive(true);
            graphLabelsIslandVilage[i].SetActive(true);
            graphLabelsRoads[i].SetActive(true);

            //DrawCurrentPoint and set color for Messages
            GameObject myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, 500.0f + graphPointsMessageCurrent[i]);
            myPoint.transform.parent = GraphHolderMessage.transform;

            //DrawCurrentPoint and set color for Alive
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, aliveIDs[i]);
            myPoint.transform.parent = GraphHolderAlive.transform;

            //DrawCurrentPoint and set color for Exists
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, -500.0f + existsIDs[i]);
            myPoint.transform.parent = GraphHolderExists.transform;

            //DrawCurrentPoint and set color for Forest
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, -999.5f + messageForestCurrent[i]);
            myPoint.transform.parent = GraphHolderForest.transform;

            //DrawCurrentPoint and set color for Snow
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, -2000.0f + messageSnowCurrent[i]);
            myPoint.transform.parent = GraphHolderSnow.transform;

            //DrawCurrentPoint and set color for Desert
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, -1500.0f + messageDesertCurrent[i]);
            myPoint.transform.parent = GraphHolderDesert.transform;

            //DrawCurrentPoint and set color for Island
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, -2500.0f + messageIslandCurrent[i]);
            myPoint.transform.parent = GraphHolderIsland.transform;

            //DrawCurrentPoint and set color for Roads
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(500.0f + currentTimePoint, 0.0f, -3000.0f + messageRoadsCurrent[i]);
            myPoint.transform.parent = GraphHolderRoads.transform;

            //RESUMED GRAPH -->>> Go on Z axis 8.0f for each graph
            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(1000.0f + currentTimePoint, 0.0f, 40.0f + graphPointsMessageCurrent[i] / 6.0f);
            myPoint.transform.parent = GraphHolderResumed.transform;

            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(1000.0f + currentTimePoint, 0.0f, 32.0f + messageForestCurrent[i] / 6.0f);
            myPoint.transform.parent = GraphHolderResumed.transform;

            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(1000.0f + currentTimePoint, 0.0f, 24.0f + messageSnowCurrent[i] / 6.0f);
            myPoint.transform.parent = GraphHolderResumed.transform;

            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(1000.0f + currentTimePoint, 0.0f, 16.0f + messageDesertCurrent[i] / 6.0f);
            myPoint.transform.parent = GraphHolderResumed.transform;

            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(1000.0f + currentTimePoint, 0.0f, 8.0f + messageIslandCurrent[i] / 6.0f);
            myPoint.transform.parent = GraphHolderResumed.transform;

            myPoint = Instantiate(GraphPoint) as GameObject;
            myPoint.GetComponent<SpriteRenderer>().color = GraphColors[i];
            myPoint.transform.position = new Vector3(1000.0f + currentTimePoint, 0.0f, messageRoadsCurrent[i] / 6.0f);
            myPoint.transform.parent = GraphHolderResumed.transform;

            //Draw a Line to previous point and color of line in Messages
            GameObject myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, 500.0f + graphPointsMessagePrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, 500.0f + graphPointsMessageCurrent[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderMessage.transform;

            //Draw a Line to previous point and color of line in Alive
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, graphPointsAlivePrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, aliveIDs[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderAlive.transform;

            //Draw a Line to previous point and color of line in Exists
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, -500.0f + graphPointsExistsPrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, -500.0f + existsIDs[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderExists.transform;

            //Draw a Line to previous point and color of line in Forest
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, -999.5f + messageForestPrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, -999.5f + messageForestCurrent[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderForest.transform;

            //Draw a Line to previous point and color of line in Snow
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, -2000.0f + messageSnowPrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, -2000.0f + messageSnowCurrent[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderSnow.transform;

            //Draw a Line to previous point and color of line in Desert
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, -1500.0f + messageDesertPrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, -1500.0f + messageDesertCurrent[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderDesert.transform;

            //Draw a Line to previous point and color of line in island
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, -2500.0f + messageIslandPrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, -2500.0f + messageIslandCurrent[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderIsland.transform;

            //Draw a Line to previous point and color of line in Roads
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(500.0f + currentTimePoint - 1, 0.0f, -3000.0f + messageRoadsPrevious[i]));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(500.0f + currentTimePoint, 0.0f, -3000.0f + messageRoadsCurrent[i]));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderRoads.transform;

            //////////////////////////////////////////////////////////////
            ///////Draw a Line to previous point  on RESUMED GRAPH////////
            //////////////////////////////////////////////////////////////
            
            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(1000.0f + currentTimePoint - 1, 0.0f, 40.0f + graphPointsMessagePrevious[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(1000.0f + currentTimePoint, 0.0f, 40.0f + graphPointsMessageCurrent[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderResumed.transform;

            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(1000.0f + currentTimePoint - 1, 0.0f, 32.0f + messageForestPrevious[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(1000.0f + currentTimePoint, 0.0f, 32.0f + messageForestCurrent[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderResumed.transform;

            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(1000.0f + currentTimePoint - 1, 0.0f, 24.0f + messageSnowPrevious[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(1000.0f + currentTimePoint, 0.0f, 24.0f + messageSnowCurrent[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderResumed.transform;

            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(1000.0f + currentTimePoint - 1, 0.0f, 16.0f + messageDesertPrevious[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(1000.0f + currentTimePoint, 0.0f, 16.0f + messageDesertCurrent[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderResumed.transform;

            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(1000.0f + currentTimePoint - 1, 0.0f, 8.0f + messageIslandPrevious[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(1000.0f + currentTimePoint, 0.0f, 8.0f + messageIslandCurrent[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderResumed.transform;

            myLine = Instantiate(GraphLine);
            myLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(1000.0f + currentTimePoint - 1, 0.0f, messageRoadsPrevious[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(1000.0f + currentTimePoint, 0.0f, messageRoadsCurrent[i] / 6.0f));
            myLine.GetComponent<LineRenderer>().startColor = GraphColors[i];
            myLine.GetComponent<LineRenderer>().endColor = GraphColors[i];
            myLine.transform.parent = GraphHolderResumed.transform;

            ////////////////////////////////////////////////////////////////////////////

            graphPointsMessagePrevious[i] = graphPointsMessageCurrent[i];
            graphPointsMessageCurrent[i] = 0;

            graphPointsAlivePrevious[i] = aliveIDs[i];
            aliveIDs[i] = 0;

            graphPointsExistsPrevious[i] = existsIDs[i];
            existsIDs[i] = 0;

            messageForestPrevious[i] = messageForestCurrent[i];
            messageForestCurrent[i] = 0;

            messageSnowPrevious[i] = messageSnowCurrent[i];
            messageSnowCurrent[i] = 0;

            messageDesertPrevious[i] = messageDesertCurrent[i];
            messageDesertCurrent[i] = 0;

            messageIslandPrevious[i] = messageIslandCurrent[i];
            messageIslandCurrent[i] = 0;

            messageRoadsPrevious[i] = messageRoadsCurrent[i];
            messageRoadsCurrent[i] = 0;
        }
    }

    public void CloseLogger()
    {
        /*
        if (isWritingStuff)
        {
            ///////////////////////////////////////////////////////////////////////
            /////////////////////////PNG GRAPH LOGGER//////////////////////////////
            ///////////////////////////////////////////////////////////////////////

            string imageName = nameForSave + "_" + resWidth + "x" + resHeight;
            string pathMessage = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Message.png";
           // string localPathMessage = "Assets/CenarioTests/" + imageName + "_Message.png";

            string pathAlive = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Alive.png";
            //string localPathAlive = "Assets/CenarioTests/" + imageName + "_Alive.png";

            string pathExists = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Exists.png";
            //string localPathExists = "Assets/CenarioTests/" + imageName + "_Exists.png";

            string pathForest = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Forest.png";

            string pathSnow = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Snow.png";

            string pathDesert = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Desert.png";

            string pathIsland = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Island.png";

            string pathRoads = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Roads.png";

            string pathResumed = Application.persistentDataPath + "/CenarioTests/" + imageName + "_Resumed.png";

            //MESSAGE GRAPH
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraMessage.targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraMessage.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraMessage.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathMessage, bytes);
            //System.IO.File.WriteAllBytes(localPathMessage, bytes);

            //ALIVE GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraAlive.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraAlive.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraAlive.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathAlive, bytes);
           // System.IO.File.WriteAllBytes(localPathAlive, bytes);

            //EXISTS GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraExists.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraExists.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraExists.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathExists, bytes);
            //System.IO.File.WriteAllBytes(localPathExists, bytes);

            //FOREST GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraForestVilage.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraForestVilage.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraForestVilage.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathForest, bytes);

            //SNOW GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraSnowVilage.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraSnowVilage.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraSnowVilage.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathSnow, bytes);

            //DESERT GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraDesertVilage.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraDesertVilage.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraDesertVilage.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathDesert, bytes);

            //ISLAND GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraIslandVilage.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraIslandVilage.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraIslandVilage.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathIsland, bytes);

            //ROADS GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraRoads.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraRoads.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraRoads.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathRoads, bytes);

            //RESUMED GRAPH
            rt = new RenderTexture(resWidth, resHeight, 24);
            graphCameraResumed.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            graphCameraResumed.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            graphCameraResumed.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            bytes = screenShot.EncodeToPNG();

            System.IO.File.WriteAllBytes(pathResumed, bytes);

            ///////////////////////////////////////////////////////////////////////
            /////////////////////////TEXT FILE LOGGER//////////////////////////////
            ///////////////////////////////////////////////////////////////////////
            int messagesTotalCount = 0;

            writer.WriteLine(System.Environment.NewLine);

            for (int i = 0; i < this.GetComponent<PlayModeManager>().messageID; i++)
            {
                writer.WriteLine("Message ID = " + i + " was removed " + removedCount[i] + " times");
            }

            writer.WriteLine("Removed " + removedTotalMessages + " total messages");

            writer.WriteLine(System.Environment.NewLine);

            for (int i = 0; i < this.GetComponent<PlayModeManager>().messageID; i++)
            {
                writer.WriteLine("Message ID = " + i + " Count = " + messageCounter[i]);
                messagesTotalCount += messageCounter[i];
            }

            writer.WriteLine(System.Environment.NewLine);

            writer.WriteLine("Total Messages = " + messagesTotalCount + System.Environment.NewLine);

            writer.WriteLine("Repeated Messages = " + repeatedMessageCount + System.Environment.NewLine);

            writer.WriteLine("New Messages = " + (messagesTotalCount - repeatedMessageCount) + System.Environment.NewLine);

            writer.WriteLine("Total Simulation Time: " + getCurrentTime() + System.Environment.NewLine);

            writer.WriteLine(System.Environment.NewLine);

            List<GameObject> myNPCs = new List<GameObject>();
            foreach (Transform npc in this.GetComponent<EditorModeController>().npcHolder.transform)
            {
                myNPCs.Add(npc.gameObject);
            }

            for (int i = 0; i < myNPCs.Count; i++)
            {
                foreach (Message m in myNPCs[i].GetComponent<NPCData>().messages)
                {
                    if (m.id != -99) {
                        if (m.messageDecayment > 0.0f)
                        {
                            aliveIDs[m.id] += 1;
                        }

                        existsIDs[m.id] += 1;
                    }
                }
            }

            writer.WriteLine("There are  " + myNPCs.Count + " NPCs");

            for (int i = 0; i < this.GetComponent<PlayModeManager>().messageID; i++)
            {
                writer.WriteLine("Message With ID = " + i + " Is alive in " + aliveIDs[i] + " NPCs and is present in " + existsIDs[i] + " NPCs");
                messagesTotalCount += messageCounter[i];
            }

            writer.Close();
        }
        */
    }
    
    
    /*
    void OnApplicationQuit()
    {
        PlayerPrefs.SetString("mapToLoad", "default");

        //Uploads final data info to database
        SendsDataToDatabase();

        //Temporarily disable drawing of graphs
        //CloseLogger();
    }
    */
}
