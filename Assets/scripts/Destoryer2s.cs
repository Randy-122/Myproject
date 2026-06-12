using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Destoryer2s : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rd;
    void Start()
    {
        if (rd == null) 
        {
            return;
        }
        Destroy(rd.gameObject, 2f);
    }

    public void myDestory ()
    {
        Destroy(transform.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
