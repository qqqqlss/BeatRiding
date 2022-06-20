using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//text
using UnityEngine.SceneManagement;//씬매니저

public class musicController : MonoBehaviour
{
    public SerialController serialController;
    public float velocity;

    //오디오
    private OptionUI music; //간접적인 음악 조절
    /*
    public AudioClip music;
    AudioSource aud;
    bool stop_ing;
    */

    //텍스트
    public GameObject pauseText;//정지텍스트
    public Text hpText;
    public Text gameOver;
    //게임오브젝트
    private PlayController player;
    private ItemGenerator item;
    private GameObject bar;
    //아두이노 인식조건
    bool isGameOver = false;
    bool isClear = false;
    //private Watchpoint watchpoint;
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>(); // 시리얼 컨트롤러 등록

        Debug.Log("Press A or Z to execute some actions");

        player = GameObject.FindWithTag("Player").GetComponent<PlayController>();
        item = this.GetComponent<ItemGenerator>();

        //오디오
        music = this.GetComponent<OptionUI>();
        /*
        this.aud = GetComponent<AudioSource>();
        aud.Stop();
        aud.clip = music;
        // 뮤트: true일 경우 소리가 나지 않음
        aud.mute = false;
        // 루핑: true일 경우 반복 재생
        aud.loop = false;
        // 자동 재생: true일 경우 자동 재생
        aud.playOnAwake = false;
        //재생
        aud.Play();
        stop_ing = false;
        */

        Time.timeScale = 1;//시간 정지
        //텍스트 안보이게
        pauseText.SetActive(false);
        //게임종료 바
        bar = GameObject.Find("MusicTimeOut");

        //클릭조건(isClick)을 가져오기 위함
        // watchpoint = GameObject.Find("Camera").GetComponent<Watchpoint>();
    }

    // Update is called once per frame
    void Update()
    {
        //hp UI
        hpText.GetComponent<Text>().text = "HP: " + player.hp + "/100";

        //게임오버
        if (player.hp <= 0)
        {
            GameOver();
            isGameOver = true;
        }

        //클리어
        if (bar.transform.position.y <= 0.5f) //막대 두께때문에 충돌시간이 이르다:.5f
        {
            clearEvent();
            isClear = true;
        }

        string message = serialController.ReadSerialMessage(); // 시리얼 메시지 읽기

        if (message == null)
      //    return;

        
        //test
        if (Input.GetKeyDown(KeyCode.Z))
        {
            BackGroundMusicOffButton();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameOver();
            SceneManager.LoadScene("Scenes/MainMenu");//나가기
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            clearEvent();
            SceneManager.LoadScene("Scenes/Map/10"); //다시하기
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            clearEvent();
            SceneManager.LoadScene("Scenes/InputName"); //결과
        }
        

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("music Connection attempt failed or disconnection detected");
        else
        {

            Debug.Log("music Message arrived: " + message); // 읽은 메시지 체크 << 메시지에대한 처리는 여기에다가 추가.
            switch (message)
            {
                case "L":   // L메시지 왔을 때 (아두이노에서 왼쪽 버튼 눌렀을 때)
                    Debug.Log("Left Button");
                    //BackGroundMusicOffButton();//여기로 옮겨야하지 않을까
                    break;
                case "R":    // R메시지 왔을 때 (아두이노에서 오른쪽 버튼 눌렀을 때)
                    Debug.Log("Right Button");
                    //watchpoint.isClick = true;
                    break;
                case "W":   // 왼쪽 방향 
                    player.Leftmove();
                    break;
                case "E":    // 오른쪽 방향
                    player.RightMove();
                    break;
            }

            //일시정지
            if (message == "L")
            {
                BackGroundMusicOffButton();
            }

            //클리어
            if (isClear == true) //막대 두께때문에 충돌시간이 이르다:.5f
            {
                if (message == "L") SceneManager.LoadScene("Scenes/Map/" + SelectPlay.musicNum);//재시작
                else if (message == "R")
                {
                    SceneManager.LoadScene("Scenes/InputName2");//랭킹등록
                }
            }

            //게임오버
            if (isGameOver == true)
            {
                if (message == "L") SceneManager.LoadScene("Scenes/Map/" + SelectPlay.musicNum);//재시작
                else if (message == "R") SceneManager.LoadScene("Scenes/MainMenu");//나가기
            }

        }

        string[] aa = message.Split(new char[] { '|' });
        if (aa[0] == "V")
        {
            velocity = System.Convert.ToSingle(aa[1]);
            Debug.Log("V=>" + velocity);
            //속도에 따른 점수 부여
            PlayController.score += velocity / 10;
        }

        /*
         * [임시용]
        //게임오버
        if (player.hp <= 0)
        {
            GameOver();
            if (message == "L") SceneManager.LoadScene("Scenes/Map/" + SelectPlay.musicNum);//재시작
            else if (message == "R") SceneManager.LoadScene("Scenes/MainMenu");//나가기
        }
        */
    }

    // [ContextMenu("clear")]//-->막대가 하늘에 있어서 이걸로 멈춰도 일시정지가 된다.
    void clearEvent()
    {
        //aud.Stop();
        music.StopUI(); //노래 종료

        Time.timeScale = 0;//시간 정지
        gameOver.GetComponent<Text>().text = "클리어\n점수:" + PlayController.score + "\n" + clear() + "\n좌클릭:재시작/우클릭:점수등록";

    }

    void GameOver()
    {
        music.StopUI(); //노래 종료

        gameOver.GetComponent<Text>().text = "게임오버\n좌클릭:재시작/우클릭: 나가기";
        Time.timeScale = 0;//시간 정지
    }

    //테스트 완료(동작잘됨)
    public void BackGroundMusicOffButton() //배경음악 키고 끄는 버튼
    {
        //aud = GetComponent<AudioSource>(); //배경음악 저장해둠
        if (music.PauseUI()) //aud.isPlaying
        {
            //aud.Pause();
            //stop_ing = true;
            pauseText.SetActive(true);
            Time.timeScale = 0;//시간 정지
        }
        else
        {
            //stop_ing = false;
            Time.timeScale = 1;//시간 정상
            pauseText.SetActive(false);
            //aud.Play();
        }
    }

    public string clear()
    {
        //Debug.Log("score " + player.score);//스코어는 *10을 해둠.
        //Debug.Log("itemtotal" + item.total);//전체갯수는 *1상태
        Debug.Log("점수" + PlayController.score / item.total / 10);

        if ((PlayController.score / item.total) == 1)
        {
            Debug.Log("S");
            return "S";
        }
        else if ((PlayController.score / item.total) >= 0.9)
        {
            Debug.Log("A");
            return "A";
        }
        else if ((PlayController.score / item.total) >= 0.8)
        {
            Debug.Log("B");
            return "B";
        }
        else if ((PlayController.score / item.total) >= 0.7)
        {
            Debug.Log("C");
            return "C";
        }
        else if ((PlayController.score / item.total) >= 0.6)
        {
            Debug.Log("D");
            return "D";
        }
        else
        {
            Debug.Log("E");
            return "E";
        }
    }

}
