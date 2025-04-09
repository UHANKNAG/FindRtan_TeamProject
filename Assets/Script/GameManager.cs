using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Card firstCard; //    ù��° ī��
    public Card secondCard; //    �ι�° ī��

    public Text timeTxt; //    �ð� �ؽ�Ʈ
    public GameObject endTxt; //    ���ӿ��� �ؽ�Ʈ

    public int cardCount = 0;
    float floatTime = 0.0f;

    public bool isOver = false;

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
        Time.timeScale = 1.0f;
        //    �ð� ����
        //    0 - ���̿� ����
        //    0.5 - �ð��� �������� ������ �귯��
        //    1 - �ð��� �����ϰ� �귯��


        //    �ʱ�ȭ ���
        endTxt.SetActive(false);
        isOver = false;
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
        if(firstCard.idx == secondCard.idx)
        //    ù��°�� �� ī��� �ι�°�� �� ī�尡 ��ġ�Ѵٸ�
        {

            audioSource.PlayOneShot(clip);
            //    ���� ���� ���� ���

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            //    ���� ī�� ����

            cardCount -= 2;
            //    ���� ī���� �� ����


            if (cardCount == 0)
            //    ���� ��� ī�带 ����ٸ�
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {
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
}
