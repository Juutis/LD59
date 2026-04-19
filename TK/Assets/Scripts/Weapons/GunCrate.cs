using UnityEngine;

public class GunCrate : MonoBehaviour
{
    [SerializeField]
    private GunType type;
    [SerializeField]
    private int ammoAmount;

    public GunType GunType { get { return type; } }
    public int AmmoAmount { get {  return ammoAmount; } }
}