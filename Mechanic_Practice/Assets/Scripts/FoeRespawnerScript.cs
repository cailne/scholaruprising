using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeRespawnerScript : MonoBehaviour {
    public GameObject respawnee;
    private float respawnCD;
    public float respawnTime;
	// Use this for initialization
	void Start () {
        respawnCD = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (respawnee.activeInHierarchy)
        {
            respawnCD = 0;
        }
        else
        {
            respawnCD += Time.deltaTime;
            if (respawnCD >= respawnTime)
            {
                respawnee.SetActive(true);
            }
        }
	}
}
