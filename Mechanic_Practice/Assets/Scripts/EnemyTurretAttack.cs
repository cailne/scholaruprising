using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretAttack : MonoBehaviour {

	public EnemyTurret enemy;

	public bool isRight = false;

	void Awake(){
		enemy = gameObject.GetComponentInParent<EnemyTurret> ();
	}

	void OnTriggerStay2D(Collider2D col){

		if (col.gameObject.tag == "Player") {

			if (isRight) {
				enemy.Attack (true);
			} else {
				enemy.Attack (false);
			}

		}

	}
}
