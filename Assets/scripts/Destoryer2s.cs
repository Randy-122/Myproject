using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Destoryer2s : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rd;
    void Start()
    {
        Destroy(rd.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
