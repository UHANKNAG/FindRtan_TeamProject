using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;



public class My_Card : MonoBehaviour
{
    public int idx; // ī�� ���� ��ȣ (¦�� ã�� ���� ����)
    public int count = 2; // ���� ���� Ƚ�� (�⺻ 2ȸ)

    public GameObject front;
    public GameObject back;

    public Animator anim;

    public SpriteRenderer frontImage;

    public Button btn;

    AudioSource audioSource;
    public AudioClip clip;


    void Start()
    {
        //    ������Ʈ �ҷ�����
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    �ʱ�ȭ ���
        btn.enabled = true;
        if (btn != null)
        {
            btn.onClick.AddListener(OpenCard);
        }
    }


    void Update()
    {
        if (My_GameManager.instance.isOver) 
            btn.enabled = false;
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Sprite/Team{idx}");
    }

    public void OpenCard()
    {
        if (My_GameManager.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);

        // 이 과정은 CardFlip 함수를 통해 구현하였다.
        /*
        front.SetActive(true);
        back.SetActive(false);
        */

        if (My_GameManager.instance.firstCard == null)
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

    public void CardFlip()           // 유니티에서 Card 프리펫에 붙어있는 Flip 애니메이션이 실행되는 중, 90도 회전한 순간 front를 비활성화하고 back을 활성화하는 함수
    {
        front.SetActive(true);       // 90도 회전한 순간 Card의 front와 back의 상태를 바꿔 마치 카드가 뒤집힌 것처럼 보이게 한다
        back.SetActive(false);
    }

    public void CardReverseFlip()    // 유니티에서 Card 프리펫에 붙어있는 ReverseFlip 애니메이션이 실행되는 중, 90도 회전한 순간 back을 비활성화하고 front을 활성화하는 함수
    {
        front.SetActive(false);
        back.SetActive(true);
    }

}
