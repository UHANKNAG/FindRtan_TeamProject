using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// (1줄 주석) 메인 게임 매니저
/// </summary>
public class MemoryMatchGameManager : MonoBehaviour
{
    public static MemoryMatchGameManager Instance;

    #region [보드 & 카드 설정]
    [Header("보드 크기 초기값")]
    public int columns = 5; 
    public int rows = 4;

    [Header("카드 프리팹 & 스프라이트")]
    public GameObject cardPrefab;
    public Sprite[] cardFrontImages; 
    public Sprite cardBackImage;

    [Header("배치 설정")]
    public float horizontalSpacing = 1.3f;
    public float verticalSpacing = 1.8f;
    public Vector3 startPosition = new Vector3(0f,0f,0f);

    [Tooltip("매칭 확인 전 대기시간(초)")]
    public float checkDelay = 1.0f;

    private CardController[,] boardCards;
    #endregion

    #region [게임 진행]
    public enum GameMode
    {
        AttemptsBased,
        ScoreBased
    }
    public GameMode currentMode = GameMode.AttemptsBased;

    private CardController firstCard;
    private CardController secondCard;
    private bool canFlip = true;

    private int attemptCount = 0;
    private int score = 0;
    private int remainingPairs = 10;
    #endregion

    #region [카드 수집 & 유닛 소환]
    [Header("카드 수집 & 유닛")]
    public int[] collectedCards = new int[5];
    public GameObject[] unitPrefabs;
    public Canvas unitCanvas;
    private List<UnitAttacker> unitsOnField = new List<UnitAttacker>();
    #endregion

    #region [보스 & 유닛 상호 공격]
    [Header("보스 관련(캔버스 UI)")]
    public RectTransform bossRectTransform;
    public float bossMaxHP = 1000f;
    public float bossHP = 1000f;
    public int bossLevel = 1;
    public float rewardOnBossKill = 50f;

    public float bossDamagePerHit = 5f;
    public float bossAttackInterval = 2f;
    private float bossAttackTimer = 0f;
    #endregion

    #region [골드 & 업그레이드]
    public float gold = 0f;
    public float upgradeCost = 100f;
    public float upgradeIncrement = 5f;
    #endregion

    #region [UI 연결]
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bossHPText;
    #endregion

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 보드 생성
        SetupBoard();
        remainingPairs = (columns * rows)/2;
        UpdateUI();
    }

    void Update()
    {
        // 보스가 주기적으로 유닛 공격
        bossAttackTimer += Time.deltaTime;
        if(bossAttackTimer>=bossAttackInterval)
        {
            bossAttackTimer=0f;
            BossAttackUnits(); 
        }
    }

    private void SetupBoard()
    {
        boardCards = new CardController[rows, columns];

        if(cardPrefab==null || cardFrontImages.Length<5)
        {
            Debug.LogError("카드 프리팹 or 이미지 세팅이 누락.");
            return;
        }

        // 5종류 x4 => 20장
        List<(int,Sprite)> cardList = new List<(int,Sprite)>();
        for(int i=0;i<5;i++)
        {
            Sprite sp = cardFrontImages[i];
            for(int j=0;j<4;j++)
            {
                cardList.Add((i,sp));
            }
        }

        // 셔플
        for(int i=0;i<cardList.Count;i++)
        {
            var tmp=cardList[i];
            int r = Random.Range(i, cardList.Count);
            cardList[i]=cardList[r];
            cardList[r]=tmp;
        }

        int index=0;
        for(int r=0;r<rows;r++)
        {
            for(int c=0;c<columns;c++)
            {
                float px = startPosition.x + c*horizontalSpacing;
                float py = startPosition.y - r*verticalSpacing;
                Vector3 pos = new Vector3(px,py,0f);

                GameObject newCard = Instantiate(cardPrefab,pos,Quaternion.identity);
                CardController cc = newCard.GetComponent<CardController>();
                if(cc!=null)
                {
                    cc.cardID = cardList[index].Item1;
                    cc.frontSprite = cardList[index].Item2;
                    cc.backSprite = cardBackImage;
                    cc.rowIndex = r;
                    cc.colIndex = c;

                    var sr = newCard.GetComponent<SpriteRenderer>();
                    if(sr!=null && cardBackImage!=null)
                    {
                        sr.sprite = cardBackImage;
                    }
                    boardCards[r,c] = cc;
                }
                index++;
            }
        }
    }

    public void CardRevealed(CardController revealed)
    {
        if(firstCard==null) firstCard = revealed;
        else
        {
            secondCard = revealed;
            attemptCount++;
            if(currentMode==GameMode.ScoreBased)
            {
                score-=1; //뒤집는 순간 -1
            }
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(checkDelay);

        if(firstCard.cardID==secondCard.cardID)
        {
            collectedCards[firstCard.cardID]++;
            firstCard.SetMatched();
            secondCard.SetMatched();

            if(currentMode==GameMode.ScoreBased)
            {
                score+=3; 
            }
            remainingPairs--;
            if(remainingPairs<=0)
            {
                GameClear();
            }
            SpawnUnitOnCanvas(firstCard.cardID);
        }
        else
        {
            StartCoroutine(firstCard.FlipToBack());
            StartCoroutine(secondCard.FlipToBack());
        }

        firstCard=null;
        secondCard=null;
        UpdateUI();
    }

    public void NotifyCardMatched(int row,int col)
    {
        boardCards[row,col]=null;
    }

    // Canvas 상에 유닛 소환. 근거리 공격 => UnitAttacker
    private void SpawnUnitOnCanvas(int cardID)
    {
        if(unitPrefabs==null || cardID<0||cardID>=unitPrefabs.Length) return;
        var prefab = unitPrefabs[cardID];
        if(prefab==null) return;
        if(unitCanvas==null)
        {
            Debug.LogWarning("unitCanvas가 없음. 유닛 소환 실패");
            return;
        }

        GameObject newUnit = Instantiate(prefab, unitCanvas.transform);

        RectTransform prefabRect = prefab.GetComponent<RectTransform>();
        RectTransform newRect = newUnit.GetComponent<RectTransform>();

        if(prefabRect!=null && newRect!=null)
        {
            newRect.anchoredPosition = prefabRect.anchoredPosition;
            newRect.localRotation    = prefabRect.localRotation;
            newRect.localScale       = prefabRect.localScale;
        }

        var ua = newUnit.GetComponent<UnitAttacker>();
        if(ua!=null)
        {
            unitsOnField.Add(ua);
        }
    }

    private void BossAttackUnits()
    {
        // hp<=0인 유닛은 없애고, hp>0이면 데미지
        for(int i=unitsOnField.Count-1;i>=0;i--)
        {
            var unit = unitsOnField[i];
            if(unit==null)
            {
                unitsOnField.RemoveAt(i);
                continue;
            }
            // 보스가 1회 타격
            unit.TakeDamage(bossDamagePerHit);

            if(unit.hp<=0)
            {
                // 정말 보스가 때려서 hp<=0이 된 경우만 파괴
                Destroy(unit.gameObject);
                unitsOnField.RemoveAt(i);
            }
        }
    }

    public void DealDamage(float dmg)
    {
        bossHP-=dmg;
        if(bossHP<0f) bossHP=0f;
        UpdateUI();

        if(bossHP<=0f)
        {
            Debug.Log($"보스 Lv.{bossLevel} 처치. 골드 +{rewardOnBossKill}");
            gold+=rewardOnBossKill;
            bossLevel++;
            bossMaxHP*=1.3f;
            bossHP=bossMaxHP;
            UpdateUI();
        }
    }

    private void GameClear()
    {
        Debug.Log("모든 카드 매칭됨 => 새 보드 4x5 생성");

        columns=4; 
        rows=5;

        for(int r=0;r<boardCards.GetLength(0);r++)
        {
            for(int c=0;c<boardCards.GetLength(1);c++)
            {
                if(boardCards[r,c]!=null)
                {
                    Destroy(boardCards[r,c].gameObject);
                }
            }
        }
        boardCards=null;
        firstCard=null;
        secondCard=null;

        remainingPairs=(columns*rows)/2;
        SetupBoard();
        UpdateUI();
    }

    public bool CanFlipCard(){return canFlip;}
    public void SetFlipLock(bool locked){ canFlip=!locked; }

    private void UpdateUI()
    {
        if(attemptsText) attemptsText.text = $"시도: {attemptCount}";
        if(scoreText)    scoreText.text    = $"점수: {score}";
        if(goldText)     goldText.text     = $"Gold: {gold:F0}";
        if(bossHPText)   bossHPText.text   = $"BossHP: {bossHP:F0}/{bossMaxHP:F0}";

        // (1줄 주석) 전투 진행중
    }

    public void OnUpgradeUnitsButtonClicked()
    {
        if(gold<upgradeCost)
        {
            Debug.Log("골드 부족. 업그레이드 실패");
            return;
        }
        gold-=upgradeCost;
        foreach(var unit in unitsOnField)
        {
            if(unit!=null)
            {
                unit.dps+=upgradeIncrement;
            }
        }
        Debug.Log($"유닛 dps +{upgradeIncrement} 업그레이드 완료");
        UpdateUI();
    }
}
