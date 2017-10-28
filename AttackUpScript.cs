using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpScript : MonoBehaviour {
    public GameObject player;
    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y>0)
            {
                gameObject.SetActive(true);
            }
        }
        if(player.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
