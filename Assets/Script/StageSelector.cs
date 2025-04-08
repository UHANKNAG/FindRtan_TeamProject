using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelector : MonoBehaviour
{
    public Button[] StgButtons;

    void Start()
    {
        int stageAt = PlayerPrefs.GetInt("StageAt", 2);
        // Build Profiles에서 Stage 1 Scene이 2번이기 때문에

        // 도달하지 못한 스테이지 버튼 잠금 처리
        for (int i = 0; i < StgButtons.Length; i++) {
            if (i + 2 > stageAt)
                StgButtons[i].interactable = false;
        }
    }
}
