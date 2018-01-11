using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour {

    public float timerPeluru;
    public float intervalPeluru;

    void Update()
    {
        timerPeluru += Time.deltaTime;

        if (timerPeluru >= intervalPeluru)
        {
            Destroy(gameObject);
        }
    }

	void OnTriggerEnter2D(Collider2D col){
		if (!col.isTrigger) {
			if (col.gameObject.tag != "Enemy") {
				Destroy (gameObject);
			}
		}
		if (col.gameObject.tag == "Player")
		{
			col.SendMessage("stagger");
			col.SendMessage ("damaged");
		}
	}
}
