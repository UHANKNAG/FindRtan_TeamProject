using Unity.VisualScripting;
using UnityEngine;

public enum STATE
{
    WORK = 0,
    ATK,
    DIE,
    WIN
}

[CreateAssetMenu(fileName = "GameUnit", menuName = "유닛 데이터")]
public class GameUnit : ScriptableObject
{
    [Header("유닛의 기본 정보")]
    public string unitName;
    public int id;
    public float hp;
    public float dmg;
    public float atkSpeed;
    public float range;
    public float speed;

    public Sprite sprite;

    [Header("유닛의 상태")]
    public STATE state;
}
