using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Explotion;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }
        if (Explotion != null)
        {
            Instantiate(Explotion, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 180))));
            Destroy(transform.gameObject);
        }
    }
}
