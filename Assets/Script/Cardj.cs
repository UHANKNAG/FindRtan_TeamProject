using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class Cardj : MonoBehaviour
{
    // 카드의 종류를 표현하는 열거형
    public enum CardType { Nomal = 0, Heal, Joker };

    public CardType type = CardType.Nomal;

    public int idx = 0;   // 카드 이미지/식별용 인덱스
    public int count = 0; // 카드가 뒤집힐 수 있는 남은 횟수

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
        // 특정 인덱스에 따라 카드 타입 결정 (예: 3 -> Heal, 4 -> Joker)
        if (idx == 3) type = CardType.Heal;
        else if (idx == 4) type = CardType.Joker;

        // 카드 뒤집기 횟수를 3으로 설정
        count = 3;

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

        // 남은 뒤집기 횟수가 0 이하라면
        if (count <= 0)
        {
            count = 0;
            countTxt.color = Color.red;
            back.GetComponent<SpriteRenderer>().color = new Color32(100, 100, 100, 255);
        }

        // 남은 뒤집기 횟수를 UI에 표시
        countTxt.text = count.ToString();
    }

    /// <summary>
    /// 카드 인덱스(idx)에 따라 스프라이트 등을 설정
    /// </summary>
    /// <param name="num">배열에서 섞인 idx 값</param>
    public void Setting(int num)
    {
        idx = num;
        // 예시로 Resources 폴더에서 "rtan0", "rtan1", ... 등을 로드
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
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
        front.SetActive(true);
        back.SetActive(false);

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
}
