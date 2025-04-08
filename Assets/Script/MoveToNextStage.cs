using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextStage : MonoBehaviour
{
    public int nextSceneLoad;

    void Start()
    {
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;

    }
}
