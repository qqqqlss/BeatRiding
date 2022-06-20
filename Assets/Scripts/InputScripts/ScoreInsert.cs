using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//게임 종료 후, insert하는 cs
public class ScoreInsert : MonoBehaviour
{
    // 버튼
    public Button btnIn, btnEnd;
    // 닉네임 입력
    public InputField username;
    // 노래제목 + 난이도 받아오기
    public Text info;
    //public Text musicTitle;
    //public Button EASY;
    //public Button HARD;

    // sql
    dbconn mysqlDB = new dbconn();
    // 게임 오브젝트
    private musicController music;

    void Start()
    {
        music = GameObject.FindWithTag("Music").GetComponent<musicController>(); // 태그명이 Music인지는 아직 확인하지 못했으나 임의로 지정함.

        btnIn = this.transform.GetComponent<Button>();
        btnEnd = this.transform.GetComponent<Button>();
        // 게임 시작할 때 난이도 설정 클릭하는 버튼 객체 불러오기
        //EASY = this.textMessage.GetComponent<Button>(); 
        //HARD = this.textMessage.GetCoponent<Button>();
        //리스너 지정
        btnIn.onClick.AddListener(OnClickUpdate);
        btnEnd.onClick.AddListener(RetryGame);
    }

    public void OnClickUpdate()
    {
        if(info.name == "Like that EASY")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (10, username.text, player.score)");
        } else if(info.name == "Like that HARD")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (11, username.text, player.score)");
        } else if(info.name == "Roboskater EASY")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (20, username.text, player.score)");
        } else if(info.name == "Roboskater HARD")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (21, username.text, player.score)");
        } else if(info.name == "Disco Knights EASY")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (30, username.text, player.score)");
        } else if(info.name == "Disco Knights HARD")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (31, username.text, player.score)");
        } else if(info.name == "Project-2-marioish EASY")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (40, username.text, player.score)");
        } else if(info.name == "Project-2-marioish HARD")
        {
            mysqlDB.sqlcmdall("INSERT INTO `player`(`musicnum`, `username`, `score`) VALUES (41, username.text, player.score)");
        } else
        {
            Debug.Log("INSERT ERROR!!!\n");
        }
        
    }
    public void RetryGame()
    {
        Application.Quit(); //게임종료 및 재시작 버튼 적용 함수
        Debug.Log("게임 재시작");
    }
}

//스크립트 파일 button에 적용하고 On Click()부분에 맞는 함수 적용시켜주면 돼용
