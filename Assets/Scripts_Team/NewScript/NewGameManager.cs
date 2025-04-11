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

    public GameObject endTxt;
    public GameObject nextTxt;
    public GameObject teamInfo;
    public GameObject deleteCard;
    public GameObject deleteUnit;
    public int nextSceneIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
        //    �̱��� �ν��Ͻ� �Ҵ�

        audioSource = GetComponent<AudioSource>();
        //    ������Ʈ �ҷ�����

        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        teamInfo.SetActive(false);

        deleteCard = GameObject.Find("Board");
        deleteUnit = GameObject.Find("Unit");
    }

    void Start()
    {
        endTxt.SetActive(false);
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
                new Vector2(myBase.transform.position.x, 2), Quaternion.identity, deleteUnit.transform);

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


    public void Victory() {
        if (nextSceneIndex > PlayerPrefs.GetInt("stageAt")) {
            PlayerPrefs.SetInt("stageAt", nextSceneIndex);
        }
        Time.timeScale = 0f;
        nextTxt.SetActive(true);
        teamInfo.SetActive(true);

        for (int i = 0; i < deleteCard.transform.childCount; i++) 
            Destroy(deleteCard.transform.GetChild(i).gameObject);
        for (int i = 0; i < deleteUnit.transform.childCount; i++) 
            Destroy(deleteUnit.transform.GetChild(i).gameObject);
    }
    
    public void GameOver()
    {
        Time.timeScale = 0f;
        endTxt.SetActive(true);
    }
}
