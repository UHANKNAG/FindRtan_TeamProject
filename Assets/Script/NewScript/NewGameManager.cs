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
    public static NewGameManager instance; //    싱글톤

    public NewCard firstCard; //    첫번째 카드
    public NewCard secondCard; //    두번째 카드

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
        //    싱글톤 인스턴스 할당

        audioSource = GetComponent<AudioSource>();
        //    컴포넌트 불러오기
    }

    void Start()
    {
        gameOverTxt.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        //    시간 설정
        //    0 - 아이에 멈춤
        //    0.5 - 시간이 절반정도 느리게 흘러감
        //    1 - 시간이 동일하게 흘러감

        //    초기화 목록
        isProcessing = false;
    }

    void Update()
    {

    }

    public IEnumerator CheckMatchCoroutine()
    {
        isProcessing = true;

        //    소리 우선 재생
        if (firstCard.idx == secondCard.idx) audioSource.PlayOneShot(clip);

        //    딜레이는 애니메이션 기다리는 시간용
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

        //    다시 카드 선택이 가능하게 만들기
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
