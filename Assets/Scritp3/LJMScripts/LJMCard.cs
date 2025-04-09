using UnityEngine;
using UnityEngine.UI;

public class LJMCard : MonoBehaviour
{
    public int idx = 0;

    public GameObject front;
    public GameObject back;

    public Animator anim;

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
        if (LJMGameManager.instance.isOver) btn.enabled = false;
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        if (LJMGameManager.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        if (LJMGameManager.instance.firstCard == null)
        {
            LJMGameManager.instance.firstCard = this;
        }
        else
        {
            LJMGameManager.instance.secondCard = this;
            LJMGameManager.instance.Matched();
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
