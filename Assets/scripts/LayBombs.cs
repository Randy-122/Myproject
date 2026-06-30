using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LayBombs : MonoBehaviour
{
	[HideInInspector]
	public bool bombLaid = false;		
	public int bombCount = 0;			
	public AudioClip bombsAway;			
	public GameObject bomb;

	//[SerializeField]
	private Text bombHUD;		


	void Awake ()
	{
		
		//bombHUD = GameObject.Find("ui_bombHUD").GetComponent<Text>();
	}


	void Update ()
	{
		// 释放炸弹
		if(Input.GetButtonDown("Fire2") && !bombLaid && bombCount > 0)
		{
			bombCount--;
			bombLaid = true;
			AudioSource.PlayClipAtPoint(bombsAway,transform.position);
			Instantiate(bomb, transform.position, transform.rotation); //实例化炸弹
		}
	}
}
