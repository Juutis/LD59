using System.Collections.Generic;
using UnityEngine;

public class AntennaManager : MonoBehaviour
{
    public static AntennaManager Instance;

    public List<AntennaSwitch> Antennas = new();
    public List<Enemy> Enemies = new();

    void Awake()
    {
        Instance = this;
    }
}
