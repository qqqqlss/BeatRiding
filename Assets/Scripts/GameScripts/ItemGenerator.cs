using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemGenerator : MonoBehaviour
{
    //bpm이 클수록 빠르게. 그러나 큰 애들이 가장 쉬움
    public GameObject ObstaclePrefab;
    public GameObject CoinPrefab;
    float delta = 0;
    public float total;
    void start()
    {
        total = 0;
    }

    void default_music(float span,float speed, int ratio)
    {
        //float span = 0.8f; // item 생성 시간
        this.delta += Time.deltaTime;
        if (this.delta > span)
        {
            //float speed = -1.2f; // item speed 지정
            this.delta = 0;
            GameObject item;
            int dice = Random.Range(1, 11);
            if (dice <= ratio) // 1 / 5 확률
            {
                item = Instantiate(CoinPrefab) as GameObject;
                total += 1;
            }
            else // 4 / 5 확률
            {
                item = Instantiate(ObstaclePrefab) as GameObject;
            }
            int[] arr = { -4, -2, 0, 2, 4 }; // item 생성 위치 x 좌표 배열
            int x = arr[Random.Range(0, arr.Length)]; // 위의 배열에서 랜덤한 수 받기

            item.transform.position = new Vector3(x, 1, 80); // 랜덤한 x 좌표에서 아이템 생성
            item.GetComponent<ItemController>().comeSpeed = speed; // 작성한 speed 만큼 다가오기
        }
    }

    public void BPM_75() //Like that(music1)
    {
        default_music(0.8f, -1.2f, 5);
    }

    public void BPM_118() //Disco Knights(music2)
    {
        default_music(1.0f, -1.0f, 5);
    }

    public void BPM_120() //project-2-marioish(music3)
    {
        default_music(0.98f, -1.0f, 5);
    }

    public void BPM_72() //Roboskater(music4)
    {
        default_music(0.8f, -1.4f, 5);
    }

    public void BPM_75_H() //Like that(music1)
    {
        default_music(0.8f, -1.2f, 2);
    }

    public void BPM_118_H() //Disco Knights(music2)
    {
        default_music(1.0f, -1.0f, 2);
    }

    public void BPM_120_H() //project-2-marioish(music3)
    {
        default_music(0.98f, -1.0f, 2);
    }

    public void BPM_72_H() //Roboskater(music4)
    {
        default_music(0.8f, -1.4f, 2);
    }

    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "30") //Easy_Disco Knights
        {
            BPM_118();
        }
        else if (SceneManager.GetActiveScene().name == "10") //Easy_Like that
        {
            BPM_75();
        }
        else if (SceneManager.GetActiveScene().name == "40") //Easy_project-2-marioish
        {
            BPM_120();
        }
        else if (SceneManager.GetActiveScene().name == "20") //Easy_Roboskater
        {
            BPM_72();
        }
        else if (SceneManager.GetActiveScene().name == "31") //Hard_Disco Knights
        {
            BPM_118_H();
        }
        else if (SceneManager.GetActiveScene().name == "11") //Hard_Like that
        {
            BPM_75_H();
        }
        else if (SceneManager.GetActiveScene().name == "41") //Hard_project-2-marioish
        {
            BPM_120_H();
        }
        else if (SceneManager.GetActiveScene().name == "21") //Hard_Roboskater
        {
            BPM_72_H();
        }
    }
}