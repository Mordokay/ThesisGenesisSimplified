using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemControler : MonoBehaviour {

    float rotationSpeed = 50.0f;
    public int type;
    QuestsController qc;

    void Update () {
        this.transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
        qc = GameObject.FindGameObjectWithTag("GameManager").GetComponent<QuestsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (qc.playerStash < 2)
            {
                qc.IncrementStash(type);
                Destroy(this.gameObject);
            }
        }
    }
}
