using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class My_Retry : MonoBehaviour
{
    public void RetryBtn()
    {
        SceneManager.LoadScene("MainScene1");
    }
}
