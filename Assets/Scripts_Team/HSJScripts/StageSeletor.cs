using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSeletor : MonoBehaviour
{
    public Button button;
    public string stageNum;
    public Text stageText;

    void Start() {
        // Button에 있는 Text를 통해 Stage 전환이 이루어질 수 있도록 변수 설정
        stageText = button.GetComponentInChildren<Text>();
        stageNum = stageText.text;
    }

    public void OpenScene() {
        // 숫자에 따른 Stage Scene을 Load한다.
        SceneManager.LoadScene("Stage " + stageNum);
    }

}
