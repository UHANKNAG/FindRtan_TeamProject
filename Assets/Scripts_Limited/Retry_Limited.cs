using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Retry_Limited : MonoBehaviour
{
    public void RetryBtn_Limited()
    {
        SceneManager.LoadScene("LimitedFlipScene");
    }
}
