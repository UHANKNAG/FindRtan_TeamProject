using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween ���̺귯�� �ʿ�



public class Card : MonoBehaviour
{

    public int idx = 0;


    public Text countTxt;
    public GameObject front;
    public GameObject back;

    public Animator anim;

    public SpriteRenderer frontImage;


    public Button btn;

    AudioSource audioSource;
    public AudioClip clip;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    �ʱ�ȭ ���
        btn.enabled = true;
    }


    void Update()
    {
        //    ���� ���� �������¶�� ������ ��� ��Ȱ��ȭ �ϱ�
        if (GameManager.instance.isOver) btn.enabled = false;
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        if (GameManager.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        if (GameManager.instance.firstCard == null)
        {
            GameManager.instance.firstCard = this;
        }
        else
        {
            GameManager.instance.secondCard = this;
            GameManager.instance.Matched();
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
