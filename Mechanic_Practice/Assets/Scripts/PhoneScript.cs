using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PhoneScript : MonoBehaviour {
    
	public int unlocking;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D col)
    {
		PlayerPrefs.SetInt ("ULevels", unlocking);
        SceneManager.LoadScene("SelectLevel");
    }
}
