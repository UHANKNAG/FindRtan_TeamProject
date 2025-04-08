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
        // �ν��Ͻ��� ���ٸ� �ڱ� �ڽ��� �־��� (�ٸ� ��ũ��Ʈ���� ���� ����)
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
        // �� ī���� ��ȣ�� ������
        if (firstCard.idx == secondCard.idx)
        {
            // ¦�� ���� �Ҹ� ���
            SoundManager.instance.PlaySFX(SoundManager.instance.matchSound);

            // ī�� ���ֱ�
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            cardCount -= 2;

            // ī�� �� ���߸� ���� ��
            if (cardCount == 0)
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {
            // ¦�� �ƴϸ� ī�� ����
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        // ���� �ʱ�ȭ
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


