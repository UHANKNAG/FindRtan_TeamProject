using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public static GameSystem instance;

    public CardCtrl firstCard;
    public CardCtrl secondCard;

    public GameObject endText;

    public int cardCount = 0;
    public bool isOver = false;

    private void Awake()
    {
        // 인스턴스가 없다면 자기 자신을 넣어줌 (다른 스크립트에서 쉽게 접근)
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        isOver = false;
        endText.SetActive(false);
    }

    public void CheckMatch()
    {
        // 두 카드의 번호가 같으면
        if (firstCard.idx == secondCard.idx)
        {
            // 짝이 맞은 소리 재생
            SoundManager.instance.PlaySFX(SoundManager.instance.matchSound);

            // 카드 없애기
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            cardCount -= 2;

            // 카드 다 맞추면 게임 끝
            if (cardCount == 0)
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {
            // 짝이 아니면 카드 덮기
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        // 선택 초기화
        firstCard = null;
        secondCard = null;
    }

    void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endText.SetActive(true);
    }
}


