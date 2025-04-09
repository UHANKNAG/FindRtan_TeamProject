using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retryj : MonoBehaviour
{
    public void RetryBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
}
