using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundParralax : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] backGrounds;         //每层背景的tranform
    public float parallaxFactor = 0.2f;
    public float framesFactor = 0.2f;
    public float smoothX = 3f;
    private Transform camTran;
    private Vector3 camPrePos;
    void Start()
    {
        Debug.Log("123 called");
        camTran = Camera.main.transform;
        camPrePos = camTran.position;
    }

    // Update is called once per frame
    void Update()
    {
        float fparallax = (camPrePos.x - camTran.position.x) * parallaxFactor;  //相机运动方向与背景运动方向相反
        //计算各层的运动量
        for (int i = 0; i < backGrounds.Length; i++)
        {

            float bkNewX = backGrounds[i].position.x + fparallax * (1 + framesFactor * i);
            Vector3 bkNewPos = new Vector3(bkNewX, backGrounds[i].position.y, backGrounds[i].position.z);
            backGrounds[i].position = Vector3.Lerp(backGrounds[i].position, bkNewPos, smoothX * Time.deltaTime);

        }
        //当前帧相机位置赋值给上一帧相机位置
        camPrePos = camTran.position;

    }
}