using System.Linq;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;

    void Start()
    {
        // 0부터 9까지 2번씩 넣어서 20장 카드 만들기
        int[] cardIds = Enumerable.Range(0, 10).SelectMany(i => new[] { i, i }).ToArray();

        // 카드 순서 섞기
        cardIds = cardIds.OrderBy(_ => Random.value).ToArray();

        // 카드 하나씩 생성해서 위치 배치하고 번호 설정
        for (int i = 0; i < 20; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, this.transform);

            float x = (i % 5) * 1.4f - 2.8f;
            float y = (i / 5) * -1.6f + 3.2f;
            cardObj.transform.position = new Vector2(x, y);

            cardObj.GetComponent<CardCtrl>().SetupCard(cardIds[i]);
        }

        GameSystem.instance.cardCount = cardIds.Length;
    }
}


