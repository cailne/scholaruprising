using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAllDirectionAttack : MonoBehaviour {

	public EnemyAllDirection enemy;

	//public bool isRight = false;

	void Awake(){
		enemy = gameObject.GetComponentInParent<EnemyAllDirection> ();
	}

	void OnTriggerStay2D(Collider2D col){

		if (col.gameObject.tag == "Player") {

			enemy.Attack ();

		}

	}
}
