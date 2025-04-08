using UnityEngine;
using UnityEngine.UI;

public class LJMGameManager : MonoBehaviour
{
    public static LJMGameManager instance;

    Animator anim;

    public LJMCard firstCard; //    ù��° ī��
    public LJMCard secondCard; //    �ι�° ī��

    public Text timeTxt; //    �ð� �ؽ�Ʈ
    public GameObject endTxt; //    ���ӿ��� �ؽ�Ʈ

    public Text comboTxt; //    �޺� �ؽ�Ʈ
    public Text scoreTxt; //    ���ھ� �ؽ�Ʈ

    public int cardCount = 0;
    float floatTime = 0.0f;

    int score = 0;

    public bool isOver = false;

    AudioSource audioSource;
    public AudioClip clip;

    int combo = 0; //    �޺�

    private void Awake()
    {
        if (instance == null) instance = this;
        //    �ν��Ͻ� �Ҵ�

        audioSource = GetComponent<AudioSource>();
        anim = comboTxt.GetComponent<Animator>();
        //    ������Ʈ �ҷ�����
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        //    �ð� ����
        //    0 - ���̿� ����
        //    0.5 - �ð��� �������� ������ �귯��
        //    1 - �ð��� �����ϰ� �귯��


        //    �ʱ�ȭ ���
        endTxt.SetActive(false);
        isOver = false;
        combo = 0;
        score = 0;
        comboTxt.text = ($"{combo} Combo");

        comboTxt.gameObject.SetActive(false);
    }

    void Update()
    {
        //    �׽�Ʈ�� ���� �ð� ��Ȱ��ȭ
        //floatTime += Time.deltaTime;

        /*
        if(floatTime >= 30f)
        //    ���� ���� �ð��� �����ٸ�
        {
            Time.timeScale = 0f;
            //    ���� ���ο� ������ �־ ����� �ʹٸ�
            //    ����Ƽ���� �ð��ʰ� �ؽ�Ʈ�� ����� ������ ��
            //    �Ʒ��� endTxt�� �ƴ� gameoverTxt�� ���� ����
            //    ����� �ð� �ʰ��� ���ӿ��������� �˸� �� �ִ�.
            endTxt.SetActive(true);

            //    ���� ���� �� ī�� ������ �����ϱ� ���� ��ġ
            isOver=true;
        }
        */
        timeTxt.text = floatTime.ToString("N2");
        //    �Ҽ����� 2�ڸ������� ����
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        //    ù��°�� �� ī��� �ι�°�� �� ī�尡 ��ġ�Ѵٸ�
        {
            audioSource.PlayOneShot(clip);
            //    ���� ���� ���� ���

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            //    ���� ī�� ����

            cardCount -= 2;
            //    ���� ī���� �� ����

            //    �޺� ���� ��Ű��
            combo++;
            int totalCombo = combo - 1;

            if (totalCombo > 0)
            {
                comboTxt.gameObject.SetActive(true);
                anim.SetTrigger("GetCombo");
                //    �޺� ���� �����
                Debug.Log("�޺� �ִϸ��̼� �ߵ�");
            }

            comboTxt.text = ($"{totalCombo} Combo");

            //    ���ھ� �߰�
            score += 1000 + (500 * totalCombo);
            scoreTxt.text = score.ToString();

            UpdateComboColor();

            if (cardCount == 0)
            //    ���� ��� ī�带 ����ٸ�
            {
                Invoke("GameOver", 1f);
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            //    ī�� �ٽ� ����

            ResetCombo();
            //    �޺� �ʱ�ȭ
        }

        firstCard = null;
        secondCard = null;
        //    ���õ� ī�� �����ϱ�
    }

    void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }

    void ResetCombo()
    {
        anim.SetTrigger("DoneCombo");
        combo = 0;
        comboTxt.text = ($"{combo.ToString()} Combo");
        UpdateComboColor();
        Invoke("DisableComboTxt", 1f);
        
    }

    void UpdateComboColor()
    {
        int limitedCombo = Mathf.Clamp(combo, 1, 4);

        Color targetColor = Color.white;

        if (limitedCombo > 1)
        {
            float t = (float)(limitedCombo - 2) / 2f;
            targetColor = new Color(1f, 1f - t, 1f - t);
        }

        comboTxt.color = targetColor;
    }

    void DisableComboTxt()
    {
        comboTxt.gameObject.SetActive(false);
    }
}
