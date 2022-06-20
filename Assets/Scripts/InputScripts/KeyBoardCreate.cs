using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardCreate : MonoBehaviour
{
    private GameObject Keyboard;
    private Text nickname;
    GameObject Nick;
    int queue = 0; //문자열 계산
    public static string name = "";

    void Start()
    {
        Keyboard = GameObject.Find("Keyboard");
        Keyboard.SetActive(false);
        nickname = GameObject.Find("U-Nickname").GetComponent<Text>();
    }

    void Update()
    {
        if (Nick)
        {
            Nick.GetComponent<Text>().text = name;
        }
        else
        {
        }
    }

    public void create()
    {
        Keyboard.SetActive(true);
        Nick = GameObject.Find("inputNick");
        queue = 0;
        name = "";
    }

    public void done() //  nick name 설정 후 keypad 비활성화
    {
        Keyboard.SetActive(false);
        Debug.Log("name=" + name);
        nickname.text = name;
    }
    public void retouch(string ex)
    {
        name = name.Substring(0, queue - 1) + ex;
    }
    public void add(string ex)
    {
        name += ex;
        queue++;
    }

    public void del()
    {
        queue--;
        name = name.Substring(0, queue);
    }
}
