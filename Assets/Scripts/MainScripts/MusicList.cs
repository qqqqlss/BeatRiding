using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicList : MonoBehaviour
{
    //SelectedMusic->text
    public Text runTime;
    public Text bpm;
    public Text selectedMusic;

    int musicButton = 0; //Watchpoint로 입력받은 기존 값
    public static string selectedMusicValue = null;

    string[] music;

    void Start()
    {
        StartCoroutine(GetMusicItem());
    }

    void Update()
    {
        if(musicButton != Watchpoint.musicButton)
        {
            musicButton = Watchpoint.musicButton;
            SetMusicItem(musicButton); //출력 값 변경
        }
    }

    IEnumerator GetMusicItem() //음악 리스트(ListMusic)의 DB값 받아오기
    {
        WWWForm form = new WWWForm();

        WWW musicData = new WWW("http://122.32.165.55/musicList_coex_D1.php", form);
        yield return musicData;
        string musicDataString = musicData.text;
        print(musicDataString); //받아온 값 확인
        music = musicDataString.Split(';'); //세미콜론을 이용하여 각 음악을 분리하여 저장

        SetMusicItem(musicButton); //초기 출력 설정
    }

    public int SetMusicItem(int i) //메인 메뉴에 음악 리스트(ListMusic) 출력
    {
        if (i >= music.Length - 1) //마지막 배열 값 ""이므로 제외, 배열 크기를 초과할 경우 0으로 리셋
        {
            i = 0;
        }
        else if (i < 0) //index가 0이하일 경우 마지막 첫번째 값으로 설정
        {
            i = (music.Length / 3) * 3;
        }

        for(int a = 0; a < 3; a++)
        {
            if(i + a > music.Length - 1 || music[i + a] == "") //배열 크기 확인 및 값의 유무(존재 X)
            {
                //값 초기화
                Text title = GameObject.Find("title" + a).GetComponent<Text>();
                Text composer = GameObject.Find("composer" + a).GetComponent<Text>();
                Text info = GameObject.Find("info" + a).GetComponent<Text>();

                title.text = "";
                composer.text = "";
                info.text = "";
            }
            else
            {
                //값 지정
                Text title = GameObject.Find("title" + a).GetComponent<Text>();
                Text composer = GameObject.Find("composer" + a).GetComponent<Text>();
                Text info = GameObject.Find("info" + a).GetComponent<Text>();

                title.text = GetDataValue(music[i+a], "title:");
                composer.text = GetDataValue(music[i + a], "composer:");
                info.text = GetDataValue(music[i + a], "genre:");

                if (a == 0) //초기 선택된 음악(SelectedMusic) 출력 값 설정(첫 번째 아이템)
                {
                    runTime.text = GetDataValue(music[i], "runtime:");
                    bpm.text = GetDataValue(music[i], "bpm:");
                    selectedMusic.text = GetDataValue(music[i], "title:");
                    //PLAY에 전달할 초기 title(곡 명) 값 지정
                    selectedMusicValue = GetDataValue(music[i], "title:");
                }
            }
        }

        return i; //변경된 값 반환
    }

    //음악 리스트(ListMusic)의 아이템을 선택했을 경우, 선택된 음악(SelectedMusic)의 출력 값 변경
    public void OnClickMusicItem(int i)
    {
        runTime.text = GetDataValue(music[musicButton + i], "runtime:");
        bpm.text = GetDataValue(music[musicButton + i], "bpm:");
        selectedMusic.text = GetDataValue(music[musicButton + i], "title:");

        selectedMusicValue = GetDataValue(music[musicButton + i], "title:");
    }

    string GetDataValue(string data, string index1) //각 음악의 세부 정보 분리(곡 이름, 작곡가 등)
    {
        if (data.Equals(""))
        {
            return "";
        }else
        {
            string value = data.Substring(data.IndexOf(index1) + index1.Length);
            if (!(index1 == "bpm:")) //현 음악의 마지막 데이터
            {
                value = value.Remove(value.IndexOf("|")); //구분자 이후 제거()
            }
            return value;
        }
    }

}
