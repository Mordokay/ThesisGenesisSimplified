using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    GameObject myPlayer;
    public float cameraMoveSpeed;
    
    void Start () {
        myPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
        if (!this.GetComponent<EditorModeController>().isEditorMode)
        {
            if (myPlayer == null)
            {
                myPlayer = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                int mapWidth = this.GetComponent<EditorModeController>().mapWidth;
                int mapHeight = this.GetComponent<EditorModeController>().mapHeight;
                Camera.main.transform.position = new Vector3(myPlayer.transform.position.x, Camera.main.transform.position.y,
                    myPlayer.transform.position.z);
                Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x, -(mapWidth / 2 - 5.78f), (mapWidth / 2 - 5.78f)), Camera.main.transform.position.y,
                        Mathf.Clamp(Camera.main.transform.position.z, -(mapHeight / 2 - 3.4f), (mapHeight / 2 - 3.4f)));
            }
        }
	}
}
