using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AntennaManager : MonoBehaviour
{
    public static AntennaManager Instance;

    public List<AntennaSwitch> Antennas = new();
    public List<Enemy> Enemies = new();

    private bool levelFinished = false;

    [SerializeField]
    private GameObject LevelFinished;

    private InputAction nextLevelAction;

    void Awake()
    {
        Instance = this;
        nextLevelAction = InputSystem.actions.FindAction("NextLevel");
    }

    void Update()
    {
        if (nextLevelAction.WasPerformedThisFrame() && levelFinished)
        {
            NextLevel();
        }
    }

    public void AntennaActivated(AntennaSwitch antenna)
    {
        Antennas.Remove(antenna);
        if (Antennas.Count == 0)
        {
            LevelFinished.SetActive(true);
            levelFinished = true;
        }
    }

    void NextLevel()
    {
        GameManager.instance.EndLevel();
    }
}
