using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {

    public float zoomSpeed;
    public float orthographicSizeMin;
    public float orthographicSizeMax;
    private Camera myCamera;
    public GameObject leftUIBox;
    public GameObject rightUIBox;
    public bool zoomToPlayMode;

    EditorModeController em;
    MouseInputController mic;

    public float cameraDefaultSize;

    void Start()
    {
        em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditorModeController>();
        mic = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MouseInputController>();
        zoomToPlayMode = false;
        myCamera = Camera.main;
    }

    void Update()
    {
        if (zoomToPlayMode)
        {
            if (Camera.main.orthographicSize - cameraDefaultSize < 0.05f)
            {
                Camera.main.orthographicSize = cameraDefaultSize;
                zoomToPlayMode = false;
            }
            else if (Camera.main.orthographicSize > cameraDefaultSize)
            {
                Camera.main.orthographicSize -= Time.deltaTime * 6.0f;
            }
            else
            {
                Camera.main.orthographicSize += Time.deltaTime * 6.0f;
            }
        }

        if (em.isEditorMode && !em.isSpawningEvent && Input.mousePosition.x < 0.76 * Screen.width)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                myCamera.orthographicSize += zoomSpeed;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                myCamera.orthographicSize -= zoomSpeed;
            }
            myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, orthographicSizeMin, orthographicSizeMax);
        }
        else if(em.isEditorMode && em.isSpawningEvent)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                em.ChangeSizeSpawnEventArea(-zoomSpeed * 0.2f, 0);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                em.ChangeSizeSpawnEventArea(zoomSpeed * 0.2f, 0);
            }
        }
        else if(!em.isEditorMode && mic.eventSpawnerArea.activeSelf)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                em.ChangeSizeSpawnEventArea(-zoomSpeed * 0.2f, 1);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                em.ChangeSizeSpawnEventArea(zoomSpeed * 0.2f, 1);
            }
        }
    }
}