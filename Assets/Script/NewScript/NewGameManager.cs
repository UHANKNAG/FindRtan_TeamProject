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

    public IEnumerator CheckMatchCoroutine()
    {
        isProcessing = true;

        //    소리 우선 재생
        if (firstCard.idx == secondCard.idx) audioSource.PlayOneShot(clip);

        //    딜레이는 애니메이션 기다리는 시간용
        yield return new WaitForSeconds(1.2f);
        
        if (firstCard.idx == secondCard.idx)
        {
            //    게임 오브젝트 생성(Unit 프리팹을 생성시킴)
            GameObject newUnit = Instantiate(Unit, 
                new Vector2(myBase.transform.position.x, 2), Quaternion.identity);

            //    컴포넌트 캐싱 / 컴포넌트 가져오기
            WorkUnit workUnit = newUnit.GetComponent<WorkUnit>();

            //    만약 캐싱 받아오기에 성공하고, 인덱스의 범위가 데이터 수 보다 많다면?
            if (workUnit != null && firstCard.idx < unitDataList.Count)
            //    왜 이렇게 될까?
            //    인덱스의 범위는 0~7, 그리고 가져오는 데이터의 수는 8
            //    즉, 인덱스의 범위가 잘못되거나 버그가 나 8을 넘어버리면 버그가 발생한 것으로 간주함
            //    따라서 idx가 유효한 범위(0~7)인지 확인하기 위한 안전장치
            {
                //    게임 유닛을 데이터 리스트에 있는 범위의 데이터를 가져와 만들기
                GameUnit matchedUnit = unitDataList[firstCard.idx];

                //    해당 데이터를 생성되었던 개체에게 복사하여 주기
                workUnit.gameUnit = Instantiate(matchedUnit);
                //    복사하지 않고 데이터를 그대로 주면 전체의 데이터가 수정되어버림!

                //    스프라이트가 적용된 게임 오브젝트가 자식객채로 있으니 찾아서 할당해주기
                workUnit.transform.Find("UnitSprite").GetComponent<SpriteRenderer>().sprite
                    = matchedUnit.sprite;
            }

            //    보드가 할당되어 있다면 카드를 섞어 다시 재배치 하기
            if (board != null)
            {
                board.ShuffleCards();
            }
        }
        else
        {
            //    매치가 안됬다면 다시 뒤집기
            firstCard.CloseCard();
            secondCard.CloseCard();
        }
        //    선택되었던 카드 할당 해제하기
        firstCard = null;
        secondCard = null;

        //    다시 카드 선택이 가능하게 만들기
        isProcessing = false; 
    }

    //    게임오버!
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverTxt.gameObject.SetActive(true);
    }

    //    다시 시도하기!
    public void Retry()
    {
        SceneManager.LoadScene("NewScene");
    }
    public void Victory()
    {

    }
}
