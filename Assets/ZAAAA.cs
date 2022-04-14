using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody2D rigi;
    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    [Header("Walk")]
    float speedXValue;
    float xMove;
    public float speedX;
    public float runSpeed;
    [Header("Jump")]
    public float jumpForce;
    public int extraJumpsValue;
    int extraJumps;
    [Header("Dash")]
    public float dashForce;
    public float dashTime;
    public float dashCooldown;
    bool isDash = false;
    float dashTimeC = 0;
    float dashCooldownC = 0;
    [Header("Soar")]
    public float soarInGravity;
    [Header("Crouch")]
    bool isCrouch = false;



    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
        speedXValue = speedX;
    }


    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        xMove = Input.GetAxis("Horizontal");
        if (isCrouch == false)
        {
            walk();
        }
    }
    void Update()
    {
        crouch();
        if (isCrouch == false)
        {
            dash();
            run();
            jump();
            soar();
        }
        characterRotate();
    }
    void run()
    {
        if (isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            speedXValue = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedXValue = speedX;
        }
    }
    void characterRotate()
    {
        if (xMove > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (xMove < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void walk()
    {
        rigi.velocity = new Vector2(xMove * Time.deltaTime * speedXValue, rigi.velocity.y);
    }
    void dash()
    {
        if (Input.GetKeyDown(KeyCode.E) && xMove != 0)
        {
            if (Time.time > dashCooldownC)
            {
                isDash = true;
                dashTimeC = Time.time + dashTime;
            }
           
        }
        if (isDash)
        {
            if (Time.time >= dashTimeC)
            {
                isDash = false;
                dashCooldownC = Time.time + dashCooldown;
            }
            if (xMove > 0)
            {
                rigi.AddForce(Vector2.right * dashForce);
            }
            else
            {
                rigi.AddForce(Vector2.left * dashForce);
            }
        }
    }
    void jump()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rigi.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
    }
    void soar()
    {
        if (Input.GetKey(KeyCode.R) && isGrounded == false)
        {
            rigi.gravityScale = soarInGravity;
        }
        if (Input.GetKeyUp(KeyCode.R) || isGrounded)
        {
            rigi.gravityScale = 1;
        }
    }
    void crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            isCrouch = true;
            transform.localScale = new Vector3(6, 4, 6);
        }else if (Input.GetKeyUp(KeyCode.LeftControl) || isGrounded==false)
        {
            isCrouch = false;
            transform.localScale = new Vector3(6, 6, 6);
        }
    }
}
