using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class Cardj : MonoBehaviour
{


    public int idx = 0;   // 카드 이미지/식별용 인덱스

    public Text countTxt;
    public GameObject front;
    public GameObject back;

    public Animator anim;

    public SpriteRenderer frontImage;

    public Button btn;

    AudioSource audioSource;
    public AudioClip clip;

    // [수정 사항] 이벤트 카드 여부 추가
    public bool isEventCard = false;

    void Start()
    {

        // AudioSource 가져오기
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        // 카드가 클릭 가능하도록 초기 설정
        btn.enabled = true;
    }

    void Update()
    {
        // 게임이 끝났다면(게임오버) 클릭 비활성화
        if (GameManagerj.instance.isOver) btn.enabled = false;
    }

    /// <summary>
    /// 카드 인덱스(idx)에 따라 스프라이트 등을 설정
    /// </summary>
    /// <param name="num">배열에서 섞인 idx 값</param>
    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Sprite/Team{idx}");
    }

    /// <summary>
    /// 카드 뒤집기 (버튼 클릭으로 호출)
    /// </summary>
    public void OpenCard()
    {
        // 이미 두 번째 카드까지 뒤집혀 있다면 대기
        if (GameManagerj.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip); // 뒤집는 소리 재생
        anim.SetBool("isOpen", true);

        // 이 과정은 CardFlip 함수를 통해 구현하였다.
        /*
        front.SetActive(true);
        back.SetActive(false);
        */

        // 게임 매니저에서 첫 번째 카드가 비어있으면 이 카드를 첫 번째 카드로
        if (GameManagerj.instance.firstCard == null)
        {
            GameManagerj.instance.firstCard = this;
        }
        else
        {
            // 이미 첫 번째 카드가 있으면 두 번째 카드로
            GameManagerj.instance.secondCard = this;
            GameManagerj.instance.Matched(); 
            // 두 장이 뒤집혔으므로 매칭 검사
        }
    }

    /// <summary>
    /// 매칭에 성공했을 때 카드를 제거
    /// </summary>
    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 1f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 매칭 실패 시 카드를 닫는 애니메이션 후 원상복귀
    /// </summary>
    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1f);
    }

    void CloseCardInvoke()
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
