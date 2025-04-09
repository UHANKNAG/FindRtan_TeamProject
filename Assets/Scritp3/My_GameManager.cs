using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class My_GameManager : MonoBehaviour
{
    public static My_GameManager instance;

    public My_Card firstCard; //    ù��° ī��
    public My_Card secondCard; //    �ι�° ī��

    public Text timeTxt; //    �ð� �ؽ�Ʈ
    public Text countTxt; //    ���� ���� Ƚ�� �ؽ�Ʈ
    public Text orderTxt; // ����� �� ���� �ؽ�Ʈ
    
    public GameObject endTxt; //    ���ӿ��� �ؽ�Ʈ

    public Image targetCardImage; // ����� �� ī�� �̹���

    public int count = 0;
    public int cardCount = 0;
    float floatTime = 0.0f;

    public bool isOver = false;

    public List<int> correctOrder; // ����� �� ī�� ����
    public int currentIndex = 0; // ���� ����

    AudioSource audioSource;
    public AudioClip clip;

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
        //    ��ü ī���� ����

        countTxt.text = count.ToString();
        //    ���� Ƚ�� ī��Ʈ 

        Time.timeScale = 1.0f;
        //    �ð� ����
        //    0 - ���̿� ����
        //    0.5 - �ð��� �������� ������ �귯��
        //    1 - �ð��� �����ϰ� �귯��


        //    �ʱ�ȭ ���
        endTxt.SetActive(false);
        isOver = false;

        currentIndex = 0;

        correctOrder = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
        
        // Linq�� ����� ���� (OrderBy + Random)
        correctOrder = correctOrder.OrderBy(x => Random.Range(0f, 1f)).ToList();

        UpdateOrderText();
        UpdateOrderVisual();
    }

    void Update()
    {
        //    �׽�Ʈ�� ���� �ð� ��Ȱ��ȭ
        //floatTime += Time.deltaTime;


        if(floatTime >= 30f)
        //    ���� ���� �ð��� �����ٸ�
        {
            Time.timeScale = 0f;
            //    ���� ���ο� ������ �־ ����� �ʹٸ�
            //    ����Ƽ���� �ð��ʰ� �ؽ�Ʈ�� ����� ������ ��
            //    �Ʒ��� endTxt�� �ƴ� gameoverTxt�� ���� ����
            //    ����� �ð� �ʰ��� ���ӿ��������� �˸� �� �ִ�.
            endTxt.SetActive(true);

            //    ���� ���� �� ī�� ������ �����ϱ� ���� ��ġ
            isOver=true;
        }
        timeTxt.text = floatTime.ToString("N2");
        //    �Ҽ����� 2�ڸ������� ����
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        //    ù��°�� �� ī��� �ι�°�� �� ī�尡 ��ġ�Ѵٸ�
        {
            // ���� ���� ��� üũ
            if (firstCard.idx == correctOrder[currentIndex])
            {
                audioSource.PlayOneShot(clip);
                //    ���� ���� ���� ���

                firstCard.DestroyCard();
                secondCard.DestroyCard();
                //    ���� ī�� ����

                cardCount -= 2;
                //    ���� ī���� �� ����

                currentIndex++;
                UpdateOrderText();
                UpdateOrderVisual();

                if (cardCount == 0)
                {
                    Invoke("GameOver", 1f);
                }
            }
            else
            {
                // ������ Ʋ���� �ٽ� ������
                firstCard.CloseCard();
                secondCard.CloseCard();
            }
        }
        else
        {
            firstCard.count--;
            secondCard.count--;
            //    ������ �� ī���� ���� ���� Ƚ�� ����

            firstCard.CloseCard();
            secondCard.CloseCard();
            //    ī�� �ٽ� ����
        }

        firstCard = null;
        secondCard = null;
        //    ���õ� ī�� �����ϱ�
    }

    void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }

    void UpdateOrderText()
    {
        if (currentIndex < correctOrder.Count)
        {
            orderTxt.text = $"����� �� ī�� : {correctOrder[currentIndex]}";
        }
        else
        {
            orderTxt.text = "���� Ŭ����";
        }
    }

    void UpdateOrderVisual()
    {
        if (currentIndex <  correctOrder.Count)
        {
            int nextIdx = correctOrder[currentIndex];
            Sprite sprite = Resources.Load<Sprite>($"rtan{nextIdx}");
            
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
