using UnityEngine;

[CreateAssetMenu(fileName = "GunConfig", menuName = "Configs/Gun config", order = 1)]
public class GunConfig : ScriptableObject
{
    public float RateOfFire;
    public float Damage;
    public float Range;
    public float SpreadAngle;
}
