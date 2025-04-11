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
    }

    private void Update()
    {
        //    만약 죽은 상태라면 작동 안하게 하기
        if (state == STATE.DIE) return;

        MoveUnit();
        HandleAttackCooldown();
        HPbar();
    }

    void HPbar()
    {
        //    체력바의 실시간 업데이트
        hpBar.localScale = new Vector3(hp / maxHp, 1f, 1f);
    }

    private void MoveUnit()
    {
        //    혹시 죽은 상태인데 움직이는 것을 방지
        if (hp <= 0) return;

        switch (state)
        {
            //    움직이는 중이라면?
            case STATE.WORK:
                //    이동속도에 비례해서 움직이기
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
        //    삼항 연산자를 이용해 아군이면 오른쪽으로,
        //    적이면 왼쪽으로 움직이게 하기
        return CompareTag("Player") ? Vector2.right :
                  //    만약 태그가 Player인게 참이라면? 오른쪽으로 움직이기
                  CompareTag("Enemy") ? Vector2.left :
                  //    만약 태그가 Enemy인게 참이라면? 왼쪽으로 움직이기
                  Vector2.zero;
        //    이도저도 아니라면 안움직이기

        //    3항 연산자 설명
        /*
            ~가 참이라면 ? ~하기 : 아니라면
            ~가 참이라면 ? ~하기 : 아니라면
            이렇게 하기
        */

        //    Vector2형 함수이기에 값을 return하여 줌
    }

    private void HandleAttackCooldown()
    {
        //    공격속도 맞추기. 만약 쿨타임이 0 이상이면 쿨타임인 것으로 간주
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
    }

    private IEnumerator AttackRoutine()
    {
        //    공격이 가능하게 만들기
        isAttacking = true;

        //      공격속도만큼 지연 시키기
        yield return new WaitForSeconds(1f / atkSpeed);

        //    대상 레이어 string값 구하기
        string targetLayer = CompareTag("Player") ? "Enemy" : "Player";

        //    타겟으로 잡을 레이어 마스크 설정
        //    만약 적이였다면 플레이어로, 플레이어였다면 적으로
        LayerMask targetMask = LayerMask.GetMask(targetLayer);

        //    범위 내 적 탐색
        //    Physics2D.OverlapCircle
        //    - 지정된 위치(transform.position), 지정된 반지름 범위(range) 내에서 특정 레이어(targetMask)의 객체를 감지하는 함수
        //    (기준 위치, 범위, 감지할 레이어)
        Collider2D target = Physics2D.OverlapCircle(transform.position, range, targetMask);

        //    만약 타겟을 찾았다면
        if (target != null)
        {
            //    컴포넌트 가져오기/컴포넌트 캐싱
            WorkUnit enemyUnit = target.GetComponent<WorkUnit>();
            if (enemyUnit != null)
            {
                //    적 유닛에게 자신의 공격력 만큼 대미지를 주기
                enemyUnit.TakeDamage(dmg);
            }
        }

        state = STATE.WORK;
        attackCooldown = 1f / atkSpeed;
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        //    체력에서 데미지 만큼 빼기
        hp -= damage;
        //    만약 체력이 0.01보다 작다면?
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