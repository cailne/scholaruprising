using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpScript : MonoBehaviour {
    public GameObject player;
    public GameObject upAttack;
    // Use this for initialization
    void Start () {
        upAttack.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y+2);
        if (Input.GetKey(KeyCode.Space))
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y>0)
            {
                upAttack.SetActive(true);
            }
        }
        if(player.GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            upAttack.SetActive(false);
        }
    }
}
