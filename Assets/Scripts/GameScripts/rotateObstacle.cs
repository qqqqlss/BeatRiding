using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObstacle : MonoBehaviour
{
    //장애물 (돌멩이)회전
    private float xdegree;
    void Start()
    {
        xdegree = Random.Range(-5f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(new Vector3(xdegree, 0, 0));
    }
}
