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
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        Dead();
    }

    

    void Dead()
    {
        if (data.hp <= 0) NewGameManager.instance.GameOver();
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(8f);
        }
    }

    void SpawnEnemy()
    {
        //    랜덤값 설정 및 초기화
        int randomID = 0;
        GameUnit matchedUnit = null;

        float rand = Random.value; // 0.0 ~ 1.0

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

        //    랜덤한 수를 찾은 뒤 List전체를 본 후 맞는 id가 있는지 탐색
        foreach (GameUnit unit in enemyDataList)
        {
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

        //    스프라이트 지정
        workUnit.transform.Find("UnitSprite").GetComponent<SpriteRenderer>().sprite = matchedUnit.sprite;
    }

}
