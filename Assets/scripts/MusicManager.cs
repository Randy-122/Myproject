using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicMixer : MonoBehaviour
{


    public AudioMixer audioMixer;


    private float savedVolume;
    private bool isPaused = false;
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


        if (Time.timeScale == 0 && !isPaused)
        {
            // 刚进入暂停状态：保存当前音量并静音
            audioMixer.GetFloat("mainVolume", out savedVolume);
            audioMixer.SetFloat("mainVolume", -80f); // -80 通常作为静音阈值
            isPaused = true;
        }
        else if (Time.timeScale != 0 && isPaused)
        {
            // 刚恢复游戏状态：将音量恢复为暂停前的值
            audioMixer.SetFloat("mainVolume", savedVolume);
            isPaused = false;
        }
    }
}
