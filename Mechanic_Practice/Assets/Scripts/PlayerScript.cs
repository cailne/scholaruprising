using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    new Animator playerAnim;
    public GameObject UIBoxHurt;
    public GameObject upAttack;
    public GameObject LAttack;
    public float dashSpeed;
    public float speed;
    public float maxSpeed;
    private float maxSpeedMemo;
    public float maxSlide;
    public float jumpLimit;
	public float jumpForce;
    public float gravPower;
    public float maxYSpeed;
    private float maxYSpeedMemo;
    private float jumpDone = 0;
    private float dashDone = 0;
	private bool isGrounded;
    private bool isSliding=false;
    private bool isWallJump=false;
    private float wallJumpTimer = 0;
    private bool facingDirection = false;
    private bool dashDirection = false;
    public float wallJumpTime;
    private bool wallContact = false;
    private bool isAttack=false;
    private float attackCD = 0;
    public float attackCDMax;
    private bool isHang = false;
    private float airHangTime = 0;
    public float airHangMax;
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

    public AudioClip[] sounds;
    private AudioSource Audio;


	void Start () {
        upAttack.SetActive(false);
        LAttack.SetActive(false);
        UIBoxHurt.SetActive(false);
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D> ();
        maxYSpeedMemo = maxYSpeed;
        maxSpeedMemo = maxSpeed;

        Audio = GetComponent<AudioSource>();

    }

    void PlaySound(int element)
    {
        Audio.clip = sounds[element];
        Audio.Play();
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

    void Update() {
        
        //control animation flipping
        if (!isAttack) {
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
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
                if (!isAttack && dashDone < 1) {
                    PlaySound(0); //voice pas attack
                    upAttack.SetActive(false);
                    LAttack.SetActive(true);
                    isAttack = true;
                    attackCD = 0;
                    dashDone += 1;
                    isWallJump = false;
                }
            }
            //cancel attack by letting go of Z
            if (Input.GetKeyUp(KeyCode.Z) && isAttack)
            {
                isAttack = false;
                attackCD = 0;
                LAttack.SetActive(false);
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
                if (!isAttack && !isWallJump && !(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        dashDirection = true;
                        rb.velocity += Vector2.right * speed * Time.deltaTime;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        dashDirection = false;
                        rb.velocity += Vector2.left * speed * Time.deltaTime;
                    }
                    else
                    {
                        rb.velocity += new Vector2(-1, 0) * rb.velocity.x;
                    }
                }



                //stopper
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || isAttack)
                {
                    rb.velocity += new Vector2(-1, 0) * rb.velocity.x;
                }

                //Jump Executor
                if (Input.GetKeyDown(KeyCode.Space) && jumpDone < jumpLimit)
                {
                    PlaySound(1);
                    playerAnim.Play("Air", 0, 0f);
                    LAttack.SetActive(false);
                    // Debug.Log("Wall jump is " + isWallJump);
                    maxYSpeed = maxYSpeedMemo;
                    if (isGrounded)
                    {
                        isGrounded = false;
                        rb.velocity = Vector2.up * jumpForce;
                        Debug.Log("Normal Jump");
                    }
                    else if (isSliding)
                    {
                        Debug.Log("isWallJumping");
                        isWallJump = true;
                        isSliding = false;
                        rb.velocity = Vector2.up * jumpForce;

                        //wall jump to left, or right, stop on release or after a split second<---------------------------------------------------------------
                        //false = left, true = right
                        if (Input.GetKey(KeyCode.RightArrow)) { facingDirection = false; }
                        if (Input.GetKey(KeyCode.LeftArrow)) { facingDirection = true; }
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
                        if (facingDirection) { rb.velocity += Vector2.right * jumpForce; }
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
                if (!isGrounded && !isAttack)
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
                //cancel attack
                LAttack.SetActive(false);
                attackCD = 0;
                isAttack = false;

            }
            //Attacking
            if (isAttack)
            {
                //attack CD
                maxSpeed = dashSpeed;
                if (dashDirection) { rb.velocity += Vector2.right * dashSpeed; }
                else { rb.velocity += Vector2.left * dashSpeed; }

                //Stop vertical movement
                rb.velocity += new Vector2(0, -1) * rb.velocity.y;

                attackCD += Time.deltaTime;
                if (attackCD >= attackCDMax)
                {
                    LAttack.SetActive(false);
                    attackCD = 0;
                    isAttack = false;
                }
            }
            else { maxSpeed = maxSpeedMemo; }
            //WallSliding
            if (wallContact&&!isWallJump) {
                if (dashDirection)
                {
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        jumpDone = 0;
                        dashDone = 0;
                        maxYSpeed = maxSlide;
                        isSliding = true;
                    }
                    else { isSliding = false; maxYSpeed = maxYSpeedMemo; }
                }else
                {
                    if (Input.GetKey(KeyCode.LeftArrow)) {
                        jumpDone = 0;
                        dashDone = 0;
                        maxYSpeed = maxSlide;
                        isSliding = true;
                    }
                    else { isSliding = false; maxYSpeed = maxYSpeedMemo; }
                }
            }
            else {
                maxYSpeed = maxYSpeedMemo;
                isSliding = false;
            }
        }
        //UI Control

        if (dashDone > 0)
        {UIingame.instance.dashNotReady();}
        else
        {UIingame.instance.dashYES();}

        if (jumpDone > 0)
        { UIingame.instance.boostNotReady(); }
        else
        { UIingame.instance.boostYES(); }

    }
    void airHang()
    {
       isHang = true;
       jumpDone = 0;
       dashDone = 0;
    }
    void stagger()
    {
        
        if (!iframeActive)
        {
            PlaySound(2); //voice effect pas hurt
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

    //Controls wall sliding
    private void wallSlide()
    {
        if (rb.velocity.y<0&&!isGrounded) {
            wallContact = true;
        }
        
    }
    private void wallExit()
    {
        wallContact = false;
    }
    /*
    private void OnCollisionExit2D(Collision2D col)
    {

        if (col.gameObject.tag == "Wall")
        {
            maxYSpeed = maxYSpeedMemo;
            isSliding = false;
        }
    }
    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "Wall")
        {
            Debug.Log("Walled stat is " + isSliding);
            Debug.Log("Max Y stat is " + maxYSpeed);
            LAttack.SetActive(false);
            attackCD = 0;
            isAttack = false;
            if ((Input.GetKey(KeyCode.LeftArrow) &&transform.position.x>col.transform.position.x)|| (Input.GetKey(KeyCode.RightArrow) && transform.position.x < col.transform.position.x))
            {
                if (rb.velocity.y<=0) {
                    jumpDone = 0;
                    dashDone = 0;
                    maxYSpeed = maxSlide;
                    isSliding = true;
                }
            }
            else
            {
                maxYSpeed = maxYSpeedMemo;
                isSliding = false;
            }
        }

    }
    */
    

    //Controls ground animation, so it doesn't jitter when it runs normally. Also controls if you are grounded or not.
    
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Blocks")
        {
            wallContact = false;
            isSliding = false; maxYSpeed = maxYSpeedMemo;
            isGrounded = true;
            jumpDone = 0;
            dashDone = 0;
        }
        
    }
    private void OnTriggerExit2D(Collider2D col)
    {
            isGrounded = false;   
    }
}
