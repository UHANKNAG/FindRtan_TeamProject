using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NewGameManager : MonoBehaviour
{
    public static NewGameManager instance; //    싱글톤

    public NewCard firstCard; //    첫번째 카드
    public NewCard secondCard; //    두번째 카드

    public bool isProcessing = false;

    AudioSource audioSource;
    public AudioClip clip;

    public NewBoard board;

    private void Awake()
    {
        if (instance == null) instance = this;
        //    싱글톤 인스턴스 할당

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
        isProcessing = false;
    }

    void Update()
    {

    }

    public IEnumerator CheckMatchCoroutine()
    {
        isProcessing = true;

        //    소리 우선 재생
        if (firstCard.idx == secondCard.idx) audioSource.PlayOneShot(clip);

        // 딜레이는 애니메이션 기다리는 시간용
        yield return new WaitForSeconds(1.2f);
        
        if (firstCard.idx == secondCard.idx)
        {
            if (board != null)
            {
                board.ShuffleCards();
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;

        //    다시 카드 선택이 가능하게 만들기
        isProcessing = false; 
    }
}
