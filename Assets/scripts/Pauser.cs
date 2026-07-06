using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    // Start is called before the first frame update
    public bool bPause = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Pause(bPause);
    }

    public void Pause(bool bPause) 
    {
        if (bPause)
        {
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1f;
        }
    
    }

    public void SetPause()
    {
        this.bPause = !bPause;
    }
}
