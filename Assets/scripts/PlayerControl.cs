using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveForce = 100f;
    public float maxSpeed = 5;
    public Rigidbody2D rb;
    public bool bFaceRight = true;
    public Transform mGroundCheck;
    public bool bJump = false;
    public float jumpForce = 1000;
    private Animator mAnimator;
    void Awake()
    {
        Debug.Log("Awake called");
        rb = GetComponent<Rigidbody2D>();
        mGroundCheck = transform.Find("GroundCheck");
        mAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }
    private void FixedUpdate()
    {

        if (rb != null)
        {
            float fInput = UnityEngine.Input.GetAxis("Horizontal");
            mAnimator.SetFloat("speed", Mathf.Abs(fInput));


            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            {
                rb.AddForce(Vector2.right * fInput * moveForce);
            }

            // Limit the player's speed
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }

            if (fInput > 0 && !bFaceRight)
                flip();
            if (fInput < 0 && bFaceRight)
                flip();
            void flip()
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                bFaceRight = !bFaceRight;
            }
            if (bJump) 
            {
                mAnimator.SetTrigger("jump");
                rb.AddForce(new Vector2(0, jumpForce));
                bJump = false;
            }
            if (Physics2D.Linecast(transform.position, mGroundCheck.position,
            1 << LayerMask.NameToLayer("Ground")))
            {
                mAnimator.SetTrigger("ground");
            }
            else {
                mAnimator.ResetTrigger("jump");
                mAnimator.ResetTrigger("ground");
            }

        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Physics2D.Linecast(transform.position, mGroundCheck.position, 
            1 << LayerMask.NameToLayer("Ground")) && UnityEngine.Input.GetButtonDown("Jump")) 
        {
            
            bJump = true;
        }


    }


}
