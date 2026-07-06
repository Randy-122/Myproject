using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Button button;
    public AudioMixer audioMixer;

    public TMPro.TMP_Text scoreText;
    public int score = 0;
    private bool bPause = false;
    void Start()
    {
        
        button.onClick.AddListener(Pauser);
        slider.onValueChanged.AddListener(OnValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Pauser() 
    {
        bPause = !bPause;
        if (bPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void OnValueChanged(float newValue) 
    {
        audioMixer.SetFloat("mainVolume", newValue);
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }
}
