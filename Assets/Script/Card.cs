using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween ���̺귯�� �ʿ�



public class Card : MonoBehaviour
{
    //    ī�� ���� �����
    public enum CardType { Nomal = 0, Heal, Joker};
    public CardType type = CardType.Nomal;

    public int idx = 0;
    public int count = 0;

    private bool isFlipped = false;

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
        //    Ư�� ������ ī�忡�� Ư���ɷ� ��ȣ �ο��ϱ�
        if(idx == 3) type = CardType.Heal;
        else if(idx == 4) type = CardType.Joker;

        //    �ִ�� ������ �� �ִ� ��
        count = 3;

        //    ������Ʈ �ҷ�����
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    �ʱ�ȭ ���
        btn.enabled = true;
    }


    void Update()
    {
        //    ���� ���� �������¶�� ������ ��� ��Ȱ��ȭ �ϱ�
        if (GameManager.instance.isOver) btn.enabled = false;

        if (count <= 0)
        //    ���� ī��Ʈ�� 0�̶��
        {
            count = 0;
            countTxt.color = Color.red;
            back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
        }

        //    ���� ī��Ʈ ����(�ǽð�����)
        countTxt.text = count.ToString();
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

    public void FlipCard()
    {
        float targetRotationX = isFlipped ? 0f : 180f;
        transform.DORotate(new Vector3(targetRotationX, 0f, 0f), 0.5f); // �ε巯�� ȸ�� (0.5��)
        isFlipped = !isFlipped; // ���� ����
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
