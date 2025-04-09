using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boardj : MonoBehaviour
{
    public GameObject card;

    void Start()
    {
        // 뒷면에 놓일 카드 인덱스들(8쌍, 총 16장)
        int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3,
                      4, 4, 5, 5, 6, 6, 7, 7 };

        // 배열을 랜덤으로 섞기
        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        // [추가] 0~7 중 무작위로 한 쌍을 '이벤트 카드'로 지정
        int eventPairIndex = Random.Range(0, 8);
        Debug.Log("이벤트 카드 인덱스: " + eventPairIndex);

        // 16개의 카드를 생성하여 배치
        for(int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;

            go.transform.position = new Vector2(x, y);

            Cardj c = go.GetComponent<Cardj>();
            c.Setting(arr[i]); // 카드 인덱스 설정

            // [추가] 해당 카드가 이벤트 쌍에 속하면 isEventCard = true
            if (arr[i] == eventPairIndex)
            {
                c.isEventCard = true;
            }
        }

        // 생성된 카드 수를 GameManager에 전달
        GameManagerj.instance.cardCount = arr.Length;
    }
}
