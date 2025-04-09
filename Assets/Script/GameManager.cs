using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Card firstCard; //    첫번째 카드
    public Card secondCard; //    두번째 카드

    public Text timeTxt; //    시간 텍스트
    public GameObject endTxt; //    게임오버 텍스트

    public int cardCount = 0;
    float floatTime = 0.0f;

    public bool isOver = false;

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
        Time.timeScale = 1.0f;
        //    시간 설정
        //    0 - 아이에 멈춤
        //    0.5 - 시간이 절반정도 느리게 흘러감
        //    1 - 시간이 동일하게 흘러감


        //    초기화 목록
        endTxt.SetActive(false);
        isOver = false;
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
        if(firstCard.idx == secondCard.idx)
        //    첫번째에 고른 카드와 두번째에 고른 카드가 일치한다면
        {

            audioSource.PlayOneShot(clip);
            //    정답 사운드 파일 재생

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            //    맞춘 카드 삭제

            cardCount -= 2;
            //    남은 카드의 수 감소


            if (cardCount == 0)
            //    만약 모든 카드를 맞췄다면
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {
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
}
