using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public GameObject card;

    // Start is called before the first frame update
    void Start()
    {
        int[] arr = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();
        // OrderBy 오름차순 정렬한다, x => 하나씩 순회한다, Random.Range로 추출된 값을, ToArray 배열로 만들어 준다
        // 랜덤으로 값을 불러 arr 배열에 있는 값이랑 순서대로 매칭시킨다.
        // 그 후 OrderBy를 통해 랜덤한 값을 기준으로 정렬한 뒤
        // ToArray를 통해 원래 arr 배열에 있던 값만 다시 배열에 저장한다

        // 반복 생성을 위하여 반복문 호출
        for (int i = 0; i < 16; i++) {
            GameObject go = Instantiate(card, this.transform); // Board의 자식으로 생성

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;
            // 몫과 나머지를 이용한 배치 전략

            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);
        }

        GameManager.instance.cardCount = arr.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
