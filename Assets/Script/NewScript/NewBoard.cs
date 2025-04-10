using System.Linq;
using UnityEngine;

public class NewBoard : MonoBehaviour
{
    //    카드 게임 오브젝트
    public GameObject card;
 
    void Start()
    {
        //    각 시작 시 카드에게 부여할 idx들 배열 생성
        int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };

        //    기준을 섞어버려 랜덤으로 섞기
        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        //    모든 카드를 위치에 맞게 배치하기
        for (int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            //    x위치와 y위치 구하기
            //    1.4 = 카드간의 간격, -2.1 = 위치 조정
            //    1.4 = 카드간의 간격, -4.0 = 위치 조정
            //    % 가로 행, / 세로 행

            //    1 나누기 4를 했을 때 남는 나머지는 1
            //    이유는 1로 4를 나눌 수 없기에 그대로 나머지행

            //    1 나누기 4를 하면 0
            //    4 나누기 4를 하면 1

            //    그럼 왜 이렇게 될까?
            //    정수끼리의 나눗샘과 나머지는 정수형 까지만 구함!
            //    즉 1나누기 4 한다고 0.25가 나오지 않음!

            //    결론 - 만약 7을 4로 나눈다 하면은
            //    몫 = 1, 나머지 3
            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 4.0f;

            //    위의 계산식에서 구해진 x, y좌표값에 생성하기
            go.transform.position = new Vector2(x, y);

            //    인덱스 번호 부여와 스프라이트 적용하기
            go.GetComponent<NewCard>().Setting(arr[i]);
        }
    }

    public void ShuffleCards()
    {
        //    모든 카드 가져오기
        NewCard[] cards = GetComponentsInChildren<NewCard>();

        //    카드의 짝 구하기(필요)
        int pairCount = cards.Length / 2;

        //    카드의 수 만큼 배열 형성
        int[] arr = new int[cards.Length];

        //    각 배열마다 인덱스 할당하기
        for (int i = 0; i < pairCount; i++)
        {
            //    짝수 인덱스 할당
            arr[i * 2] = i;

            //    홀수 인덱스 할당
            arr[i * 2 + 1] = i;

            //    이렇게 하면 작업속도를 절반으로 낮출 수 있음
            //    대신 홀수가 나올 경우가 있는 경우에는 수정 필요
        }

        //    기준을 섞어버려 랜덤으로 섞기
        arr = arr.OrderBy(x => Random.Range(0f, 100f)).ToArray();
        
        //    카드 전부 닫기 애니메이션 먼저 실행 시키기
        foreach (NewCard card in cards)
        {
            card.ForceCloseImmediately();
        }

        //    모든 카드에 idx, 이미지 갱신하기
        for (int i = 0; i < (cards.Length); i++)
        {
            cards[i].Setting(arr[i]);
        }
    }
}
