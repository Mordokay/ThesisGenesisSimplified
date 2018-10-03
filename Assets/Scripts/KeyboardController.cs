using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

    GameObject gm;
    public float speedCamera;

    //30 frames per second
    float fakeDeltaTime = 0.0333f;
    public UIManager uiManager;

    void Start()
    {
        speedCamera = 10.0f;
        gm = GameObject.FindGameObjectWithTag("GameManager");
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gm.GetComponent<EditorModeController>().isEditorMode && Input.mousePosition.x < 0.76 * Screen.width)
            {
                uiManager.Play();
            }
            else
            {
                uiManager.Pause();
            }
        }
        */
        if (gm.GetComponent<EditorModeController>().isEditorMode && Input.mousePosition.x < 0.76 * Screen.width)
        {
            if (Input.GetKey(KeyCode.W))
            {
                Camera.main.transform.Translate(new Vector3(0.0f, fakeDeltaTime * speedCamera, 0.0f));
            }
            if (Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.Translate(new Vector3(-fakeDeltaTime * speedCamera, 0.0f, 0.0f));
            }
            if (Input.GetKey(KeyCode.S))
            {
                Camera.main.transform.Translate(new Vector3(0.0f, -fakeDeltaTime * speedCamera, 0.0f));
            }
            if (Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.Translate(new Vector3(fakeDeltaTime * speedCamera, 0.0f, 0.0f));
            }
        }
    }
}
