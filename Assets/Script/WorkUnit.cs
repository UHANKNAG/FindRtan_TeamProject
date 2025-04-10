using UnityEngine;
using System.Collections;

public class WorkUnit : MonoBehaviour
{
    public GameUnit gameUnit;

    public string unitName;
    public int id;
    float maxHp;
    public float hp;
    public float dmg;
    public float atkSpeed;
    public float range;
    public float speed;

    public Sprite sprite;

    public STATE state;

    private bool isAttacking = false;
    private float attackCooldown = 0f;
    private Rigidbody2D rb;
    private Collider2D col;

    public RectTransform hpBar;

    private void Start()
    {
        if (gameUnit == null)
        {
            Debug.LogWarning("gameUnit이 할당되지 않았습니다.");
            return;
        }

        // GameUnit 데이터를 초기화
        unitName = gameUnit.unitName;
        id = gameUnit.id;
        maxHp = gameUnit.hp;
        hp = gameUnit.hp;
        dmg = gameUnit.dmg;
        atkSpeed = gameUnit.atkSpeed;
        range = gameUnit.range;
        speed = gameUnit.speed;
        state = gameUnit.state;
        sprite = gameUnit.sprite;

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (state == STATE.DIE) return;

        MoveUnit();
        HandleAttackCooldown();
        HPbar();
    }

    void HPbar()
    {
        hpBar.localScale = new Vector3(hp / maxHp, 1f, 1f);
    }

    private void MoveUnit()
    {
        if (hp <= 0) return;

        switch (state)
        {
            case STATE.WORK:
                rb.linearVelocity = GetMoveDirection() * speed;
                break;

            case STATE.ATK:
                rb.linearVelocity = Vector2.zero;

                if (attackCooldown <= 0 && !isAttacking)
                    StartCoroutine(AttackRoutine());
                break;
        }
    }

    private Vector2 GetMoveDirection()
    {
        return CompareTag("Player") ? Vector2.right :
               CompareTag("Enemy") ? Vector2.left :
               Vector2.zero;
    }

    private void HandleAttackCooldown()
    {
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(1f / atkSpeed);

        // 대상 레이어 이름 결정
        string targetLayer = CompareTag("Player") ? "Enemy" : "Player";

        // 레이어 마스크 설정
        LayerMask targetMask = LayerMask.GetMask(targetLayer);
       
        // 범위 내 적 탐색
        Collider2D target = Physics2D.OverlapCircle(transform.position, range, targetMask);

        if (target != null)
        {
            WorkUnit enemyUnit = target.GetComponent<WorkUnit>();
            if (enemyUnit != null)
            {
                enemyUnit.TakeDamage(dmg);
            }
        }

        state = STATE.WORK;
        attackCooldown = 1f / atkSpeed;
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0.01f)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //    자신이 플레이어고 적을 만났다면
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
            state = STATE.ATK;
        }

        //    자신이 적이고 플레이어를 만났다면
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            state = STATE.ATK;
        }
       
    }
}