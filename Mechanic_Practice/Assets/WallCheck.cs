using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position= new Vector2(player.transform.position.x, player.transform.position.y);

    }
    private void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Wall")
        {
            player.SendMessage("wallSlide");
        }
        
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            player.SendMessage("wallExit");
        }
    }
}
