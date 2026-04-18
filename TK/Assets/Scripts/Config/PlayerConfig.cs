using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player config", order = 1)]
public class PlayerConfig : ScriptableObject
{
    public int Health;
    public float Speed;
}
