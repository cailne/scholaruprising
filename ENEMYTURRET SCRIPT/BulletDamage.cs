using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		if (!col.isTrigger) {
			if (col.gameObject.tag == "Player") {
				//col.GetComponent<PLAYERSCRIPT>DAMAGED FUNCTION();
			}
			Destroy (gameObject);
		}
	}
}
