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
