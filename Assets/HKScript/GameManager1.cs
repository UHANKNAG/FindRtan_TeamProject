using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;

    public Card1 firstCard; //    ù��° ī��
    public Card1 secondCard; //    �ι�° ī��

    public Text timeTxt; //    �ð� �ؽ�Ʈ
    public Text countTxt; //    ���� ���� Ƚ�� �ؽ�Ʈ
    public GameObject endTxt; //    ���ӿ��� �ؽ�Ʈ

    public int count = 0;
    public int cardCount = 0;
    float floatTime = 0.0f;

    public bool isOver = false;

    AudioSource audioSource;
    public AudioClip clip;

    // HK 선언
    bool victoryGame = false;
    int nextSceneLoad;


    private void Awake()
    {
        if(instance == null) instance = this;
        //    �ν��Ͻ� �Ҵ�
        
        // audioSource = GetComponent<AudioSource>();
        //    ������Ʈ �ҷ�����

        Time.timeScale = 1.0f;
    }

    void Start()
    {
        count = 20;
        //    ��ü ī���� ����

        countTxt.text = count.ToString();
        //    ���� Ƚ�� ī��Ʈ 

        
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
        //    ù��°�� ���� ī��� �ι�°�� ���� ī�尡 ��ġ�Ѵٸ�
        {

            // audioSource.PlayOneShot(clip);
            //    ���� ���� ���� ���

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            //    ���� ī�� ����

            cardCount -= 2;
            //    ���� ī���� �� ����

            // if (firstCard.type == Card1.CardType.Heal)
            // {
            //     //    ���� �ִ� �ð��� �ø��ٸ� �ִ� �ð��� ����ȭ ��Ų��
            //     //    �׸��� ���� �ܼ���� ��� �ؾ����� ������ �ؾ���
            // }
            // if (firstCard.type == Card1.CardType.Joker)
            // {
            //     Debug.Log("��Ŀī���� ¦�� ���������ϴ�!");
            //     //    ��Ŀī���� ���
            // }


            if (cardCount == 0)
            //    ���� ��� ī�带 ����ٸ�
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {
            // firstCard.count--;
            // secondCard.count--;
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
        // Time.timeScale = 0f;
        endTxt.SetActive(true);
        MoveToNextStage();
    }

    void MoveToNextStage() {
        // 마지막 Scene 경우 게임 끝내기 추가할 것
        
        if (victoryGame) {
            SceneManager.LoadScene(nextSceneLoad);

            if (nextSceneLoad > PlayerPrefs.GetInt("StageAt")) {
                PlayerPrefs.SetInt("StageAt", nextSceneLoad);
            }
        }
        Debug.Log("Move to Next Stage");
    }
}
