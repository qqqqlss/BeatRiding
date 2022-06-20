using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float comeSpeed = -0.3f;
   
    void FixedUpdate()
    {
        transform.Translate(0, 0, this.comeSpeed);//위치변경
        
        if (transform.position.z < -3.0f)
        {
            Destroy(gameObject);
        }
    }
}
