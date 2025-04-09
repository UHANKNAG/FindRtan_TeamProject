using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry1 : MonoBehaviour
{
    public void RetryBtn()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
