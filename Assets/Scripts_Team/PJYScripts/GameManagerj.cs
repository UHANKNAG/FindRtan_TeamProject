using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerj : MonoBehaviour
{
    public static GameManagerj instance;

    public Cardj firstCard;  // 첫 번째 뒤집힌 카드
    public Cardj secondCard; // 두 번째 뒤집힌 카드

    public Text timeTxt;    // 시간 표시용 텍스트
    public GameObject endTxt; // 게임 종료 시 표시될 텍스트(패널)

    public int cardCount = 0;  // 남은 카드 수
    float floatTime = 0.0f;    // 경과 시간

    public bool isOver = false;

    AudioSource audioSource;
    public AudioClip clip;

    // [추가] 이벤트 카드 매칭 시 표시할 패널
    [Header("이벤트 카드 UI")]
    public GameObject eventPanel;

    private void Awake()
    {
        // 싱글턴 할당
        if (instance == null) instance = this;

        // 오디오 소스 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {

        Time.timeScale = 1.0f;
        endTxt.SetActive(false);
        isOver = false;

        // [추가] 이벤트 패널이 존재한다면 초기에는 비활성화
        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }
    }

    void Update()
    {
        // (예시) 테스트용 코드: 시간 측정
        // floatTime += Time.deltaTime;

        if (floatTime >= 30f)
        {
            // 30초가 넘으면 게임 오버 처리
            Time.timeScale = 0f;
            endTxt.SetActive(true);
            isOver = true;
        }

        timeTxt.text = floatTime.ToString("N2"); // 소수점 둘째 자리까지 표시
    }

    /// <summary>
    /// 두 장의 카드가 뒤집혔을 때 호출 (매칭 판정)
    /// </summary>
    public void Matched()
    {
        // idx가 같다면 -> 매칭 성공
        if (firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);

            // 두 카드 제거
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            // 남아 있는 전체 카드 수 감소
            cardCount -= 2;

            // [추가] 이벤트 카드인지 확인 - 
            //       "마지막에 이벤트 패널이 뜨지 않도록" cardCount가 남아있을 때만 표시
            if (cardCount > 0)
            {
                if (firstCard.isEventCard && secondCard.isEventCard)
                {
                    Debug.Log("이벤트 카드 매칭 성공!");
                    ShowEventPanel();
                }
            }

            // 모든 카드가 사라졌으면 게임 종료
            if (cardCount == 0)
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {

            // 다시 뒷면으로
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        // 다음 비교를 위해 두 카드 정보 초기화
        firstCard = null;
        secondCard = null;
    }

    void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);

        // 혹시 모를 패널 잔여 표시를 방지하기 위해 이벤트 패널 확실히 비활성화
        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }
    }

    // [추가] 이벤트 패널 표시
    public void ShowEventPanel()
    {
        if (eventPanel != null)
        {
            eventPanel.SetActive(true);
        }
    }

    // [선택] 일정 시간 후 숨기는 기능이 필요하다면 추가
    public void HideEventPanel()
    {
        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }
    }
}
