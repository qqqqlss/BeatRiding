using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Watchpoint : MonoBehaviour
{
    public SerialController serialController; // 아두이노 사용

    private Transform tr;
    private Ray ray;
    private RaycastHit hit;

    public float dist = 10.0f;

    private GameObject preGaze;//이전에 응시중이었던것
    private GameObject curGaze;//현재 응시중인것

    private KeyBoardCreate keyboard; //불러올 함수가 있는 스크립트
    int count = 100; //키보드 중복 확인

    //main
    private OptionUI option;
    float vol = 1f; //사운드 조절
    private MusicList music;
    public static int musicButton = 0;

    private Ranking rank;
    public static int rankButton = 0;

    private OnClickLevel level;

    //startInput화면
    private GameObject isRegister;

    void Awake()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        if (SceneManager.GetActiveScene().name == "InputName2"|| SceneManager.GetActiveScene().name == "InputName-start")
        {
            keyboard = GameObject.Find("Keyboard").GetComponent<KeyBoardCreate>();
        }
        else if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            music = GameObject.Find("MusicList").GetComponent<MusicList>();
            rank = GameObject.Find("RankInfo").GetComponent<Ranking>();
            level = GameObject.Find("Difficulty").GetComponent<OnClickLevel>();
            option = GameObject.Find("OptionGroup").GetComponent<OptionUI>();
        }
    }

    void Start()
    {
        tr = GetComponent<Transform>();//메인 카메라의 Transform컴포넌트를 추출
        if (SceneManager.GetActiveScene().name == "InputName-start")
        {
            isRegister = GameObject.Find("SetVRtxt");
            isRegister.SetActive(false);
        }
        
    }
   
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        //광선생성
        ray = new Ray(tr.position, tr.forward * dist);

        //광선을 씬뷰에 시각적으로 표시
        Debug.DrawRay(ray.origin, ray.direction * dist, Color.green);

        //충돌된 hit 검출하는 raycast
        if(Physics.Raycast(ray,out hit, dist))
        {
            GameObject target = hit.collider.gameObject;
            //Debug.Log("충돌함:" + target.name);
            CrossHair.isGaze = true;
            /*
            //test용 Z
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Search(target);
            }
            */
            if (message == null) //아두이노 사용
                return;
            // Check if the message is plain data or a connect/disconnect event.
            if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
                Debug.LogWarning("Connection established");
            else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
                Debug.LogWarning("Connection attempt failed or disconnection detected");
            else
            {
                Debug.Log("Message arrived: " + message);
                if (message=="R"||message=="L")
                {
                    Search(target);
                }
            }
            
        }
        else
        {
            CrossHair.isGaze = false;
        }

        //오브젝트의 응시여부
        CheckGaze();
    }

    void CheckGaze()
    {
        //포인터 이벤트 정보 추출
        PointerEventData data = new PointerEventData(EventSystem.current);

        //검출
        if(Physics.Raycast(ray,out hit, dist))
        {
            curGaze = hit.collider.gameObject;

            //이전 응시와 현재 응시 오브젝트가 다른겅우
            if (curGaze != preGaze)
            {
                //현재 오브젝트에 이벤트 전달
                ExecuteEvents.Execute(curGaze, data, ExecuteEvents.pointerEnterHandler);
                //이전 버튼에 PinterExit이벤트 전달
                ExecuteEvents.Execute(preGaze, data, ExecuteEvents.pointerExitHandler);
                //이전 오브젝트 정보 갱신
                preGaze = curGaze;
            }
        }
        else
        {
            //기존 오브젝트에서 벗어나 다른 오브젝트에 PointExit이벤트를 전달한다.
            if (preGaze != null)
            {
                ExecuteEvents.Execute(preGaze, data, ExecuteEvents.pointerExitHandler);
                preGaze = null;
            }
        }
    }

    void Search(GameObject target)
    {
        switch (target.name)
        {
            case "PlayButton":
                //게임 리스트(레벨선택->게임선택->그래야 게임시작가능)에서 선택할 수 있도록 해야함.
                Debug.Log("GO!");
                SceneManager.LoadScene("Scenes/Map/"+SelectPlay.musicNum);
                //SceneManager.LoadScene("Scenes/InputName"); //테스트 용
                break;
            case "easy":
                //레벨(EASY)
                level.OnClickEasy();
                break;
            case "hard":
                //레벨(HARD)
                level.OnClickHard();
                break;
            case "musicScrollUp":
                //MusicList의 출력 아이템 변경(앞->뒤)
                musicButton += 3;
                musicButton = music.SetMusicItem(musicButton);
                break;
            case "musicScrollDown":
                //MusicList의 출력 아이템 변경(뒤->앞)
                musicButton -= 3;
                musicButton = music.SetMusicItem(musicButton);
                break;
            case "item0":
                //첫 번째 노래 선택
                music.OnClickMusicItem(0);
                break;
            case "item1":
                //두 번째 노래 선택
                music.OnClickMusicItem(1);
                break;
            case "item2":
                //세 번째 노래 선택
                music.OnClickMusicItem(2);
                break;
            case "rankScrollUp":
                //MusicList의 출력 아이템 변경(앞->뒤)
                rankButton += 1;
                rankButton = rank.SetPlayer(rankButton);
                break;
            case "rankScrollDown":
                //MusicList의 출력 아이템 변경(뒤->앞)
                rankButton -= 1;
                rankButton = rank.SetPlayer(rankButton);
                break;
            case "ChangeNicknameBtn":
                break;
            case "soundUp":
                //배경음 볼륨 조절(+)
                if (vol < 2f)
                {
                    vol += 0.2f;
                    Debug.Log("sound: "+ vol); //test
                    option.SoundText(vol);
                }
                break;
            case "soundDown":
                //배경음 볼륨 조절(-)
                if (vol > 0f)
                {
                    vol -= 0.2f;
                    Debug.Log("sound: " + vol); //test
                    option.SoundText(vol);
                }
                break;
            case "soundO":
                //음소거 해제
                Debug.Log("sound: Mute OFF"); //test
                option.MuteUI(false);
                break;
            case "soundX":
                //음소거
                Debug.Log("sound: Mute ON"); //test
                option.MuteUI(true);
                break;
            case "vibrationDown":
                break;
            case "vibrationUp":
                break;
            //키보드-----------------------------

            case "InputButton": // 변경 버튼
                keyboard.create();
                break;
            case "ButtonEnter": // 완료
                keyboard.done();
                break;
            case "ButtonCancel": // 마지막 문자 지우기
                keyboard.del();
                break;
            case "Button (0)": // 기타 버튼
                count = 100;
                break;
            case "Button (1)": //ABC
                if (count == 1)
                {
                    keyboard.retouch("B");
                    count = 10;
                }
                else if (count == 10)
                {
                    keyboard.retouch("C");
                    count = 100;
                }
                else
                {
                    keyboard.add("A");
                    count = 1;
                }
                break;
            case "Button (2)": //DEF
                if (count == 2)
                {
                    keyboard.retouch("E");
                    count = 11;
                }
                else if (count == 11)
                {
                    keyboard.retouch("F");
                    count = 100;
                }
                else
                {
                    keyboard.add("D");
                    count = 2;
                }
                break;
            case "Button (3)": //GHI
                if (count == 3)
                {
                    keyboard.retouch("H");
                    count = 12;
                }
                else if (count == 12)
                {
                    keyboard.retouch("I");
                    count = 100;
                }
                else
                {
                    keyboard.add("G");
                    count = 3;
                }
                break;
            case "Button (4)": //JKL
                if (count == 4)
                {
                    keyboard.retouch("K");
                    count = 13;
                }
                else if (count == 13)
                {
                    keyboard.retouch("L");
                    count = 100;
                }
                else
                {
                    keyboard.add("J");
                    count = 4;
                }
                break;
            case "Button (5)": //MNO
                if (count == 5)
                {
                    keyboard.retouch("N");
                    count = 14;
                }
                else if (count == 14)
                {
                    keyboard.retouch("O");
                    count = 100;
                }
                else
                {
                    keyboard.add("M");
                    count = 5;
                }
                break;
            case "Button (6)": //PQR
                if (count == 6)
                {
                    keyboard.retouch("Q");
                    count = 15;
                }
                else if (count == 15)
                {
                    keyboard.retouch("R");
                    count = 100;
                }
                else
                {
                    keyboard.add("P");
                    count = 6;
                }
                break;
            case "Button (7)": //STU
                if (count == 7)
                {
                    keyboard.retouch("T");
                    count = 16;
                }
                else if (count == 16)
                {
                    keyboard.retouch("U");
                    count = 100;
                }
                else
                {
                    keyboard.add("S");
                    count = 7;
                }
                break;
            case "Button (8)": //VWX
                if (count == 8)
                {
                    keyboard.retouch("W");
                    count = 17;
                }
                else if (count == 17)
                {
                    keyboard.retouch("X");
                    count = 100;
                }
                else
                {
                    keyboard.add("V");
                    count = 8;
                }
                break;
            case "Button (9)": //YZ
                if (count == 9)
                {
                    keyboard.retouch("Z");
                    count = 100;
                }
                else
                {
                    keyboard.add("Y");
                    count = 9;
                }
                break;
            //---------------------키보드?--------
            case "Register":
                //DB에 사용자 정보 저장하기
                Invoke("DBinsert", 2);
                break;
            case "Register2":
                if (isRegister != null)
                {
                    Debug.Log("등록되었다는 메시지 없다!!");
                    isRegister.SetActive(true);
                }
                SceneManager.LoadScene("Scenes/MainMenu");
                break;
            case "None":
                //메인화면으로 돌아가기
                SceneManager.LoadScene("Scenes/MainMenu");
                break;
            case "quit":
                Debug.Log("build된 어플리케이션이 종료.");
                Application.Quit();
                break;
        }
    }
    void DBinsert()
    {
        ClearInfo.insert = true; //DB 입력 후, 씬 이동
    }
    
}
