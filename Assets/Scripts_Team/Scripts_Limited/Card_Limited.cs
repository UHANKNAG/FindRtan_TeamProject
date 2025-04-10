using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;



public class Card_Limited : MonoBehaviour
{

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
        if (GamaManager_Limited.instance.isOver) btn.enabled = false;

        if (count <= 0)
        //    ���� ī��Ʈ�� 0�̶��
        {
            count = 0;
            countTxt.color = Color.red;
            back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);


            Invoke("GameEnd", 1f);
  

        }

        //    ���� ī��Ʈ ����(�ǽð�����)
        countTxt.text = count.ToString();
    }

    public void GameEnd()
    {
        Time.timeScale = 0f;
        GamaManager_Limited.instance.endTxt.SetActive(true);
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Sprite/Team{idx}");
    }

    public void OpenCard()
    {
        if (GamaManager_Limited.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        if (GamaManager_Limited.instance.firstCard == null)
        {
            GamaManager_Limited.instance.firstCard = this;
        }
        else
        {
            GamaManager_Limited.instance.secondCard = this;
            GamaManager_Limited.instance.Matched();
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
