using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    GameObject player;

    float timeSinceLastAtack;
    float attackDamageAxe;

    float attackIntervalTime;

    private void Start()
    {
        timeSinceLastAtack = Time.timeSinceLevelLoad;
        attackDamageAxe = 5.0f;
        attackIntervalTime = 0.5f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Element"))
        {
            if (player.GetComponent<PlayerMovement>().isAtacking && 
                Time.timeSinceLevelLoad - timeSinceLastAtack > attackIntervalTime)
            {
                other.GetComponent<ElementController>().Attack(attackDamageAxe);
                timeSinceLastAtack = Time.timeSinceLevelLoad;
                //Debug.Log(other.gameObject.name);
            }
        }
    }
}
