using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public float speed = 0f;//= 0.51f; //맵이 이동하는 속도=페달속도
    private musicController musicCon;
    private PlayController player;

    void Start()
    {
        musicCon = GameObject.Find("GameDirector").GetComponent<musicController>();
    }

    void FixedUpdate()
    {
        speed = musicCon.velocity/40;
        //맵 뒤로 이동
        GoBack(speed);
    }

    void GoBack(float speed)
    {
        transform.position += new Vector3(0, 0, -speed);
        if (-50 >= transform.position.z)
        {
            transform.position = new Vector3(0, 0, 250.0f);
        }
    }
}
