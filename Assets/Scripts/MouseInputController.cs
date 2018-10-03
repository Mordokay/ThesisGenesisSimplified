using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseInputController : MonoBehaviour {

    GameObject gm;

    public LayerMask TerrainLayerMask;
    public LayerMask UndergroundLayerMask;
    public LayerMask ElementLayerMask;
    public LayerMask PlacingElementLayerMask;
    public LayerMask PatrolLayerMask;
    public LayerMask NPCLayerMask;

    Vector3 lastMousePos;
    public string lastTerrainTileClicked;
    string lastUndergroundTileClicked;

    GameObject player;
    public GameObject eventSpawnerArea;

    UIManager uiManager;

    void Start () {
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        lastMousePos = Vector3.zero;
        gm = GameObject.FindGameObjectWithTag("GameManager");
        lastTerrainTileClicked = "";
        lastUndergroundTileClicked = "";
    }

    void Update()
    {
        //Show Circle around the cursor
        if (eventSpawnerArea.activeSelf)
        {
            Vector3 newMousePos = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0.0f, 0.76f * Screen.width), Input.mousePosition.y, 0.0f);
            eventSpawnerArea.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(newMousePos).x, 0.0f,
                Camera.main.ScreenToWorldPoint(newMousePos).z) ;
        }
        
        if (Input.GetMouseButton(0))
        {
            if (!lastMousePos.Equals(Input.mousePosition) && gm.GetComponent<EditorModeController>().isDrawingTerrain)
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, TerrainLayerMask);

                if (hit.collider != null && !lastTerrainTileClicked.Equals(hit.collider.gameObject.name))
                {
                    if (hit.collider.tag.Equals("Terrain"))
                    {
                        lastTerrainTileClicked = hit.collider.gameObject.name;
                        Vector3 pos = hit.collider.gameObject.transform.position;
                        if (gm.GetComponent<EditorModeController>().removeTerrain)
                        {
                            gm.GetComponent<EditorModeController>().removeTerrainAtPos((int)pos.x, (int)pos.z);
                        }
                        else
                        {
                            gm.GetComponent<EditorModeController>().SetTerrainAtPos((int)pos.x, (int)pos.z);
                        }
                    }
                }
                lastMousePos = Input.mousePosition;
            }
            //player attacks
            if (!gm.GetComponent<EditorModeController>().isEditorMode && this.GetComponent<TutorialController>().tutorialStage >= 7)
            {
                player.GetComponent<Animator>().SetBool("Attack", true);

                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, ElementLayerMask);
                //Debug.Log(hit.collider.name);
                if (hit.collider != null && Vector3.Distance(hit.collider.gameObject.transform.position, player.transform.position) < 1.5f)
                {
                    if (!player.GetComponent<PlayerMovement>().isAtacking)
                    {
                        player.GetComponent<PlayerMovement>().isAtacking = true;
                    }
                    player.GetComponent<PlayerMovement>().objectBeingAtacked = hit.collider.gameObject;
                }
                else
                {
                    player.GetComponent<PlayerMovement>().isAtacking = false;
                    //player.GetComponent<Animator>().SetBool("Attack", false);
                }

                Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, NPCLayerMask);
                //Debug.Log(hit.collider.name);
                if (hit.collider != null && Vector3.Distance(hit.collider.gameObject.transform.position, player.transform.position) < 1.5f)
                {
                    Debug.Log("PlayerAtacked " + hit.collider.gameObject.transform.parent.name);
                    hit.collider.gameObject.GetComponent<NPCPatrolMovement>().AtackedByPlayer();
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (!lastMousePos.Equals(Input.mousePosition) && gm.GetComponent<EditorModeController>().isDrawingTerrain)
            {
                RaycastHit hit;
                Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, UndergroundLayerMask);

                if (hit.collider != null && !lastUndergroundTileClicked.Equals(hit.collider.gameObject.name))
                {
                    if (hit.collider.tag.Equals("Underground"))
                    {
                        lastUndergroundTileClicked = hit.collider.gameObject.name;
                        Vector3 pos = hit.collider.gameObject.transform.position;
                        gm.GetComponent<EditorModeController>().SetUndergroundAtPos((int)pos.x, (int)pos.z);
                    }
                }
                lastMousePos = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //player stops attacking
            if (!gm.GetComponent<EditorModeController>().isEditorMode)
            {
                player.GetComponent<PlayerMovement>().isAtacking = false;
                player.GetComponent<Animator>().SetBool("Attack", false);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!gm.GetComponent<EditorModeController>().isEditorMode)
            {
                if (eventSpawnerArea.activeSelf)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, TerrainLayerMask);
                    //Debug.Log(hit.collider.name);
                    if (hit.collider != null && hit.collider.tag.Equals("Terrain"))
                    {
                        //Events are spawned on the exact position

                        //Vector3 pos = hit.collider.gameObject.transform.position;
                        Vector3 pos = hit.point;
                        gm.GetComponent<EditorModeController>().AddEvent(pos, 1);
                    }
                }
            }
            else
            {
                if (gm.GetComponent<EditorModeController>().isPlacingElements)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, PlacingElementLayerMask);

                    if (hit.collider != null)
                    {
                        if (hit.collider.tag.Equals("Terrain"))
                        {
                            Vector3 pos = hit.collider.gameObject.transform.position;
                            gm.GetComponent<EditorModeController>().InsertElement(pos);
                        }
                    }
                }
                else if (gm.GetComponent<EditorModeController>().isPlacingNPC)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, TerrainLayerMask);

                    if (hit.collider != null)
                    {
                        if (hit.collider.tag.Equals("Terrain"))
                        {
                            Vector3 pos = hit.collider.gameObject.transform.position;
                            gm.GetComponent<EditorModeController>().InsertNPC(pos);
                        }
                    }
                }
                else if (gm.GetComponent<EditorModeController>().removeElement)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, ElementLayerMask);

                    if (hit.collider != null)
                    {
                        gm.GetComponent<EditorModeController>().RemoveElement(hit.collider.gameObject);
                    }
                }
                else if (gm.GetComponent<EditorModeController>().removePatrolPoint)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, PatrolLayerMask);

                    if (hit.collider != null && hit.collider.tag == "PatrolPoint")
                    {
                        gm.GetComponent<EditorModeController>().RemovePatrolPoint(hit.collider.gameObject);
                    }
                }
                else if (gm.GetComponent<EditorModeController>().removeNPC)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, NPCLayerMask);

                    if (hit.collider != null && hit.collider.tag == "NPC")
                    {
                        Destroy(hit.collider.gameObject.transform.parent.gameObject);
                    }
                }
                else if (gm.GetComponent<EditorModeController>().isInspectingElement)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, NPCLayerMask);

                    if (hit.collider != null && hit.collider.tag == "NPC")
                    {
                        //Debug.Log("I am gonna inspect NPC called: " + hit.collider.gameObject.transform.parent.gameObject.name);
                        uiManager.RefreshNPCUpdater(hit.collider.gameObject.GetComponentInParent<NPCData>());
                    }
                    else
                    {
                        RaycastHit hit2;
                        Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit2, Mathf.Infinity, PatrolLayerMask);

                        if (hit2.collider != null && hit2.collider.tag == "PatrolPoint")
                        {
                            //Debug.Log("I am gonna inspect NPC called: " + hit.collider.gameObject.transform.parent.gameObject.name);
                            uiManager.RefreshPatrolPointInspectorPanel(hit2.collider.gameObject.GetComponentInParent<PatrolPointData>());
                            //Debug.Log("I just hit a patrol point!!!");
                        }
                    }
                }
                else if (gm.GetComponent<EditorModeController>().isSpawningEvent)
                {
                    RaycastHit hit;
                    Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, TerrainLayerMask);

                    if (hit.collider != null && hit.collider.tag.Equals("Terrain"))
                    {
                        //Events are spawned on the exact position

                        //Vector3 pos = hit.collider.gameObject.transform.position;
                        Vector3 pos = hit.point;
                        gm.GetComponent<EditorModeController>().AddEvent(pos, 0);
                    }
                }
            }
        }
    }
}