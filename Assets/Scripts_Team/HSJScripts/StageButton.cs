using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
        public void ToStage()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
