using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicMixer : MonoBehaviour
{


    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (audioMixer != null) 
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) 
            {
                float volume;
                audioMixer.GetFloat("mainVolume", out volume);
                volume -= 10;
                volume = Mathf.Clamp(volume, -80 , 20);
                audioMixer.SetFloat("mainVolume", volume);
            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                float volume;
                audioMixer.GetFloat("mainVolume", out volume);
                volume += 10;
                volume = Mathf.Clamp(volume, -80, 20);
                audioMixer.SetFloat("mainVolume", volume);
            }   

        }
    }
}
