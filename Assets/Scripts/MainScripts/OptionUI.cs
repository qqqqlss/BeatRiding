using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    private SoundManager volume;
    private bool mute = false;
    private float volumeSize = 0.1f;

    private void Awake()
    {
        volume = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    public void SoundText(float vol)
    {
        if(volumeSize != vol) //사운드 조절 존재
        {
            Text t = GameObject.Find("sound").GetComponent<Text>();
            t.text = "" + (int)(50 * vol) + "%";

            volume.BGSoundVolume(vol);
        }
    }

    public void MuteUI(bool check)
    {
        if (check)//mute on
        {
            //추가 UI 설정
            volume.BGSoundMute(true);
        }
        else //mute off
        {
            //추가 UI 설정
            volume.BGSoundMute(false);
        }
    }
    public bool PauseUI()
    {
        return volume.BGSoundPause();
    }
    public bool StopUI()
    {
        return volume.BGSoundStop();
    }
}
