using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RetryButton : MonoBehaviour
{
    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // 자기 자신 불러오기
    }
}
