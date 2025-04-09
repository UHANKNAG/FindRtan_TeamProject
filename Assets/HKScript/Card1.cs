using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;



public class Card1 : MonoBehaviour
{
    //    ī�� ���� �����
    public enum CardType { Nomal = 0, Heal, Joker};
    public CardType type = CardType.Nomal;

    public int idx = 0;
    public int count = 0;

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
        // //    Ư�� ������ ī�忡�� Ư���ɷ� ��ȣ �ο��ϱ�
        // if(idx == 3) type = CardType.Heal;
        // else if(idx == 4) type = CardType.Joker;

        // //    �ִ�� ������ �� �ִ� ��
        // count = 3;

        // //    ������Ʈ �ҷ�����
        // audioSource = GetComponent<AudioSource>();
        // btn = GetComponentInChildren<Button>();

        // //    �ʱ�ȭ ���
        // btn.enabled = true;
    }


    void Update()
    {
        // //    ���� ���� �������¶�� ������ ��� ��Ȱ��ȭ �ϱ�
        // if (GameManager1.instance.isOver) btn.enabled = false;

        // if (count <= 0)
        // //    ���� ī��Ʈ�� 0�̶��
        // {
        //     count = 0;
        //     countTxt.color = Color.red;
        //     back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
        // }

        // //    ���� ī��Ʈ ����(�ǽð�����)
        // countTxt.text = count.ToString();
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        if (GameManager1.instance.secondCard != null) return;

        // audioSource.PlayOneShot(clip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);
        
        if(GameManager1.instance.firstCard == null)
        {
            GameManager1.instance.firstCard = this;
        }
        else
        {
            GameManager1.instance.secondCard = this;
            GameManager1.instance.Matched();
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
