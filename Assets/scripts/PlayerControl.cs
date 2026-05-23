using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveForce = 100f;
    public float maxSpeed = 5;
    public Rigidbody2D rb;

    void Awake()
    {
        Debug.Log("Awake called");
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        if (rb != null)
        {
            float fInput = UnityEngine.Input.GetAxis("Horizontal");

            if (Mathf.Abs(rb.velocity.x) < maxSpeed)
            {
                rb.AddForce(Vector2.right * fInput * moveForce);
            }

            // Limit the player's speed
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
