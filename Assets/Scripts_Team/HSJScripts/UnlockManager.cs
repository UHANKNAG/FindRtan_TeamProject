using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    private int stageAt;    // 현재 Stage Scene Number를 담을 변수
    public Button[] stgButtons; // 버튼 배열열
    
    void Start() {
        stageAt = PlayerPrefs.GetInt("stageAt", 2);
        // PlayerPrefs를 통해 stageAt 키를 불러온다
        // 키에 저장된 값이 없을 경우 2를 반환한다
        // 현재 stageAt에 2가 저장

        for (int i = 0; i < stgButtons.Length; i++) {
            if (i + 2 > stageAt)
                stgButtons[i].interactable = false;
        }
        // 현재 Stage가 Scene Num보다 작으면 접근 false

        Debug.Log(stageAt);
    }
}
