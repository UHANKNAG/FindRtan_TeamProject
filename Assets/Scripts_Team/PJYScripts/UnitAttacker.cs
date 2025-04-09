using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// (1줄 주석) 유닛이 Canvas UI 상에서 보스 RectTransform 쪽으로 이동, 근거리 공격
/// </summary>
public class UnitAttacker : MonoBehaviour
{
    [Header("유닛 전투 스탯")]
    public float hp = 50f;         // 유닛 체력
    public float dps = 10f;        // 초당 공격력

    [Header("유닛 이동(인스펙터에서 수정)")]
    public float moveSpeed = 200f; // Canvas 상 이동속도
    public float stopDistance = 50f;// 근접 거리

    private MemoryMatchGameManager gameManager;
    private RectTransform bossRect; // 보스 UI RectTransform
    private RectTransform myRect;   // 유닛 자신의 RectTransform

    void Start()
    {
        gameManager = MemoryMatchGameManager.Instance;
        myRect = GetComponent<RectTransform>();

        // 보스 RectTransform
        if (gameManager != null && gameManager.bossRectTransform != null)
        {
            bossRect = gameManager.bossRectTransform;
        }
    }

    void Update()
    {
        // hp<=0이면 제거 X, 그냥 여기서 로직 중단
        if (hp <= 0f) return;

        // 보스 Rect가 없다면 이동/공격 불가
        if (bossRect == null) return;

        // UI 캔버스 상에서 위치 계산
        Vector2 myPos   = myRect.anchoredPosition;
        Vector2 bossPos = bossRect.anchoredPosition;

        float dist = Vector2.Distance(myPos, bossPos);

        if (dist > stopDistance)
        {
            // 보스에게 이동
            Vector2 dir = (bossPos - myPos).normalized;
            myPos += dir * moveSpeed * Time.deltaTime;
            myRect.anchoredPosition = myPos;
        }
        else
        {
            // 근접 → dps로 보스 공격
            if (gameManager != null && gameManager.bossHP > 0f)
            {
                float damage = dps * Time.deltaTime;
                gameManager.DealDamage(damage);
            }
        }
    }

    // 보스가 공격 시 TakeDamage 호출
    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp < 0f) hp = 0f;
    }
}
