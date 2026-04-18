using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy config", order = 1)]
public class EnemyConfig : ScriptableObject
{
    public int Health;
    public float Speed; 
}
