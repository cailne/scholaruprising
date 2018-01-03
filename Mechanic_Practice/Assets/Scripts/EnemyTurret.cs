using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour {

	//You can add health here

	public float distance;
	public float shootInterval;
	public float bulletSpeed = 100;
	public float bulletTimer;


	public GameObject bullet;
	public Transform target;
	public Transform shootPointLeft, shootPointRight;


	void Start(){
		//set health
	}

	void Update(){

	}

	public void Attack(bool attackRight){
		bulletTimer += Time.deltaTime;

		if (bulletTimer >= shootInterval) {
			Vector2 direction = target.transform.position - transform.position;
			direction.Normalize ();

			if (!attackRight) { //assume that the player is on the left
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				bulletTimer = 0;
			}

			if (attackRight) { //assume that the player is on the right
				GameObject bulletClone;
				bulletClone = Instantiate (bullet, shootPointRight.transform.position, shootPointRight.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D> ().velocity = direction * bulletSpeed;

				bulletTimer = 0;
			}
		}
	}
}
