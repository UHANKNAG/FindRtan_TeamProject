using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    private int stageAt;
    public Button[] stgButtons;
    
    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
        
    }
    
    void Start() {
        stageAt = PlayerPrefs.GetInt("stageAt", 2);
        for (int i = 0; i < stgButtons.Length; i++) {
            if (i + 2 > stageAt)
                stgButtons[i].interactable = false;
        }
        // 현재 Stage가 Scene Num보다 작으면 접근 false
        Debug.Log(stageAt);
    }

    void Update()
    {
        

    }
}
