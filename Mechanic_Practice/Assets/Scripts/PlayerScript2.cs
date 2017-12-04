using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript2 : MonoBehaviour {
    new Animator playerAnim;
    public GameObject UIBoxHurt;
    public GameObject upAttack;
    public GameObject LAttack;
    public float speed;
    public float maxSpeed;
    public float maxSlide;
    public float jumpLimit;
    public float jumpForce;
    public float gravPower;
    public float maxYSpeed;
    private float maxYSpeedMemo;
    private float jumpDone = 0;
    private bool isGrounded;
    private bool isSliding = false;
    private bool isWallJump = false;
    private float wallJumpTimer = 0;
    private bool wallJumpDirection = false;
    public float wallJumpTime;
    private bool isAttack = false;
    private float attackCD = 0;
    public float attackCDMax;
    private bool isHang = false;
    private float airHangTime = 0;
    public float airHangMax;
    bool airhand = false;
    private int jumpTotal;
    private bool isStagger = false;
    private float staggerCD = 0;
    public float staggerCDMax;
    private bool iframeActive = false;
    private float iframe = 0;
    public float iframeMax;
    Rigidbody2D rb;
    private Color tmp;
    private float verticalJump;

    void Start()
    {
        upAttack.SetActive(false);
        LAttack.SetActive(false);
        UIBoxHurt.SetActive(false);
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        maxYSpeedMemo = maxYSpeed;
    }

    void FixedUpdate()
    {
        //controls animation booleans and variables
        if (isGrounded) { playerAnim.SetBool("Grounded", true); } else { playerAnim.SetBool("Grounded", false); }
        if (isAttack) { playerAnim.SetBool("Attacking", true); } else { playerAnim.SetBool("Attacking", false); }
        if (isSliding) { playerAnim.SetBool("Sliding", true); } else { playerAnim.SetBool("Sliding", false); }
        if (isStagger) { playerAnim.SetBool("Damaged", true); } else { playerAnim.SetBool("Damaged", false); }
        if (isHang) { playerAnim.SetBool("AirHang", true); } else { playerAnim.SetBool("AirHang", false); }
        playerAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    void Update()
    {
        Debug.Log("Grounded stat is " + isGrounded);

        //control animation flipping
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }




        //controls Stagger
        if (isStagger)
        {
            UIBoxHurt.SetActive(true);
            staggerCD += Time.deltaTime;
            if (staggerCD >= staggerCDMax)
            {
                isStagger = false;
                staggerCD = 0;
                UIBoxHurt.SetActive(false);
            }
        }

        //controls iframes
        if (iframeActive)
        {
            iframe += Time.deltaTime;

            if (iframe >= iframeMax)
            {
                iframeActive = false;
                iframe = 0;
                tmp = GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                GetComponent<SpriteRenderer>().color = tmp;
            }
        }

        //as long as you are not staggered
        if (!isStagger)
        {
            //attack with z
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!isAttack)
                {
                    upAttack.SetActive(false);
                    LAttack.SetActive(true);
                    isAttack = true;
                    attackCD = 0;
                }
            }


            //jump cancels air hang
            if (Input.GetKeyDown(KeyCode.Space))
            {
                airHangTime = 0;
                isHang = false;
            }
            //air hang cancels jump/move/gravity
            if (!isHang)
            {
                //jump Attack spawn/despawner
                if (Input.GetKey(KeyCode.Space))
                {
                    if (rb.velocity.y > 0)
                    {
                        upAttack.SetActive(true);
                    }
                }
                if (rb.velocity.y <= 0)
                {
                    upAttack.SetActive(false);
                }


                //Horizontal movements
                if (!isWallJump && !(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rb.velocity += Vector2.right * speed * Time.deltaTime;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rb.velocity += Vector2.left * speed * Time.deltaTime;
                    }
                    else
                    {
                        rb.velocity += new Vector2(-1, 0) * rb.velocity.x;
                    }
                }



                //stopper
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    rb.velocity += new Vector2(-1, 0) * rb.velocity.x;
                }

                //Jump Executor
                if (Input.GetKeyDown(KeyCode.Space) && jumpDone < jumpLimit)
                {
                    LAttack.SetActive(false);
                    // Debug.Log("Wall jump is " + isWallJump);
                    maxYSpeed = maxYSpeedMemo;
                    if (isGrounded)
                    {
                        isGrounded = false;
                        rb.velocity = Vector2.up * jumpForce;
                    }
                    else if (isSliding)
                    {
                        isWallJump = true;
                        isSliding = false;
                        rb.velocity = Vector2.up * jumpForce;

                        //wall jump to left, or right, stop on release or after a split second<---------------------------------------------------------------
                        //false = left, true = right
                        if (Input.GetKey(KeyCode.RightArrow)) { wallJumpDirection = false; }
                        if (Input.GetKey(KeyCode.LeftArrow)) { wallJumpDirection = true; }
                        //Debug.Log("WallJump Start");

                    }
                    //midAirjump
                    else
                    {
                        rb.velocity = Vector2.up * jumpForce;
                        jumpDone++;
                        wallJumpTimer = 0;
                    }

                }

                //wall jump
                if (isWallJump)
                {
                    wallJumpTimer += Time.deltaTime;
                    //Debug.Log("You have jumped for " + wallJumpTimer);
                    if (wallJumpTimer < wallJumpTime)
                    {
                        if (wallJumpDirection) { rb.velocity += Vector2.right * jumpForce; }
                        else { rb.velocity += Vector2.left * jumpForce; }
                        // Debug.Log("Velocity is at " + rb.velocity.x);
                        //Debug.Log("Facing is at "+wallJumpDirection);
                    }
                    else
                    {
                        wallJumpTimer = 0;
                        isWallJump = false;
                        // Debug.Log("WallJump End");
                    }
                }


                //momentum Cancellor on Up Jump
                if (rb.velocity.y > 0 && Input.GetKeyUp(KeyCode.Space))
                {
                    rb.velocity += Vector2.down * rb.velocity.y;
                    isWallJump = false;
                    //Debug.Log("Lost WallJuump by release");
                }

                //Speed Limiter
                if (rb.velocity.y > maxYSpeed) { rb.velocity += Vector2.down * (rb.velocity.y - maxYSpeed); }
                else if (rb.velocity.y < -1 * maxYSpeed) { rb.velocity += Vector2.up * -1 * (rb.velocity.y + maxYSpeed); }

                if (rb.velocity.x > maxSpeed) { rb.velocity += Vector2.left * (rb.velocity.x - maxSpeed); }
                else if (rb.velocity.x < -1 * maxSpeed) { rb.velocity += Vector2.right * -1 * (rb.velocity.x + maxSpeed); }

                if (isWallJump)
                {
                    if (rb.velocity.x > maxSpeed / 2) { rb.velocity += Vector2.left * (rb.velocity.x - maxSpeed / 2); }
                    else if (rb.velocity.x < -1 * maxSpeed / 2) { rb.velocity += Vector2.right * -1 * (rb.velocity.x + maxSpeed / 2); }
                }
                //Gravity
                if (!isGrounded)
                {
                    rb.velocity += Vector2.down * gravPower * Time.deltaTime;
                    maxYSpeed = maxYSpeedMemo;
                }
            }
            else if (isHang)
            {
                //airHang timing
                airHangTime += Time.deltaTime;
                rb.velocity = new Vector2(0, 0);
                if (airHangTime >= airHangMax)
                {
                    airHangTime = 0;
                    isHang = false;
                }

            }
            if (isAttack)
            {
                //attack CD
                attackCD += Time.deltaTime;
                if (attackCD >= attackCDMax / 2)
                {
                    LAttack.SetActive(false);
                }
                if (attackCD >= attackCDMax)
                {
                    attackCD = 0;
                    isAttack = false;
                }
            }
        }




    }
    void airHang()
    {
        if (!isGrounded)
        {
            isHang = true;
            jumpDone = 0;
        }
    }
    void stagger()
    {

        if (!iframeActive)
        {
            isStagger = true;
            LAttack.SetActive(false);
            upAttack.SetActive(false);
            rb.velocity = new Vector2(0, 0);
            iframeActive = true;
            tmp = GetComponent<SpriteRenderer>().color;
            tmp.a = 0.5f;
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {

        if (col.gameObject.tag == "Wall")
        {
            maxYSpeed = maxYSpeedMemo;
            isSliding = false;
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            if ((Input.GetKey(KeyCode.LeftArrow) && transform.position.x > col.transform.position.x) || (Input.GetKey(KeyCode.RightArrow) && transform.position.x < col.transform.position.x))
            {
                if (rb.velocity.y < 0)
                {
                    jumpDone = 0;
                    maxYSpeed = maxSlide;
                    isSliding = true;
                }
            }
            else
            {
                maxYSpeed = maxYSpeedMemo;
            }
        }

    }




    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Blocks")
        {
            isGrounded = false;
        }

    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Blocks")
        {
            isGrounded = true;
            jumpDone = 0;
        }

    }
}

//JUMP AND GRAVITY

// A FEW LOG CHECKS

/*if (doubleJump == true) {
    Debug.Log ("Double Jump is available");
} else {
    Debug.Log ("Double jump is not available");
}*/

/*if (isGrounded == true) {
    Debug.Log ("Character is grounded");
} else {
    Debug.Log ("Character is not grounded");
}*/
/*
if (isGrounded)
{
    jumpTime = 0;
    //Player can jump if they are grounded
    if (Input.GetKey(KeyCode.Space))
    {
        rb.velocity = Vector2.up * jumpForce;
        jumpTime = 1 * Time.deltaTime;
        Debug.Log("Jumptime is " + jumpTime);
    }
}
else
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (doubleJump == true)
        {
            //If you are on air (not grounded), trying to jump while doubleJump is true
            //then you are allowed to jump do so, even though you cannot hold-jumping
            //Right after you finish your secondary jump, doubleJump will be set into false
            //which means you cannot jump any longer unless you ARE BACK TO GROUND
            rb.velocity = Vector2.up * jumpForce;

        }
    }
}
  */
/*if (col.gameObject.tag=="Blocks") {
          if(transform.position.y>col.transform.position.y+col.transform.localScale.y/2+transform.localScale.y/2&&(transform.position.x>col.transform.position.x-col.transform.localScale.x/2) && (transform.position.x < col.transform.position.x + col.transform.localScale.x / 2))
          {
              isGrounded = true;
              jumpDone = 0;
              Debug.Log("is Grounded!");
          }
      }*/
