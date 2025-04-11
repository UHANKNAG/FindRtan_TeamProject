using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Card firstCard;
    public Card secondCard;

    AudioSource audioSource;
    public AudioClip clip;

    public Text timeTxt;
    public GameObject endTxt;
    public GameObject nextTxt;
    public GameObject teamInfo;

    public int nextSceneIndex;
    public int cardCount = 0;

    float time = 60.0f;


    private void Awake() {
        time = 60.0f;
        if (instance == null)
            instance = this;
        Time.timeScale = 1.0f;
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        teamInfo.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        Debug.Log(nextSceneIndex);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time <= 0.0f) {
            GameOver();
        }
    }

    public void Matched() {

        if (firstCard.idx == secondCard.idx) {
            audioSource.PlayOneShot(clip);

            firstCard.anim.SetBool("isMatched", true);  // 첫번째 카드의 애니메이터 파라미터 isMatched를 true로 바꿔준다.
            secondCard.anim.SetBool("isMatched", true); // 두번째 카드의 애니메이터 파라미터 isMatched를 true로 바꿔준다.
            Invoke("DestroyCard", 1f);               // 1초 후에 DestroyCard 함수를 호출한다.


            cardCount -= 2;

            if (cardCount == 0) {
                Invoke("Victory", 0.5f);
            }
        }
        else 
        {
            firstCard.ClosedCard();
            secondCard.ClosedCard();
            firstCard = null;
            secondCard = null;
        }


    }


    public void DestroyCard()
    {
        firstCard.DestroyCard();
        secondCard.DestroyCard();
        firstCard = null;
        secondCard = null;
    }


    public void GameOver()
    {
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }

    public void Victory() {
        if (nextSceneIndex > PlayerPrefs.GetInt("stageAt")) {
            PlayerPrefs.SetInt("stageAt", nextSceneIndex);
        }
        Time.timeScale = 0f;
        teamInfo.SetActive(true);
        nextTxt.SetActive(true);
    }
}
