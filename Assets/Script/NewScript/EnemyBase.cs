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

        //    �ڷ�ƾ �����ϱ�(�� ���� �ڷ�ƾ)
        StartCoroutine(SpawnEnemyRoutine());
    }

    void Update()
    {
        Dead();
    }

    

    void Dead()
    {
        //    ���� ü���� 0���� ���ų� ������ ���� ����
        if (data.hp <= 0) NewGameManager.instance.GameOver();
    }

    //    �ڷ�ƾ �� �Լ�
    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(8f);
        }
    }

    //    ���� ������Ű�� �Լ�
    void SpawnEnemy()
    {
        //    ������ ���� �� �ʱ�ȭ
        int randomID = 0;
        GameUnit matchedUnit = null;

        //    ������ �ο�
        float rand = Random.value; 
        //    Random.Range�� �˰ڴµ� value�� ����?
        //    Random.value�� 0~1������ ������ ���� �ִ� ������Ƽ��
        //    0.0000000 ~ 1.0000000���� ����!
        
        //    ���� �� ���ϱ�
        //    20 = 60%
        //    21 = 20%
        //    22 = 20%
        randomID = rand < 0.6f ? 20 :
                          rand < 0.8f ? 21 :
                          22;

        //    �Ʒ����� ���� ������ ������
        /*
        if (rand < 0.6f) randomID = 20;
        else if (rand < 0.8f) randomID = 21;
        else randomID = 22;
        */

        //    ������ ���� ã�� �� List��ü�� �� �� �´� id�� �ִ��� ��ü Ž��
        foreach (GameUnit unit in enemyDataList)
        //    for�� �˰ڴµ� foreach�� ����?
        //    foreach�� �迭�� ��ü�� ���� �ݺ���!
        {
            //    ���� �ִٸ�?
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
        //    ���� �������� �ʰ� �״�� �Ҵ��Ѵٸ�?
        //    ü���� 250�̿��ٰ� ���ݴ��� 220�� �Ǿ��ٸ�
        //    ������ ������ ���ֵ� �� �ش� �����͸� ������ 250�� �ƴ�
        //    220���� �����Ǳ� ����!

        //    Instantiate�� ������ �ƴ� ����� �����ؾ���!
        //    Instantiate(�ڽ�);
        //    �ڽ��� ����? NO!
        //    �ڽ��� ������ ��ġ�� �����Ͽ� ��ġ! YES!


        //    ��������Ʈ ����
        //    ������ ���� �ִ� ��������Ʈ�� ������ ���ֿ� ����
        workUnit.transform.Find("UnitSprite").GetComponent<SpriteRenderer>().sprite = matchedUnit.sprite;
    }

}
