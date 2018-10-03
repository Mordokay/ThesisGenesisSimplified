using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySQLManager : MonoBehaviour
{
    public Text minutesText;
    public Text secondsText;

    //Doesn't receive anything
    string createPlayerID_GameMode = "http://web.ist.utl.pt/ist165821/CreatePlayerID_GameMode.php";
    //Receives <id, mode>
    string addPlayerID_GameMode = "http://web.ist.utl.pt/ist165821/AddPlayerID_GameMode.php";
    //Receives <TableName>.  TableName = "Thesis_PlayerID_Mode"
    string createPlayerDataTable = "http://web.ist.utl.pt/ist165821/CreatePlayerDataTable.php";
    //Receives <TableName, TypeOfData, Value1, Value2>
    string addPlayerData = "http://web.ist.utl.pt/ist165821/AddPlayerData.php";
    //Doesn't receive anything
    string getBestMode = "http://web.ist.utl.pt/ist165821/GetBestMode.php";
    //Doesn't receive anything
    string getPlayerID = "http://web.ist.utl.pt/ist165821/GetPlayerID.php";

    string playerID = "";

    public IEnumerator RecordData(int[] questionsAnswers)
    {
        yield return StartCoroutine(this.GetComponent<MySQLManager>().SendsQuestionsToDatabase(questionsAnswers));
    }

    public IEnumerator RecordPersonalData(string questionsAnswers)
    {
        yield return StartCoroutine(this.GetComponent<MySQLManager>().SendsPersonalQuestionsToDatabase(questionsAnswers));
    }

    public string playerMode = "";
    string playerTableName = "";

    GameObject player;
    float timeSinceLastPositionUpdate = 999.0f;

    public Text playerID1;
    public Text playerID2;
    public Text playerIDMenu;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SetUp());
    }

    IEnumerator SetUp()
    {
        yield return StartCoroutine(GetBestMode());
        yield return StartCoroutine(GetPlayerID());
        Debug.Log("PlayerID: " + playerID + " playerMode: " + playerMode);

        playerID1.text = "PlayerID: " + playerID;
        playerID2.text = "PlayerID: " + playerID;
        playerIDMenu.text = "PlayerID:" + System.Environment.NewLine + playerID;

        yield return StartCoroutine(AddPlayerID_GameMode(playerID, playerMode));
        playerTableName = "Thesis_" + playerID + "_" + playerMode;
        yield return StartCoroutine(CreatePlayerDataTable(playerTableName));
    }

    IEnumerator InsertPlayerPos()
    {
        yield return StartCoroutine(AddPlayerData("p", player.transform.position.x, player.transform.position.z));
    }

    IEnumerator InsertTime()
    {
        yield return StartCoroutine(AddPlayerData("t", float.Parse(minutesText.text), float.Parse(secondsText.text)));
    }

    IEnumerator InsertData(string type, float value1, float value2)
    {
        yield return StartCoroutine(AddPlayerData(WWW.EscapeURL(type), value1, value2));
    }

    public IEnumerator LogEventAtTime(string description)
    {
        yield return StartCoroutine(AddPlayerData(description, float.Parse(minutesText.text), float.Parse(secondsText.text)));
    }

    IEnumerator CreatePlayerID_GameMode()
    {
        string post_url = createPlayerID_GameMode;
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        Debug.Log(hs_post.text);
    }

    IEnumerator AddPlayerID_GameMode(string id, string mode)
    {
        string post_url = addPlayerID_GameMode + "?id=" + id + "&mode=" + mode;
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        Debug.Log(hs_post.text);
    }

    IEnumerator CreatePlayerDataTable(string tableName)
    {
        string post_url = createPlayerDataTable + "?tableName=" + WWW.EscapeURL(tableName);
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        Debug.Log(hs_post.text);
    }

    //WWW.EscapeURL(tableName)

    public IEnumerator AddPlayerData(string dataType, float value1, float value2)
    {
        string post_url = addPlayerData + "?tableName=" + WWW.EscapeURL(playerTableName) + "&t=" + dataType + "&v1=" + value1 + "&v2=" + value2;
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        //Debug.Log(hs_post.text);
    }

    IEnumerator GetBestMode()
    {
        string post_url = getBestMode;
        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        playerMode = hs_post.text;
        //Debug.Log(hs_post.text);
    }

    IEnumerator GetPlayerID()
    {
        string post_url = getPlayerID;
        WWW hs_post = new WWW(post_url);
        yield return hs_post;

        playerID = hs_post.text;
        //Debug.Log(hs_post.text);
    }

    public IEnumerator SendsDataToDatabase()
    {
        //int messagesTotalCount = this.GetComponent<SimulationDataLogger>().totalMessages;
        //int repeatedMessageCount = this.GetComponent<SimulationDataLogger>().repeatedMessageCount;

        /*
        for (int i = 0; i < this.GetComponent<PlayModeManager>().messageID; i++)
        {
            //StartCoroutine(InsertData("msg_" + i + "_talked", this.GetComponent<SimulationDataLogger>().messageCounter[i], 0));
            messagesTotalCount += this.GetComponent<SimulationDataLogger>().messageCounter[i];
        }
        */

        //yield return StartCoroutine(InsertData("totalMsgCount", messagesTotalCount, 0));
        //yield return StartCoroutine(InsertData("repeatedMsgCount", repeatedMessageCount, 0));
        //yield return StartCoroutine(InsertData("newMsgCount", (messagesTotalCount - repeatedMessageCount), 0));
        yield return StartCoroutine(InsertTime());
    }

    public IEnumerator SendsQuestionsToDatabase(int[] questionValues)
    {
        string questionsResults = "";
        for (int i = 0; i < questionValues.Length; i++)
        {
            questionsResults += "Q" + (i + 1).ToString() + ":" + questionValues[i].ToString() + "_";
        }
        yield return StartCoroutine(InsertData(questionsResults, 0, 0));
    }

    public IEnumerator SendsPersonalQuestionsToDatabase(string questionValues)
    {
        yield return StartCoroutine(InsertData(questionValues, 0, 0));
    }

    public void QuitGame()
    {
        StartCoroutine( QuitGameEnumerator());
    }

    public IEnumerator QuitGameEnumerator()
    {
        PlayerPrefs.SetString("mapToLoad", "default");

        //Uploads final data info to database
        yield return StartCoroutine(this.GetComponent<MySQLManager>().SendsDataToDatabase());

        Application.Quit();
    }

    void Update()
    {
        /*
        timeSinceLastPositionUpdate += Time.deltaTime;
        //sends position data every 2 seconds to database
        if (timeSinceLastPositionUpdate > 2.0f)
        {
            timeSinceLastPositionUpdate = 0.0f;
            StartCoroutine(InsertPlayerPos());
        }
        */
    }
}
