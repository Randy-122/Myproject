using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    Transform heroTransform;
    public Vector3 offset = new Vector3(0, 1.5f, 0);


    void Start()
    {
        heroTransform = GameObject.Find("Hero").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = heroTransform.position + offset;
    }
}
