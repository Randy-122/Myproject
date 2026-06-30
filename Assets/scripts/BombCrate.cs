using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCrate : MonoBehaviour
{

    public AudioClip pickupClip;
    private Animator anim;      		
    private bool landed = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (anim == null) 
        {
            anim = GetComponentInParent<Animator>();
        }
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);
            other.GetComponent<LayBombs>().bombCount++;//
            Destroy(transform.root.gameObject);
        }
        else if (other.tag == "Ground" && !landed)
        {
            anim.SetTrigger("land");
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }


    }
}
