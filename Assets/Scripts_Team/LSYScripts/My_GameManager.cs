using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class My_GameManager : MonoBehaviour
{
    public static My_GameManager instance;

    public My_Card firstCard;
    public My_Card secondCard;

    public Text timeTxt;
    public Text countTxt;

    
    public GameObject endTxt;

    public Image targetCardImage;

    public int count = 0;
    public int cardCount = 0;
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

                firstCard.DestroyCard();
                secondCard.DestroyCard();

                cardCount -= 2;

                currentIndex++;
          
                UpdateOrderVisual();

                if (cardCount == 0)
                {
                    Invoke("GameOver", 1f);
                }
            }
            else
            {
                firstCard.CloseCard();
                secondCard.CloseCard();
            }
        }
        else
        {
            firstCard.count--;
            secondCard.count--;

            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }

    void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);
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
