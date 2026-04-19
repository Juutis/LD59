using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy config", order = 1)]
public class EnemyConfig : ScriptableObject
{
    public int Health;
    public float Speed;
    public float TurnSpeed = 360.0f;
    public float VisionRange = 10;
    public float VisionAngle = 70;
    public float SmellDistance = 1.5f;
    public float AttackRange = 3.0f;
    public int MinBurst = 8;
    public int MaxBurst = 10;
    public float AimingDuration = 0.6f;
    public float TrackingDuration = 0.4f;
    public float BackSwingDuration = 0.8f;
    public float MinTimeBetweenBursts = 1.0f;
    public float MaxTimeBetweenBursts = 1.5f;
    public float FireRate = 10;
    public float AccuracyVariationDegrees = 20;
}
