using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StealController : MonoBehaviour
{
    public SerialController serialController;

    public float hp;
    public static float score;
    public float size;
    bool isKeyDown = false;
    public GameObject frontHp;//hp
    RectTransform rectTran;//hp bar
    float x = 0;

    void Start()
    {
        hp = 100;
        score = 0;
        size = 700;

        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        rectTran = frontHp.GetComponent<RectTransform>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "item")
        {
            Debug.Log("아이템을 못 먹음!");
            if (score < 300)
            {
                hp -= 10;
                size -= 70f;

                rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            }
            else
            {
                score -= 300;
            }
        }
    }
}
