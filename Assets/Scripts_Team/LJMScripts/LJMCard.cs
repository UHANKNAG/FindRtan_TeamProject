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
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        btn.enabled = true;

        front.SetActive(true);
        back.SetActive(false);
    }


    void Update()
    {
        if (LJMGameManager.instance.isOver) btn.enabled = false;
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Sprite/Team{idx}");
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
