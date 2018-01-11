using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public PlayerScript plyr;
	private Slider _slider;

	// Use this for initialization
	void Start () {
		_slider = GetComponentInChildren<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		_slider.value = plyr.health;
	}
}
