using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public static SoundManager instance;

    private bool bgPause = true; //일시정지 확인
    private bool bgStop = true; //플레이 종료 확인

    private void Awake()
    {
        //씬이 변경되면 기존의 SoundManager를 파괴하고, 생성된 SoundManager가 없다면 생성한다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("SoundManager ON");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("SoundManager OFF");
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //씬 명에 따라 배경음 재생
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
            {
                BgSoundPlay(bglist[i]);

                //메인메뉴가 아닌 경우, 1회만 재생
                if (arg0.name != "MainMenu")
                {
                    bgSound.loop = false;
                    bgSound.playOnAwake = true;
                }
            }
        }
    }

    public void BgSoundPlay(AudioClip clip)
    {
        //메인 배경음 재생
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("MainBGSound")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();

        bgPause = false;
        bgStop = false;
    }

    public void BGSoundVolume(float val)
    {
        //mixer에서 지정한 볼륨설정(UI에서 사용)
        mixer.SetFloat("MainBGSound", Mathf.Log10(val) * 20);
    }

    public void BGSoundMute(bool check)
    {
        // 배경음 음소거(정지 X, 메인 메뉴에서 사용)
        if (bgSound.mute != false)
        {
            bgSound.mute = check; //음소거 해제
            return;
        }
        else
        {
            bgSound.mute = check; //음소거
            return;
        }
    }

    public bool BGSoundPause()
    {
        // 배경음 일시정지(게임 플레이에서 사용)
        if (bgPause == false) //재생중
        {
            bgSound.Pause(); //일시정지
            bgPause = true;
            return bgPause;
        }
        else //일시정지중
        {
            bgSound.Play(); //일시정지 해제
            bgPause = false;
            return bgPause;
        }
    }

    public bool BGSoundStop()
    {
        if (bgStop == false) //플레이 종료
        {
            bgSound.Stop();
            bgStop = true;
            return bgStop;
        }
        else //게임 다시 하기
        {
            bgSound.Play(); //재시작
            bgStop = false;
            return bgStop;
        }
    }
}
