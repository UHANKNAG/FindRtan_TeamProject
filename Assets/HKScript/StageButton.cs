using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public Button button;
    public string stageNum;
    public Text stageText;


    void Start()
    {
        stageText = button.GetComponentInChildren<Text>();
        stageNum = stageText.text;
    }

    public void OpenScene() {
        SceneManager.LoadScene("Stage " + stageNum);
    }
}
