using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class My_Board : MonoBehaviour
{
    public GameObject card;

    void Start()
    {
        int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
        arr = arr.OrderBy(x => Random.Range(0f, 1f)).ToArray();
        
        for(int i = 0; i < 20; i++)
        {
            GameObject go =  Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.8f;

            go.transform.position = new Vector2(x, y);
            go.GetComponent<My_Card>().Setting(arr[i]);
        }

        My_GameManager.instance.cardCount = arr.Length;
    }

}
