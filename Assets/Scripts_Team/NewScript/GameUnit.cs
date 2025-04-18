using Unity.VisualScripting;
using UnityEngine;

public enum STATE
{
    WORK = 0,
    ATK,
    DIE,
    WIN
}

[CreateAssetMenu(fileName = "GameUnit", menuName = "���� ������")]
public class GameUnit : ScriptableObject
{
    [Header("������ �⺻ ����")]
    public string unitName;
    public int id;
    public float hp;
    public float dmg;
    public float atkSpeed;
    public float range;
    public float speed;

    public Sprite sprite;

    [Header("������ ����")]
    public STATE state;
}
