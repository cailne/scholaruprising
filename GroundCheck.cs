using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private Player plyr;

    void Start()
    {

        plyr = gameObject.GetComponentInParent<Player>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (null == plyr)
        {
            Start();
        }
        plyr.grounded = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        plyr.grounded = false;
    }

}
