using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;



public class My_Card : MonoBehaviour
{
    public int idx; // 카드 고유 번호 (짝을 찾기 위한 기준)
    public int count = 2; // 선택 가능 횟수 (기본 2회)

    public GameObject front;
    public GameObject back;

    public Animator anim;

    public SpriteRenderer frontImage;

    public Button btn;

    AudioSource audioSource;
    public AudioClip clip;


    void Start()
    {
        //    컴포넌트 불러오기
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    초기화 목록
        btn.enabled = true;
    }


    void Update()
    {
        //    만약 게임 오버상태라면 뒤집기 기능 비활성화 하기
        if (My_GameManager.instance.isOver) btn.enabled = false;

        if (count <= 0)
        //    만약 카운트가 0이라면
        {
            count = 0;
            back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
        }
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        if (My_GameManager.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);
        
        if(My_GameManager.instance.firstCard == null)
        {
            My_GameManager.instance.firstCard = this;
        }
        else
        {
            My_GameManager.instance.secondCard = this;
            My_GameManager.instance.Matched();
        }
    }
    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 1f);
    }

    public void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1f);
    }

    public void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }
}
