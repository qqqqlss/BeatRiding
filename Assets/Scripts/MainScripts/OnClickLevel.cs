using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickLevel : MonoBehaviour
{
    public static string selectedLevelValue = null;
    public Image easyImage;
    public Text easyText;
    public Image hardImage;
    public Text hardText;

    void Start()
    {
        selectedLevelValue = "EASY";
    }

    public void OnClickEasy()
    {
        selectedLevelValue = "EASY";
        easyText.color = new Color(38 / 255f, 35 / 255f, 60 / 255f);
        easyImage.color = new Color(1f, 1f, 1f);
        hardText.color = new Color(1f, 1f, 1f);
        hardImage.color = new Color(38 / 255f, 35 / 255f, 60 / 255f);
    }

    public void OnClickHard()
    {
        selectedLevelValue = "HARD";
        easyText.color = new Color(1f, 1f, 1f);
        easyImage.color = new Color(38 / 255f, 35 / 255f, 60 / 255f);
        hardText.color = new Color(38 / 255f, 35 / 255f, 60 / 255f);
        hardImage.color = new Color(1f, 1f, 1f);
    }
}
