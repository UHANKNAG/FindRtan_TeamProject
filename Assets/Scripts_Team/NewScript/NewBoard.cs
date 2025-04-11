using System.Linq;
using UnityEngine;

public class NewBoard : MonoBehaviour
{
    //    ī�� ���� ������Ʈ
    public GameObject card;
 
    void Start()
    {
        //    �� ���� �� ī�忡�� �ο��� idx�� �迭 ����
        int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        //    ������ ������� �������� ����
        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        //    ��� ī�带 ��ġ�� �°� ��ġ�ϱ�
        for (int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            //    x��ġ�� y��ġ ���ϱ�
            //    1.4 = ī�尣�� ����, -2.1 = ��ġ ����
            //    1.4 = ī�尣�� ����, -4.0 = ��ġ ����
            //    % ���� ��, / ���� ��

            //    1 ������ 4�� ���� �� ���� �������� 1
            //    ������ 1�� 4�� ���� �� ���⿡ �״�� ��������

            //    1 ������ 4�� �ϸ� 0
            //    4 ������ 4�� �ϸ� 1

            //    �׷� �� �̷��� �ɱ�?
            //    ���������� �������� �������� ������ ������ ����!
            //    �� 1������ 4 �Ѵٰ� 0.25�� ������ ����!

            //    ��� - ���� 7�� 4�� ������ �ϸ���
            //    �� = 1, ������ 3
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 4.0f;

            //    ���� ���Ŀ��� ������ x, y��ǥ���� �����ϱ�
            go.transform.position = new Vector2(x, y);

            //    �ε��� ��ȣ �ο��� ��������Ʈ �����ϱ�
            go.GetComponent<NewCard>().Setting(arr[i]);
        }
    }

    public void ShuffleCards()
    {
        //    ��� ī�� ��������
        NewCard[] cards = GetComponentsInChildren<NewCard>();

        //    ī���� ¦ ���ϱ�(�ʿ�)
        int pairCount = cards.Length / 2;

        //    ī���� �� ��ŭ �迭 ����
        int[] arr = new int[cards.Length];

        //    �� �迭���� �ε��� �Ҵ��ϱ�
        for (int i = 0; i < pairCount; i++)
        {
            //    ¦�� �ε��� �Ҵ�
            arr[i * 2] = i;

            //    Ȧ�� �ε��� �Ҵ�
            arr[i * 2 + 1] = i;

            //    �̷��� �ϸ� �۾��ӵ��� �������� ���� �� ����
            //    ��� Ȧ���� ���� ��찡 �ִ� ��쿡�� ���� �ʿ�
        }

        //    ������ ������� �������� ����
        arr = arr.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        
        //    ī�� ���� �ݱ� �ִϸ��̼� ���� ���� ��Ű��
        foreach (NewCard card in cards)
        {
            card.ForceCloseImmediately();
        }

        //    ��� ī�忡 idx, �̹��� �����ϱ�
        for (int i = 0; i < (cards.Length); i++)
        {
            cards[i].Setting(arr[i]);
        }
    }
}
