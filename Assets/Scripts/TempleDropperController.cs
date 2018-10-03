using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleDropperController : MonoBehaviour {

    public QuestsController qc;

    private void Start()
    {
        qc = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            qc.UpdateQuestsBar();
        }
    }
}