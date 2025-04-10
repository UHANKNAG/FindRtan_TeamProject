using Unity.Profiling;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSeletor : MonoBehaviour
{
    public Button button;
    public string stageNum;
    public Text stageText;

    void Start() {
        stageText = button.GetComponentInChildren<Text>();
        stageNum = stageText.text;
    }

    public void OpenScene() {
        SceneManager.LoadScene("Stage " + stageNum);
    }

}
