using UnityEngine;

public class HealthCrate : MonoBehaviour
{
    [SerializeField]
    private int healAmount;

    public int HealAmount { get { return healAmount; } }
}
