using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIingame : MonoBehaviour {
    public static UIingame instance;
    private Color tmp;

    void Start()
    {
        instance = this;
    }

    public GameObject boostReady;
    public GameObject dashReady;

    public void boostNotReady()
    {
        boostReady.SetActive(false);
        /*
        tmp = boostReady.color;
        tmp.a = 0.5f;
        GetComponent<SpriteRenderer>().color = tmp;
        */
    }

    public void dashNotReady()
    {
        dashReady.SetActive(false);
    }

    public void dashYES()
    {
        dashReady.SetActive(true);
    }

    public void boostYES()
    {
        boostReady.SetActive(true);
    }
}
