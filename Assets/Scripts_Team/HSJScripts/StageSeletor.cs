using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSeletor : MonoBehaviour
{
    public Button[] stgButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int stageAt = PlayerPrefs.GetInt("stageAt", 2);
        // PlayerPrefs 임시적인 데이터 저장

        for (int i = 0; i < stgButtons.Length; i++) {
            if (i + 2 > stageAt)
                stgButtons[i].interactable = false;
        }
        // 현재 Stage가 Scene Num보다 작으면 접근 false
    }

}
