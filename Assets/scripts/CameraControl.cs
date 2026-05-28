using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Player;
    public float SmoothX = 5f;
    public float SmoothY = 5f;

    public Vector2 MinCamXandY = new Vector2(-10f, -5f);
    public Vector2 MaxCamXandY = new Vector2(10f, 5f);

    void Awake() 
    {
        Player = GameObject.Find("Hero").transform;
        if (Player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                Player = playerObj.transform;
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void LateUpdate()
    {
        if (Player == null) return;

        TrackPlayer();
    }

    void TrackPlayer()
    {
        float CamNewX = transform.position.x;
        float CamNewY = transform.position.y;

        if (NeedMoveX())
        {
            CamNewX = Mathf.Lerp(transform.position.x, Player.position.x, SmoothX *  Time.deltaTime);
        }

        if (NeedMoveY()) 
        {
            CamNewY = Mathf.Lerp(transform.position.y, Player.position.y, SmoothY * Time.deltaTime);
        }
        CamNewX = Mathf.Clamp(CamNewX, MinCamXandY.x, MaxCamXandY.x);
        CamNewY = Mathf.Clamp(CamNewY, MinCamXandY.y, MaxCamXandY.y);

        transform.position = new Vector3(CamNewX, CamNewY, transform.position.z);
    }

    bool NeedMoveX()
    {
        return Mathf.Abs(Player.position.x - transform.position.x) > 0.01f;
    }
    bool NeedMoveY()
    {
        return Mathf.Abs(Player.position.y - transform.position.y) > 0.01f;
    }
}
