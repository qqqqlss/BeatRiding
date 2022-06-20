using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearInfo : MonoBehaviour
{
    //private PlayController player;
    public Text music;
    public Text score;
    public Text rank;
    public Text pname;

    public static bool insert;

    private int scoreNum = 0;
    private int musicNum = 0;
    private string nickname = "";
    private int rankNum;

    void Start()
    {
        insert = false;
        if (SceneManager.GetActiveScene().name == "InputName")
        {
            nickname = KeyBoardCreate.name;
            musicNum = SelectPlay.musicNum;
            scoreNum = (int)PlayController.score; //DB column이 int로 되어있기 때문에, 임시로 변환
            StartCoroutine(GetRanking(scoreNum));

            music.text = MusicList.selectedMusicValue;
            score.text = "점수:" + scoreNum;
            pname.text = KeyBoardCreate.name;
        }
        if (SceneManager.GetActiveScene().name == "InputName2")
        {
            nickname = KeyBoardCreate.name;
            musicNum = SelectPlay.musicNum;
            scoreNum = (int)PlayController.score; //DB column이 int로 되어있기 때문에, 임시로 변환
            StartCoroutine(GetRanking(scoreNum));

            music.text = MusicList.selectedMusicValue;
            score.text = "점수:" + scoreNum;
            pname.text = KeyBoardCreate.name;
        }
    }

    void Update()
    {
        /*
        if (nickname != KeyBoardCreate.name) //입력한 닉네임 가져오기
        {
            nickname = KeyBoardCreate.name;
        }
        */

        if (insert)
        {
            //Debug.Log(musicNum + " " + nickname + " " + scoreNum);
            Debug.Log("이름: "+nickname);
            if (SceneManager.GetActiveScene().name == "InputName") StartCoroutine(InsertRanking());
            insert = false;

            SceneManager.LoadScene("Scenes/MainMenu");
        }
    }

    IEnumerator GetRanking(int score) //현 플레이어의 랭킹 조회
    {
        WWWForm form = new WWWForm();
        form.AddField("cmd", "rank");
        form.AddField("numPost", musicNum);
        form.AddField("scorePost", score);

        WWW RankingData = new WWW("http://122.32.165.55/musicPlayer_coex_D1.php", form);
        yield return RankingData;
        string RankingDataString = RankingData.text;
        Debug.Log(RankingDataString); //받아온 값 확인
        rankNum = int.Parse(RankingDataString);

        //return 한 이후에 값을 저장하는 행위를 하기 때문에 start에서 사용하면 화면에 보여줄 수 없다.
        rank.text = "" + rankNum + "위";
    }

    public IEnumerator InsertRanking() //현 플레이어의 랭킹을 DB에 기록
    {
        if (nickname == "") //사용자의 닉네임이 없을 경우
        {
            nickname = "unknown";
        }

        WWWForm form = new WWWForm();
        form.AddField("cmd", "insert");
        form.AddField("numPost", musicNum);
        form.AddField("scorePost", scoreNum);
        form.AddField("namePost", nickname);

        WWW insertResult = new WWW("http://122.32.165.55/musicPlayer_coex_D1.php", form);
        yield return insertResult;
        string insertResultString = insertResult.text;
        Debug.Log(insertResultString); //받아온 값 확인
    }
}
