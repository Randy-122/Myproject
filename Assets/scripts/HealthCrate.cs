using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCtarte : MonoBehaviour
{
    public float healthBonus;
    public AudioClip collect;
    public Animator anim;  //需要在初始化				
    private bool landed = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (anim == null) 
        {
            anim = GetComponent<Animator>();
        }
        if (other.tag == "Player")
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.health += healthBonus;
            playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);
            playerHealth.UpdateHealthBar();
            AudioSource.PlayClipAtPoint(collect, transform.position);
            Destroy(transform.root.gameObject);
        }
        else if (other.tag == "Ground" && !landed)
        {
            anim.SetTrigger("Land");
            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }
    }

}
