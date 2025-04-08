using System.Linq;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;

    void Start()
    {
        // 0���� 9���� 2���� �־ 20�� ī�� �����
        int[] cardIds = Enumerable.Range(0, 10).SelectMany(i => new[] { i, i }).ToArray();

        // ī�� ���� ����
        cardIds = cardIds.OrderBy(_ => Random.value).ToArray();

        // ī�� �ϳ��� �����ؼ� ��ġ ��ġ�ϰ� ��ȣ ����
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


