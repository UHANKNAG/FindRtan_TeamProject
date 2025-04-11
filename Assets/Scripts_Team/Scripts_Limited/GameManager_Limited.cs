using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamaManager_Limited : MonoBehaviour
{
    public static GamaManager_Limited instance;

    public Card_Limited firstCard;
    public Card_Limited secondCard;

    public Text timeTxt;
    public GameObject endTxt;
    public GameObject nextTxt;
    public GameObject teamInfo;

    public int nextSceneIndex;

    public int cardCount = 0;
    float floatTime = 0.0f;

    public bool isOver = false;

    AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        if(instance == null) instance = this;
        audioSource = GetComponent<AudioSource>();
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        teamInfo.SetActive(false);
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        endTxt.SetActive(false);
        nextTxt.SetActive(false);
        isOver = false;
    }

    void Update()
    {

    }

    public void Matched()
    {
        if(firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);

            firstCard.anim.SetBool("isMatched", true);  // 첫번째 카드의 애니메이터 파라미터 isMatched를 true로 바꿔준다.
            secondCard.anim.SetBool("isMatched", true); // 두번째 카드의 애니메이터 파라미터 isMatched를 true로 바꿔준다.
            Invoke("DestroyCard", 1f);               // 1초 후에 DestroyCard 함수를 호출한다.

            cardCount -= 2;


            if (cardCount == 0)
            {
                Invoke("Victory", 1f);
            }
        }
        else
        {
            firstCard.count--;
            secondCard.count--;

            firstCard.CloseCard();
            secondCard.CloseCard();
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

    public void Victory() {
        if (nextSceneIndex > PlayerPrefs.GetInt("stageAt")) {
            PlayerPrefs.SetInt("stageAt", nextSceneIndex);
        }
        Time.timeScale = 0f;
        nextTxt.SetActive(true);
        teamInfo.SetActive(true);
    }
    
    public void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }
}
