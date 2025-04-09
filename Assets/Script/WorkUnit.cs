using UnityEngine;
using System.Collections;

public class WorkUnit : MonoBehaviour
{
    public GameUnit gameUnit;

    public string unitName;
    public int id;
    public float hp;
    public float dmg;
    public float atkSpeed;
    public float range;
    public float speed;

    public STATE state;

    private bool isAttacking = false;
    private float attackCooldown = 0f;
    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        // GameUnit 데이터를 초기화
        unitName = gameUnit.unitName;
        id = gameUnit.id;
        hp = gameUnit.hp;
        dmg = gameUnit.dmg;
        atkSpeed = gameUnit.atkSpeed;
        range = gameUnit.range;
        speed = gameUnit.speed;
        state = gameUnit.state;

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (state == STATE.DIE) return;

        MoveUnit();
        HandleAttackCooldown();
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

        // 공격 딜레이
        yield return new WaitForSeconds(1f / atkSpeed);

        // 대상 레이어 결정
        string targetLayer = CompareTag("Player") ? "Enemy" : "Player";
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

        // 상태 복귀
        state = STATE.WORK;

        attackCooldown = 1f / atkSpeed;
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0.01f)
        {
            Die();
        }
    }

    private void Die()
    {
        state = STATE.DIE;
        tag = "Untagged";
        col.enabled = false;
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 0.1f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Player"))
        {
            state = STATE.ATK;
        }

        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Enemy"))
        {
            state = STATE.ATK;
        }
    }
}