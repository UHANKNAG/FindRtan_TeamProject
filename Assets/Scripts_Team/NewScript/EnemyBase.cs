using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
        if (data.hp <= 0)
        {
            if (CompareTag("Player")) NewGameManager.instance.GameOver();
            else if (CompareTag("Enemy")) NewGameManager.instance.Victory();
        }
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
        //    ������ ���� �� �ʱ�ȭ
        int randomID = 0;
        GameUnit matchedUnit = null;

        float rand = Random.value; // 0.0 ~ 1.0

        if (rand < 0.6f) randomID = 20;
        else if (rand < 0.8f) randomID = 21;
        else randomID = 22;

        //    ������ ���� ã�� �� List��ü�� �� �� �´� id�� �ִ��� Ž��
        foreach (GameUnit unit in enemyDataList)
        {
            if (unit.id == randomID)
            {
                matchedUnit = unit;
                break;
            }
        }

        //    ������ġ(���� �̻��� id���� �ְų� �ش� �����Ͱ� ���ٸ�)
        if (matchedUnit == null) return;

        //    ���� ������Ʈ�� ���� ����(�� ������)
        GameObject newUnit = Instantiate(enemy, new Vector2(transform.position.x, 2), Quaternion.identity);

        //    ���� ���� �� ������Ʈ���� 'WorkUnit' ��ũ��Ʈ�� ã�Ƽ� ������ ����
        WorkUnit workUnit = newUnit.GetComponent<WorkUnit>();

        //    ���� ���ֿ� id�� ��ġ�ϴ� ����� ���� �����͸� �����ؼ� �־���
        workUnit.gameUnit = Instantiate(matchedUnit);

        //    ��������Ʈ ����
        workUnit.transform.Find("UnitSprite").GetComponent<SpriteRenderer>().sprite = matchedUnit.sprite;
    }

}
