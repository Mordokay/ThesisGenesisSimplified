using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour {

    float duration;
    float moveSpeed;
	
	void Update () {
        this.transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        duration -= Time.deltaTime;

		if(duration < 0)
        {
            Destroy(this.gameObject);
        }
	}

    public void Initialize(Vector3 pos, float duration, float moveSpeed, string damage)
    {
        this.transform.position = pos;
        this.GetComponent<TextMesh>().text = damage;
        this.duration = duration;
        this.moveSpeed = moveSpeed;
    }
}
