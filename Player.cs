using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 3f;
    public float speed = 50f;
    public float jumpPower = 50f;

    public bool grounded;

    private Rigidbody2D rb2dp;
    private Animator anim;

	// Use this for initialization
	void Start () {
        rb2dp = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Grounded", true);
        anim.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        rb2dp.AddForce((Vector2.right * speed) * x);

        if(rb2dp.velocity.x > maxSpeed)
        {
            rb2dp.velocity = new Vector2(maxSpeed, rb2dp.velocity.y);
        }
        if (rb2dp.velocity.x < -maxSpeed)
        {
            rb2dp.velocity = new Vector2(-maxSpeed, rb2dp.velocity.y);
        }
    }
}
