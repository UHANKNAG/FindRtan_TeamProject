using UnityEngine;
using UnityEngine.UI;

public class NewCard : MonoBehaviour
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
        //    ������Ʈ �ҷ�����
        audioSource = GetComponent<AudioSource>();
        btn = GetComponentInChildren<Button>();

        //    �ʱ�ȭ ���
        btn.enabled = true;

        //    ����׿� �ʱ�ȭ ���
        front.SetActive(true);
        back.SetActive(false);
    }


    void Update()
    {
        //    ���� ���� �������¶�� ������ ��� ��Ȱ��ȭ �ϱ�
        if (NewGameManager.instance.isProcessing) btn.enabled = false;
        else btn.enabled = true;
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"NewSprite/NewTeam{idx}");
    }

    public void OpenCard()
    {
        //    �ι�° ī�尡 null�� �ƴ϶�� ����! �� ���� ������
        if (NewGameManager.instance.secondCard != null) return;

        //    ���� ������ �ɷ��ִ� ���¶�� ����!
        if (NewGameManager.instance.isProcessing) return;
        
        //    ī�� ������ �Ҹ� ����ϱ�
        audioSource.PlayOneShot(clip);

        //    �ִϸ��̼� ���� ����
        anim.SetBool("isOpen", true);

        //    ī�� �� �� ������
        front.SetActive(true);
        back.SetActive(false);

        //    ù��° ī�尡 ���ٸ�?
        if (NewGameManager.instance.firstCard == null)
        {
            //    ���� ù��° ī�尡 ���ٸ� ù��° ī��� �Ҵ��ϱ�
            NewGameManager.instance.firstCard = this;
        }
        //    �� ���� ��Ȳ
        else
        {
            //    ù��° ī�尡 �ִٸ� �ι�° ī�� �Ҵ��ϱ�
            NewGameManager.instance.secondCard = this;
            //    �¾Ҵ��� �ȸ¾Ҵ��� Ȯ���ϴ� �Լ�
            NewGameManager.instance.StartCoroutine(NewGameManager.instance.CheckMatchCoroutine());
        }
        
    }

    //    ¦�� ���� �ʾ��� ���
    public void CloseCard()
    {
        //    �ִϸ��̼� ���� �� ī�� �յ� ������
        anim.SetBool("isOpen", false);
        front.SetActive(true);
        back.SetActive(false);
    }

    //    ¦�� �¾��� ���
    //    �� �ڵ�� �ٸ������� �����! NewBoard���� �����
    public void ForceCloseImmediately()
    {
        anim.SetBool("isOpen", false);
        front.SetActive(true);
        back.SetActive(false);
    }
    

}
