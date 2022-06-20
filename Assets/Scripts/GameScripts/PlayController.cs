using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class PlayController : MonoBehaviour
{
    public SerialController serialController;

    public float hp;
    public static float score;
    public float size;
    private int combo;
    private Animator comboAnimator;
    public Text scoreText;
    public Text comboText;
    bool isKeyDown = false;
    public GameObject frontHp;//hp
    RectTransform rectTran;//hp bar
    float x=0;

    void Start()
    {
        hp = 100;
        score = 0;
        combo = 0;
        size = 700;

        scoreText = GameObject.Find("scoreText").GetComponent<Text>();
        comboText = GameObject.Find("comboText").GetComponent<Text>();

        scoreText.text = "점수 : " + score;

        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        rectTran = frontHp.GetComponent<RectTransform>();
    }
  
    public void Leftmove()
    {
        if (x > -3){
            transform.position = new Vector3(x - 2, 5, -2);
            x = transform.position.x;
        }
    }
    public void RightMove()
    {
        if (x < 3){ 
            transform.position = new Vector3(x + 2, 5, -2);
            x = transform.position.x;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        int SceneNum = Convert.ToInt32(SceneManager.GetActiveScene().name);
        if (other.gameObject.tag == "item")
        {
            if (SceneNum % 10 == 0)
            {
                score += 500;
                scoreText.text = "점수 : " + score;
                Debug.Log("아이템을 먹음!");
                combo += 1;

                if (combo >= 2)
                {
                    comboText.text = "COMBO " + combo.ToString();
                    score += 50;
                }
            }
            else
            {
                score += 500;
                scoreText.text = "점수 : " + score;
                Debug.Log("아이템을 먹음!");
                combo += 1;

                if (combo >= 2)
                {
                    comboText.text = "COMBO " + combo.ToString();
                    score += 100;
                }

            }

            /*else if (combo <= 10)
            {
                comboText.text = "COMBO " + combo.ToString();
                score += 600;
            }
            else
            {
                comboText.text = "COMBO " + combo.ToString();
                score += 800;
            }
            */
        }
        if (other.gameObject.tag == "obstacle")
        {
            hp -= 10;
            Debug.Log("충돌함");
            serialController.SendSerialMessage("Z");
            size -= 70f;
            combo = 0;
            comboText.text = " ";


            rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        }
    }
}
