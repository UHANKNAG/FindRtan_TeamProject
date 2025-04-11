using UnityEngine;
using UnityEngine.UI;

public class NewCard : MonoBehaviour
{
    public int idx = 0;

    //    앞뒤 게임 오브젝트
    public GameObject front;
    public GameObject back;

    //    애니메이터 불러오기
    public Animator anim;

    //    앞장의 이미지
    public SpriteRenderer frontImage;

    public Button btn;

    AudioSource audioSource;
    public AudioClip clip;


    void Start()
    {
        //    컴포넌트 불러오기
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    초기화 목록
        btn.enabled = true;

        //    디버그용 초기화 목록
        front.SetActive(true);
        back.SetActive(false);
    }


    void Update()
    {
        //    만약 게임 오버상태라면 뒤집기 기능 비활성화 하기
        if (NewGameManager.instance.isProcessing) btn.enabled = false;
        else btn.enabled = true;
    }

    //    카드에 인덱스와 스프라이트를 지정하는 함수
    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        //    두번째 카드가 null이 아니라면 꺼져! 즉 버그 방지용
        if (NewGameManager.instance.secondCard != null) return;
        //    만약 제한이 걸려있는 상태라면 꺼져! 이것도 버그 방지용
        if (NewGameManager.instance.isProcessing) return;
        
        //    카드 뒤집는 소리 재생하기
        audioSource.PlayOneShot(clip);

        //    애니메이션 조건 세팅
        anim.SetBool("isOpen", true);

        //    카드 앞 뒤 뒤집기
        front.SetActive(true);
        back.SetActive(false);

        //    첫번째 카드가 없다면?
        if (NewGameManager.instance.firstCard == null)
        {
            //    만약 첫번째 카드가 없다면 첫번째 카드로 할당하기
            NewGameManager.instance.firstCard = this;
        }
        //    그 외의 상황
        else
        {
            //    첫번째 카드가 있다면 두번째 카드 할당하기
            NewGameManager.instance.secondCard = this;
            //    맞았는지 안맞았는지 확인하는 함수
            NewGameManager.instance.StartCoroutine(NewGameManager.instance.CheckMatchCoroutine());
        }
        
    }

    //    짝이 맞이 않았을 경우
    public void CloseCard()
    {
        //    애니메이션 세팅 후 카드 앞뒤 뒤집기
        //    현재 디버그(시연)을 위해 값을 뒤집음
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }

    //    짝이 맞았을 경우
    //    이 코드는 다른곳에서 실행됨! NewBoard에서 실행됨
    public void ForceCloseImmediately()
    {
        //    현재 디버그(시연)을 위해 값을 뒤집음
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }
    

}
