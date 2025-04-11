using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class My_GameManager : MonoBehaviour
{
    public static My_GameManager instance;

    public My_Card firstCard;
    public My_Card secondCard;

    public Text timeTxt;
    public Text countTxt;

    
    public GameObject endTxt;
    public GameObject nextTxt;
    public GameObject teamInfo;

    public Image targetCardImage;

    public int count = 0;
    public int cardCount = 0;
    public int nextSceneIndex;
    float floatTime = 0.0f;

    public bool isOver = false;

    public List<int> correctOrder;
    public int currentIndex = 0;

    AudioSource audioSource;
    public AudioClip matchClip;

    private void Awake()
    {
        if(instance == null) instance = this;
        //    �ν��Ͻ� �Ҵ�
        
        audioSource = GetComponent<AudioSource>();
        //    ������Ʈ �ҷ�����

        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        teamInfo.SetActive(false);
    }

    void Start()
    {
        count = 20;

        countTxt.text = count.ToString();

        Time.timeScale = 1.0f;
        endTxt.SetActive(false);
        isOver = false;

        currentIndex = 0;
        correctOrder = Enumerable.Range(0, 10).OrderBy(x => Random.Range(0f, 1f)).ToList();


        UpdateOrderVisual();
    }

    void Update()
    {

    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            if (firstCard.idx == correctOrder[currentIndex])
            {
                audioSource.PlayOneShot(matchClip);

                firstCard.anim.SetBool("isMatched", true);  // 첫번째 카드의 애니메이터 파라미터 isMatched를 true로 바꿔준다.
                secondCard.anim.SetBool("isMatched", true); // 두번째 카드의 애니메이터 파라미터 isMatched를 true로 바꿔준다.
                Invoke("DestroyCard", 1f);               // 1초 후에 DestroyCard 함수를 호출한다.

                cardCount -= 2;

                currentIndex++;
          
                UpdateOrderVisual();

                if (cardCount == 0)
                {
                    Invoke("Victory", 1f);
                }
            }
            else
            {
                firstCard.CloseCard();
                secondCard.CloseCard();
                firstCard = null;
                secondCard = null;
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


    public void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }

    public void Victory() {
        if (nextSceneIndex > PlayerPrefs.GetInt("stageAt")) {
            PlayerPrefs.SetInt("stageAt", nextSceneIndex);
        }
        Time.timeScale = 0f;
        nextTxt.SetActive(true);
        teamInfo.SetActive(true);
    }


    void UpdateOrderVisual()
    {
        if (currentIndex <  correctOrder.Count)
        {
            int nextIdx = correctOrder[currentIndex];
            Sprite sprite = Resources.Load<Sprite>($"Sprite/team{nextIdx}");   
            
            if (targetCardImage != null && sprite != null)
            {
                targetCardImage.sprite = sprite;
                targetCardImage.enabled = true;
            }
        }
        else
        {
            if (targetCardImage != null)
            {
                targetCardImage.enabled = false; // Ŭ���� �� �̹��� ����
            }
        }

    }

    List<int> ShiffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
        return list;
    }
}
