using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rocket;
    public float speed = 15;
    [SerializeField] public PlayerControl playerCtrl;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCtrl != null)
        {
            if (Input.GetButtonDown("Fire1"))
            //if(Input.GetKeyDown(KeyCode.mouse0))
            {
                if (playerCtrl.bFaceRight)
                {
                    Rigidbody2D rocketInstance = Instantiate(rocket, transform.position
                                                    , Quaternion.Euler(new Vector3(0, 0, 0)));
                    rocketInstance.velocity = new Vector2(speed, 0);
                }
                else
                {
                    Rigidbody2D rocketInstance = Instantiate(rocket, transform.position
                                                    , Quaternion.Euler(new Vector3(0, 0, 180)));
                    rocketInstance.velocity = new Vector2(-speed, 0);
                }

            }
        }
    }
}
