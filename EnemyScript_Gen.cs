using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript_Gen : MonoBehaviour {
    public GameObject Hero;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (!gameObject.activeSelf)
        {
            Debug.Log("Yo, u ded");
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.tag=="Attack")
        {
            col.transform.parent.SendMessage("airHang");
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        Debug.Log("Gt Out");
        if (col.gameObject.tag == "Player")
        {
            col.SendMessage("stagger");
        }
    }
}
