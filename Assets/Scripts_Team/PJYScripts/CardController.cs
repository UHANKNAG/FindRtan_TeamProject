using System.Collections;
using UnityEngine;

/// <summary>
/// (1줄 주석) 카드 뒤집기/매칭 로직
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CardController : MonoBehaviour
{
    [Header("카드 식별 정보")]
    public int cardID = -1;      // 카드 유형(0~4)
    public int rowIndex = -1;    // 보드 행
    public int colIndex = -1;    // 보드 열

    [Header("카드 스프라이트 설정")]
    public Sprite frontSprite;   // 앞면
    public Sprite backSprite;    // 뒷면

    [Header("상태 플래그")]
    public bool isFlipped = false;  
    public bool isMatched = false;  

    [Header("회전 애니메이션")]
    public float flipDuration = 0.3f;

    private SpriteRenderer spriteRenderer;
    private MemoryMatchGameManager gameManager;
    private Vector3 defaultScale;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (backSprite != null)
        {
            spriteRenderer.sprite = backSprite;
        }
    }

    void Start()
    {
        gameManager = MemoryMatchGameManager.Instance;
        defaultScale = transform.localScale;

        // 2D 콜라이더
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;
        col.size = new Vector2(1f, 1.4f);
    }

    void OnMouseDown()
    {
        if (gameManager == null) return;
        if (isMatched) return;
        if (!gameManager.CanFlipCard()) return;
        if (isFlipped) return;

        StartCoroutine(FlipToFront());
    }

    private IEnumerator FlipToFront()
    {
        gameManager.SetFlipLock(true);

        float t = 0f;
        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float ratio = t / flipDuration;
            float newY = Mathf.Lerp(defaultScale.y, 0f, ratio);
            transform.localScale = new Vector3(defaultScale.x, newY, defaultScale.z);
            yield return null;
        }

        isFlipped = true;
        spriteRenderer.sprite = frontSprite;

        t = 0f;
        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float ratio = t / flipDuration;
            float newY = Mathf.Lerp(0f, defaultScale.y, ratio);
            transform.localScale = new Vector3(defaultScale.x, newY, defaultScale.z);
            yield return null;
        }

        gameManager.SetFlipLock(false);
        gameManager.CardRevealed(this);
    }

    public IEnumerator FlipToBack()
    {
        gameManager.SetFlipLock(true);

        float t = 0f;
        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float ratio = t / flipDuration;
            float newY = Mathf.Lerp(defaultScale.y, 0f, ratio);
            transform.localScale = new Vector3(defaultScale.x, newY, defaultScale.z);
            yield return null;
        }

        isFlipped = false;
        if (backSprite != null)
        {
            spriteRenderer.sprite = backSprite;
        }

        t = 0f;
        while (t < flipDuration)
        {
            t += Time.deltaTime;
            float ratio = t / flipDuration;
            float newY = Mathf.Lerp(0f, defaultScale.y, ratio);
            transform.localScale = new Vector3(defaultScale.x, newY, defaultScale.z);
            yield return null;
        }

        gameManager.SetFlipLock(false);
    }

    public void SetMatched()
    {
        isMatched = true;
        gameManager.NotifyCardMatched(rowIndex, colIndex);
        Destroy(gameObject);
    }
}
