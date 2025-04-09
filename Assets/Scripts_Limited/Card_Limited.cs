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
        //    최대로 뒤집을 수 있는 수
        count = 3;

        //    컴포넌트 불러오기
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    초기화 목록
        btn.enabled = true;
    }


    void Update()
    {
        //    만약 게임 오버상태라면 뒤집기 기능 비활성화 하기
        if (GamaManager_Limited.instance.isOver) btn.enabled = false;

        if (count <= 0)
        //    만약 카운트가 0이라면
        {
            count = 0;
            countTxt.color = Color.red;
            back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
        }

        //    남은 카운트 띄우기(실시간으로)
        countTxt.text = count.ToString();
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
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
