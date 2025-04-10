using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class My_Retry : MonoBehaviour
{
    public void RetryBtn()
    {
        SceneManager.LoadScene("LSY_Scene");
    }

    public void MyRetryBtn()
    {
        SceneManager.LoadScene("LJMScene");
    }

    public void RetryBtn_Limited()
    {
        SceneManager.LoadScene("LimitedFlipScene");
    }
}
