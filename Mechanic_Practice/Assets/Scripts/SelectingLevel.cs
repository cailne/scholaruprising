using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingLevel : MonoBehaviour {

	public GameObject button;
	private int unlock;
	public int howtoUnlock;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("ULevels") == 0) {
			PlayerPrefs.SetInt ("ULevels", 1);
		}
		unlock = PlayerPrefs.GetInt ("ULevels");
	}
	
	// Update is called once per frame
	void Update () {
		if (unlock < howtoUnlock) {
			button.SetActive (false);
		} else if (unlock >= howtoUnlock) {
			button.SetActive (true);
		}
	}
}