using System.Linq;
using UnityEngine;

public class NewBoard : MonoBehaviour
{
    public GameObject card;
    public int width = 4;
    public int height = 4;

    void Start()
    {
        int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        for (int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 4.0f;

            go.transform.position = new Vector2(x, y);
            go.GetComponent<NewCard>().Setting(arr[i]);
        }
    }

    public void ShuffleCards()
    {
        //    ��� ī�� ��������
        NewCard[] cards = GetComponentsInChildren<NewCard>();

        int pairCount = cards.Length / 2;
        int[] arr = new int[cards.Length];

        for (int i = 0; i < pairCount; i++)
        {
            arr[i * 2] = i;
            arr[i * 2 + 1] = i;
        }

        //    ī�� ����
        arr = arr.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        
        //    ī�� ���� �ݱ� �ִϸ��̼� ���� ���� ��Ű��
        foreach (NewCard card in cards)
        {
            card.ForceCloseImmediately();
        }

        //    idx, �̹��� �����ϱ�
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].Setting(arr[i]);
        }
    }
}
