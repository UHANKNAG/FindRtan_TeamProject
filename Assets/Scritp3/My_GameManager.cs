using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class My_GameManager : MonoBehaviour
{
    public static My_GameManager instance;

    public My_Card firstCard; //    첫번째 카드
    public My_Card secondCard; //    두번째 카드

    public Text timeTxt; //    시간 텍스트
    public Text countTxt; //    남은 선택 횟수 텍스트
    public Text orderTxt; // 맞춰야 할 순서 텍스트
    
    public GameObject endTxt; //    게임오버 텍스트

    public Image targetCardImage; // 맞춰야 할 카드 이미지

    public int count = 0;
    public int cardCount = 0;
    float floatTime = 0.0f;

    public bool isOver = false;

    public List<int> correctOrder; // 맞춰야 할 카드 순서
    public int currentIndex = 0; // 현재 순서

    AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        if(instance == null) instance = this;
        //    인스턴스 할당
        
        audioSource = GetComponent<AudioSource>();
        //    컴포넌트 불러오기
    }

    void Start()
    {
        count = 20;
        //    전체 카드의 갯수

        countTxt.text = count.ToString();
        //    남은 횟수 카운트 

        Time.timeScale = 1.0f;
        //    시간 설정
        //    0 - 아이에 멈춤
        //    0.5 - 시간이 절반정도 느리게 흘러감
        //    1 - 시간이 동일하게 흘러감


        //    초기화 목록
        endTxt.SetActive(false);
        isOver = false;

        currentIndex = 0;

        correctOrder = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
        
        // Linq를 사용한 셔플 (OrderBy + Random)
        correctOrder = correctOrder.OrderBy(x => Random.Range(0f, 1f)).ToList();

        UpdateOrderText();
        UpdateOrderVisual();
    }

    void Update()
    {
        //    테스트를 위해 시간 비활성화
        //floatTime += Time.deltaTime;


        if(floatTime >= 30f)
        //    만약 제한 시간이 끝난다면
        {
            Time.timeScale = 0f;
            //    만약 새로운 문구를 넣어서 만들고 싶다면
            //    유니티에서 시간초과 텍스트를 만들고 가져온 뒤
            //    아래에 endTxt가 아닌 gameoverTxt를 따로 만들어서
            //    만들면 시간 초과로 게임오버했음을 알릴 수 있다.
            endTxt.SetActive(true);

            //    게임 종료 후 카드 선택을 방지하기 위한 조치
            isOver=true;
        }
        timeTxt.text = floatTime.ToString("N2");
        //    소숫점의 2자리까지만 띄우기
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        //    첫번째에 고른 카드와 두번째에 고른 카드가 일치한다면
        {
            // 순서 제한 모드 체크
            if (firstCard.idx == correctOrder[currentIndex])
            {
                audioSource.PlayOneShot(clip);
                //    정답 사운드 파일 재생

                firstCard.DestroyCard();
                secondCard.DestroyCard();
                //    맞춘 카드 삭제

                cardCount -= 2;
                //    남은 카드의 수 감소

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
                // 순서가 틀리면 다시 뒤집기
                firstCard.CloseCard();
                secondCard.CloseCard();
            }
        }
        else
        {
            firstCard.count--;
            secondCard.count--;
            //    선택한 두 카드의 남은 선택 횟수 감소

            firstCard.CloseCard();
            secondCard.CloseCard();
            //    카드 다시 덮기
        }

        firstCard = null;
        secondCard = null;
        //    선택된 카드 해제하기
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
            orderTxt.text = $"맞춰야 할 카드 : {correctOrder[currentIndex]}";
        }
        else
        {
            orderTxt.text = "게임 클리어";
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
                targetCardImage.enabled = false; // 클리어 시 이미지 숨김
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
