using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject front;
    public GameObject back;

    public Animator anim;

    AudioSource audioSource;
    public AudioClip clip;

    public SpriteRenderer frontImage;

    public int idx = 0;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setting(int number) {
        // 매개변수를 활용하여 외부에서 Setting에 값을 넣어 줄 수 있도록
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
        // Resources 폴더 속 Sprite를 Load할 건데, 경로가 (Resources 안에 바로 있어서 이름만 적어 주면 됨)
        // $ 문자는 문자열을 보간(사이를 채우다)된 문자열로 인정해 줌. 
        // 보간된 문자열을 보간 식이 포함될 수 있기 때문에 {} 안에 변수를 넣어 사용할 수 있다.
    }

    public void OpenCard() {
        if (GameManager.instance.secondCard != null) return;

        audioSource.PlayOneShot(clip);
        // PlayOneShot을 사용하면 다른 Audio Clip끼리 겹치지 않는다

        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        if (GameManager.instance.firstCard == null) {// 첫 카드가 비었다면
        // 첫 카드에 내 정보 넘겨 준다
            GameManager.instance.firstCard = this;
        }
        else {  // 첫 카드가 비어있지 않다면
        // 두 번째 카드에 내 정보를 넘겨 준다
            GameManager.instance.secondCard = this;

        // Matched 함수를 호출해 준다
            GameManager.instance.Matched();
        }
    }

    public void DestroyCard() {
        Invoke("DestroyCardInvoke",  0.5f);
    }

    void DestroyCardInvoke() {
        Destroy(gameObject);
    }

    public void ClosedCard() {
        Invoke("ClosedCardInvoke",  0.5f);
    }

    void ClosedCardInvoke() {
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }
}
