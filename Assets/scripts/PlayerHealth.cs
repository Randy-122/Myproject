using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 100f;
    public float repeatHurtPeriod = 0.5f;
    public float hurtForce = 10f;
    public float upForce = 10f;
    public AudioClip[] ouchClips;
    public float damageAmount = 30f;

    private float lastHurtTime = 0;
    private PlayerControl PlayerControl;
    private Animator anim;

    private SpriteRenderer healthBar;
    private Vector3 healthScale;
    void Start()
    {
        PlayerControl = GetComponent<PlayerControl>();
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("Health").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            if(Time.time > lastHurtTime + repeatHurtPeriod)
            {
                if (health > 0)
                {
                    lastHurtTime = Time.time;
                    TakeDamage(collision.gameObject.transform);
                }
                else 
                {
                    HeroDie();
                }
            }
        }

    }

    private void TakeDamage(Transform enemyTran) 
    {
        
        PlayerControl.bJump = false;
        Vector3 hurtVector = transform.position - enemyTran.position + Vector3.up * upForce;
        GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);
        health -= damageAmount;
        if (health <= 0)
        {
            HeroDie();
            
        }
        UpdateHealthBar();
        int i = Random.Range(0, ouchClips.Length);
        AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
    }

    private void HeroDie() 
    {
        Collider2D[]cols = GetComponents<Collider2D>();
        foreach (Collider2D col in cols)
        {
            col.isTrigger = true;
        }

        SpriteRenderer[] sprs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spr in sprs)
        {
            spr.sortingLayerName = "UI";
        }
        anim.SetTrigger("Dead");//播放死亡动画

        Transform gunTransform = transform.Find("Gun");

        if (gunTransform != null)
        {
            gunTransform.GetComponent<Gun>().enabled = false;
        }
    }

    void UpdateHealthBar() 
    {
        healthBar.material.color = Color.Lerp(Color.red, Color.green, health / 100f);
        healthBar.transform.localScale = new Vector3(health / 100f, 1, 1);

    }
}   
