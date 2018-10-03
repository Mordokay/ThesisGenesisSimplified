using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolGoalFeedback : MonoBehaviour {

    public List<GameObject> Arrows;
    public Transform origin;
    public Vector3 destination;

    GameObject arrow;

    public float timeSinceLastArrow;
    public float timeSinceLastGoalArrow;

    public float arrowForceSpeed;

    public bool isTalkArrow;
    Color talkColor;
    EditorModeController em;

    void Start () {
        talkColor = Color.blue;
        arrow = Resources.Load("Arrow") as GameObject;

        em = GameObject.FindGameObjectWithTag("GameManager").GetComponent<EditorModeController>();
        Arrows = new List<GameObject>();
        timeSinceLastArrow = Time.timeSinceLevelLoad;
        arrowForceSpeed = 200.0f;
        destination = Vector3.one;
    }
	
    public void ClearAllArrows()
    {
        //Debug.Log("Clearing line with " + Arrows.Count + " arrows");
        for (int i = Arrows.Count - 1; i >= 0; i--)
        {
            Destroy(Arrows[i]);
            Arrows.RemoveAt(i);
        }
    }

    void ChangeLineColor(Color newColor)
    {
        for (int i = Arrows.Count - 1; i >= 0; i--)
        {
            Arrows[i].GetComponentInChildren<SpriteRenderer>().color = newColor;
        }
    }

	void Update () {

        if (isTalkArrow)
        {
            timeSinceLastGoalArrow += Time.deltaTime;
            if (!destination.Equals(Vector3.one) && timeSinceLastGoalArrow > 0.4f)
            {
                GameObject myArrow = Instantiate(arrow, this.transform);
                Destroy(myArrow, 3.0f);
                myArrow.transform.position = origin.transform.position;
                myArrow.GetComponent<Rigidbody>().AddForce((destination - myArrow.transform.position).normalized * (arrowForceSpeed));
                myArrow.GetComponentInChildren<SpriteRenderer>().color = talkColor;
                //Debug.Log("destination " + destination + " myArrow.transform.position " + myArrow.transform.position);
                myArrow.transform.LookAt(destination);
                Arrows.Add(myArrow);
                timeSinceLastGoalArrow = 0;
            }
        }
        else
        {
            timeSinceLastArrow += Time.deltaTime;
            if (!destination.Equals(Vector3.one) && timeSinceLastArrow > 0.1f)
            {
                GameObject myArrow = Instantiate(arrow, this.transform);
                Destroy(myArrow, 3.0f);
                myArrow.transform.position = origin.transform.position;
                myArrow.GetComponent<Rigidbody>().AddForce((destination - myArrow.transform.position).normalized * arrowForceSpeed);
                myArrow.GetComponentInChildren<SpriteRenderer>().color =  origin.transform.GetComponentInChildren<SpriteRenderer>().color;

                myArrow.transform.LookAt(destination);
                Arrows.Add(myArrow);
                timeSinceLastArrow = 0;
            }
        }
        for (int i = Arrows.Count - 1; i >= 0; i--)
        {
            if (Arrows[i] != null)
            {
                if (isTalkArrow)
                {
                    if (Vector3.Distance(Arrows[i].transform.position, destination) < 0.1f ||
                        Arrows[i].transform.position.x >= (em.mapWidth / 2) || Arrows[i].transform.position.x < -(em.mapWidth / 2)
                        || Arrows[i].transform.position.y >= (em.mapHeight / 2) || Arrows[i].transform.position.y < -(em.mapHeight / 2))
                    {
                        Destroy(Arrows[i]);
                        Arrows.RemoveAt(i);
                    }

                }
                else
                {
                    if (Vector3.Distance(Arrows[i].transform.position, destination) < 0.5f ||
                        Arrows[i].transform.position.x >= (em.mapWidth / 2) || Arrows[i].transform.position.x < -(em.mapWidth / 2)
                        || Arrows[i].transform.position.y >= (em.mapHeight / 2) || Arrows[i].transform.position.y < -(em.mapHeight / 2))
                    {
                        Destroy(Arrows[i]);
                        Arrows.RemoveAt(i);
                    }
                }
            }
        }
	}
}