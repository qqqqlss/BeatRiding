using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//MusicTimeOut에 들어있음
public class CalorieMath : MonoBehaviour
{
    musicController musicCon;
    public float personWeight;//몸무게kg
    public float bikeWeight;//자전거무게kg
    public float how_wait;//얼마간격으로 속도 체크할까?
    float speed;//총 평균속도km/h
    float time;//주행시간(초)==노래시간데이터베이스에서 가지고오기
    float x;//평균속도에 따른 칼로리 소모량 지수
    float kcal;//소모된 칼로리
    public Text kcalText;
    bool isChecking;//체크 가능 여부
    IEnumerator speedMeasure;
    void Start()
    {
        speed = 0;
        isChecking = true;
        personWeight = 50f; //50kg가정
        bikeWeight = 10f;//10kg
        time = 184f;//노래시간이 184초
        how_wait = 1f;
        this.transform.position = new Vector3(0, time, 0);
        kcalText.GetComponent<Text>().text = "";
        //칼로리 계산
        speedMeasure = SpeedMeasure();
        StartCoroutine(speedMeasure);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(0, -1f * Time.deltaTime, 0);//아래로 내려오는 중...
        //다 내려온 것은 musicController의 clearEvent()에서 호출
    }

    IEnumerator SpeedMeasure()
    {
        float MapSpeed;// 맵스피드 평균속도(리드스위치로 가지고 들어옴)
        
        float gap = time / how_wait;//=how_wait를 얼마나 기다려야 하는가@1
        Debug.Log("gap" + gap);
        //gap==36.8 그러나 끝나면 시간이 멈추므로 35초에서 끝남=소수점무시하고 -1만큼에 종료된다.
        gap -=4;//마지막 4초는 측정불가.
        for (float tick=0f;tick<gap;tick++)//그럼 그만큼 기다려보면 끝나겠다.@3
        {
            MapSpeed = GameObject.Find("Track1").GetComponent<MapController>().speed+15f;//0.51f
            Debug.Log("MapSpeed" + MapSpeed);
            yield return new WaitForSeconds(how_wait);//@2
            speed += MapSpeed;
            //Debug.Log("tick" + tick);
        }

        speed /= gap;//스피드 평균값
        Debug.Log("speed평균값" + speed);
        x=susic(speed);
        Debug.Log("ischecking" + isChecking);
        
        if (isChecking == true)
        {
            kcal = (personWeight + bikeWeight)  * x * time/60;//분이어야함
            Debug.Log("kcal" + kcal);
            kcalText.GetComponent<Text>().text = "소비된 kcal: " + kcal;
        }
        else
        {
            kcal = (personWeight + bikeWeight) * x * time/60;//분이어야함
            kcalText.GetComponent<Text>().text = "소비된 kcal: " + kcal+"측정불가(너무 빨랐거나, 너무 느림)";
        }
        
    }


    float susic(float speed)
    {
        float fixdiff = 1.6f;//평균속도간격
        if ((12.9 - fixdiff <= speed) && (12.9 + fixdiff > speed))
        {
            x = 0.065f;
        }
        else if ((16.1 - fixdiff <= speed) && (16.1 + fixdiff > speed))
        {
            x = 0.0783f;
        }
        else if ((19.3 - fixdiff <= speed) && (19.3 + fixdiff > speed))
        {
            x = 0.0939f;
        }
        else if ((22.5 - fixdiff <= speed) && (22.5 + fixdiff > speed))
        {
            x = 0.1129f;
        }
        else if ((24.1 - fixdiff <= speed) && (24.1 + fixdiff > speed))
        {
            x = 0.1237f;
        }
        else if ((25.7 - fixdiff <= speed) && (25.7 + fixdiff > speed))
        {
            x = 0.1356f;
        }
        else if ((27.4 - fixdiff <= speed) && (27.4 + fixdiff > speed))
        {
            x = 0.1488f;
        }
        else if ((29 - fixdiff <= speed) && (29 + fixdiff > speed))
        {
            x = 0.1631f;
        }
        else if ((30.6 - fixdiff <= speed) && (30.6 + fixdiff > speed))
        {
            x = 0.1788f;
        }
        else if ((32.2 - fixdiff <= speed) && (32.2 + fixdiff > speed))
        {
            x = 0.1964f;
        }
        else if ((33.8 - fixdiff <= speed) && (33.8 + fixdiff > speed))
        {
            x = 0.215f;
        }
        else if ((37 - fixdiff <= speed) && (37 + fixdiff > speed))
        {
            x = 0.2586f;
        }
        else if ((40.2 - fixdiff <= speed) && (40.2 + fixdiff > speed))
        {
            x = 0.3111f;
        }
        else
        {
            Debug.Log("TOO FAST or TOO LAST");
            x = 0f;//안움직이거나 고장으로 판단.
            this.isChecking = false;
        }
        Debug.Log("x" + x);
        return x;
    }
}
