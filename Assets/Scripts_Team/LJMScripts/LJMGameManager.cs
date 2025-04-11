using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LJMGameManager : MonoBehaviour
{
    public static LJMGameManager instance;

    Animator anim;

    public LJMCard firstCard; 
    public LJMCard secondCard; 

    public Text timeTxt;
    public GameObject endTxt;    
    public GameObject nextTxt;
    public GameObject teamInfo;

    public Text comboTxt; 
    public Text scoreTxt; 

    public int cardCount = 0;
    float floatTime = 0.0f;

    int score = 0;
    public int nextSceneIndex;

    public bool isOver = false;

    AudioSource audioSource;
    public AudioClip clip;
    public AudioClip oofClip;

    int combo = 0; 

    Vector3 comboOriginPos;

    private void Awake()
    {
        if (instance == null) instance = this;
   
        audioSource = GetComponent<AudioSource>();
        anim = comboTxt.GetComponent<Animator>();
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        teamInfo.SetActive(false);
    }

    void Start()
    {
        Time.timeScale = 1.0f;

        endTxt.SetActive(false);
        isOver = false;
        combo = 0;
        score = 0;
        comboTxt.text = ($"{combo} Combo");

        comboOriginPos = comboTxt.rectTransform.localPosition;

        comboTxt.gameObject.SetActive(false);

        
    }

    void Update()
    {

    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            audioSource.volume = 1f;
            audioSource.PlayOneShot(clip);

            firstCard.anim.SetBool("isMatched", true);  // ù��° ī���� �ִϸ����� �Ķ���� isMatched�� true�� �ٲ��ش�.
            secondCard.anim.SetBool("isMatched", true); // �ι�° ī���� �ִϸ����� �Ķ���� isMatched�� true�� �ٲ��ش�.
            Invoke("DestroyCard", 1f);               // 1�� �Ŀ� DestroyCard �Լ��� ȣ���Ѵ�.

            cardCount -= 2;

            combo++;
            int totalCombo = combo - 1;

            if (totalCombo > 0)
            {
                comboTxt.gameObject.SetActive(true);
                anim.SetTrigger("GetCombo");
            }

            comboTxt.text = ($"{totalCombo} Combo");

            score += 1000 + (500 * totalCombo);
            scoreTxt.text = score.ToString();

            UpdateComboColor();

            if (cardCount == 0)
            {
                Invoke("Victory", 1f);
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            firstCard = null;
            secondCard = null;

            ResetCombo();
        }


    }

    public void DestroyCard()
    {
        firstCard.DestroyCard();
        secondCard.DestroyCard();
        firstCard = null;
        secondCard = null;
    }




    public void GameOver()
    {
        isOver = true;
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }

    public void Victory() {
        if (nextSceneIndex > PlayerPrefs.GetInt("stageAt")) {
            PlayerPrefs.SetInt("stageAt", nextSceneIndex);
        }
        Time.timeScale = 0f;
        nextTxt.SetActive(true);
        teamInfo.SetActive(true);
    }

    void ResetCombo()
    {
        anim.SetTrigger("DoneCombo");
        combo = 0;
        comboTxt.text = ($"{combo} Combo");
        UpdateComboColor();

        audioSource.volume = 0.3f;
        audioSource.PlayOneShot(oofClip);
        

        Invoke("ReturnComboTxt", 1f);

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

    void ReturnComboTxt()
    {
        comboTxt.rectTransform.localPosition = comboOriginPos;
        comboTxt.gameObject.SetActive(false);
    }
}
