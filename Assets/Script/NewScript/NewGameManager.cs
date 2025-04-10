using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class NewGameManager : MonoBehaviour
{
    public static NewGameManager instance; //    �̱���

    public NewCard firstCard; //    ù��° ī��
    public NewCard secondCard; //    �ι�° ī��

    public GameObject myBase;

    public bool isProcessing = false;

    AudioSource audioSource;
    public AudioClip clip;

    public NewBoard board;

    public GameObject Unit;
    public List<GameUnit> unitDataList;

    public Text gameOverTxt;

    private void Awake()
    {
        if (instance == null) instance = this;
        //    �̱��� �ν��Ͻ� �Ҵ�

        audioSource = GetComponent<AudioSource>();
        //    ������Ʈ �ҷ�����
    }

    void Start()
    {
        gameOverTxt.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        //    �ð� ����
        //    0 - ���̿� ����
        //    0.5 - �ð��� �������� ������ �귯��
        //    1 - �ð��� �����ϰ� �귯��

        //    �ʱ�ȭ ���
        isProcessing = false;
    }

    void Update()
    {

    }

    public IEnumerator CheckMatchCoroutine()
    {
        isProcessing = true;

        //    �Ҹ� �켱 ���
        if (firstCard.idx == secondCard.idx) audioSource.PlayOneShot(clip);

        //    �����̴� �ִϸ��̼� ��ٸ��� �ð���
        yield return new WaitForSeconds(1.2f);
        
        if (firstCard.idx == secondCard.idx)
        {

            GameObject newUnit = Instantiate(Unit, 
                new Vector2(myBase.transform.position.x, 2), Quaternion.identity);

            WorkUnit workUnit = newUnit.GetComponent<WorkUnit>();
            if (workUnit != null && firstCard.idx < unitDataList.Count)
            {
                GameUnit matchedUnit = unitDataList[firstCard.idx];

                workUnit.gameUnit = Instantiate(matchedUnit);

                workUnit.transform.Find("UnitSprite").GetComponent<SpriteRenderer>().sprite
                    = matchedUnit.sprite;
            }

            if (board != null)
            {
                board.ShuffleCards();
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;

        //    �ٽ� ī�� ������ �����ϰ� �����
        isProcessing = false; 
    }


    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverTxt.gameObject.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene("NewScene");
    }
}
