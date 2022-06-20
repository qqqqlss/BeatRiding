using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{   
    //선택했을때 조준점 색상 변경///미사용


    private Transform tr;
    //조준점 이미지를 연결할 변수
    private Image crossHair;

    //레티클의 초기 색상
    private Color originColor = Color.red;
    //응시했을때 레티클 색상
    public Color gazeColor = Color.green;

    //응시여부 결정변수
    public static bool isGaze = false;

    void Start()
    {
        tr = GetComponent<Transform>();
        crossHair = GetComponent<Image>();
        crossHair.color = originColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGaze)
        {
            //레티클 색상변경
            crossHair.color = gazeColor;
        }
        else
        {
            //최초색상으로 환원
            crossHair.color = originColor;

        }
    }
}
