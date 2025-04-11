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
        count = 3;

        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        btn.enabled = true;
    }


    void Update()
    {
        if (GamaManager_Limited.instance.isOver) btn.enabled = false;

        if (count <= 0)
        {
            count = 0;
            countTxt.color = Color.red;
            back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);

            Invoke("GameEnd", 1.5f);
        }

        countTxt.text = count.ToString();
    }

    public void GameEnd()
    {
        GamaManager_Limited.instance.GameOver();
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

        // 이 과정은 CardFlip 함수를 통해 구현하였다.
        /*
        front.SetActive(true);
        back.SetActive(false);
        */

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
