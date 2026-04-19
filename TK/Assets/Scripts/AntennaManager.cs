using System.Collections.Generic;
using UnityEngine;

public class AntennaManager : MonoBehaviour
{
    public static AntennaManager Instance;

    public List<AntennaSwitch> Antennas = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
