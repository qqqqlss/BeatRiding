using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 맵 매칭(음악 번호)

public class SelectPlay : MonoBehaviour
{
    string selectedMusic = null;
    string selectedLevel = null;
    public static int musicNum;

    void Update()
    {
        if (selectedLevel != OnClickLevel.selectedLevelValue) //선택된 level 값 변동 
        {
            selectedLevel = OnClickLevel.selectedLevelValue;
            
            if (selectedMusic != MusicList.selectedMusicValue) //선택된 title 값 변동 
            {
                selectedMusic = MusicList.selectedMusicValue;
                StartCoroutine(GetMusicLevel());
            }
            else
            {
                selectedMusic = MusicList.selectedMusicValue;
                StartCoroutine(GetMusicLevel());
            }
        }
        else if (selectedMusic != MusicList.selectedMusicValue) //level 값 변동 없이, 선택된 title 값 변동 
        {
            selectedMusic = MusicList.selectedMusicValue;
            StartCoroutine(GetMusicLevel());
        }
    }

    IEnumerator GetMusicLevel() //음악 리스트(ListMusic)의 DB값 받아오기
    {
        if(selectedLevel == null || selectedMusic == null)
        {
            yield return 0;
        }
        WWWForm form = new WWWForm();
        form.AddField("musicPost", selectedMusic);
        form.AddField("levelPost", selectedLevel);

        WWW musicLevelData = new WWW("http://122.32.165.55/musicLevel_coex_D1.php", form);
        yield return musicLevelData;
        string musicLevelDataString = musicLevelData.text;
        Debug.Log(musicLevelDataString); //받아온 값 확인
        musicNum = int.Parse(musicLevelDataString);
    }
}
