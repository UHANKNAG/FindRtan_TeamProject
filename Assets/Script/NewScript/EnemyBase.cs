using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    WorkUnit data;

    public List<GameUnit> enemyDataList;

    public GameObject enemy;

    void Start()
    {
        data = GetComponent<WorkUnit>();

        //    코루틴 시작하기(적 스폰 코루틴)
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        Dead();
    }

    

    void Dead()
    {
        //    만약 체력이 0보다 같거나 적으면 게임 오버
        if (data.hp <= 0) NewGameManager.instance.GameOver();
    }

    //    코루틴 용 함수
    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(8f);
        }
    }

    //    적을 스폰시키는 함수
    void SpawnEnemy()
    {
        //    랜덤값 설정 및 초기화
        int randomID = 0;
        GameUnit matchedUnit = null;

        //    랜덤값 부여
        float rand = Random.value; 
        //    Random.Range는 알겠는데 value는 뭐야?
        //    Random.value는 0~1까지의 랜덤한 수를 주는 프로퍼티로
        //    0.0000000 ~ 1.0000000까지 나옴!
        
        //    랜덤 값 구하기
        //    20 = 60%
        //    21 = 20%
        //    22 = 20%
        randomID = rand < 0.6f ? 20 :
                          rand < 0.8f ? 21 :
                          22;

        //    아래랑도 같은 역할을 수행함
        /*
        if (rand < 0.6f) randomID = 20;
        else if (rand < 0.8f) randomID = 21;
        else randomID = 22;
        */

        //    랜덤한 수를 찾은 뒤 List전체를 본 후 맞는 id가 있는지 전체 탐색
        foreach (GameUnit unit in enemyDataList)
        //    for는 알겠는데 foreach는 뭐야?
        //    foreach는 배열의 전체를 보는 반복문!
        {
            //    만약 있다면?
            if (unit.id == randomID)
            {
                matchedUnit = unit;
                break;
            }
        }

        //    안전장치(만약 이상한 id값을 넣거나 해당 데이터가 없다면)
        if (matchedUnit == null) return;

        //    게임 오브젝트를 씬에 생성(적 프리팹)
        GameObject newUnit = Instantiate(enemy, new Vector2(transform.position.x, 2), Quaternion.identity);

        //    새로 만든 적 오브젝트에서 'WorkUnit' 스크립트를 찾아서 변수에 저장
        WorkUnit workUnit = newUnit.GetComponent<WorkUnit>();

        //    만든 유닛에 id가 일치하는 사용할 유닛 데이터를 복사해서 넣어줌
        workUnit.gameUnit = Instantiate(matchedUnit);
        //    만약 복사하지 않고 그대로 할당한다면?
        //    체력을 250이였다가 공격당해 220이 되었다면
        //    앞으로 스폰된 유닛들 중 해당 데이터를 받으면 250이 아닌
        //    220으로 생성되기 때문!

        //    Instantiate는 생성이 아닌 복사로 이해해야함!
        //    Instantiate(박스);
        //    박스를 생성? NO!
        //    박스를 지정된 위치에 복사하여 배치! YES!


        //    스프라이트 지정
        //    데이터 내에 있는 스프라이트를 생성한 유닛에 지정
        workUnit.transform.Find("UnitSprite").GetComponent<SpriteRenderer>().sprite = matchedUnit.sprite;
    }

}
