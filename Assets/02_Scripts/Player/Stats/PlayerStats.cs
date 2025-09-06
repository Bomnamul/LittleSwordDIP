using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "LittleSword/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int maxHP = 100;
    public float moveSpeed = 5f;
    public int attackDamage = 20;
}
