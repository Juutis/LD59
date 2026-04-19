using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int[] currentLevelAmmos;

    private PlayerShooting playerShooting;

    private int currentLevel = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetPlayerShooting(PlayerShooting player)
    {
        this.playerShooting = player;
    }
    public void EndLevel()
    {
        currentLevelAmmos = playerShooting.GetAmmos();
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }

    public int[] GetAmmos()
    {
        if (currentLevel == 0)
        {
            return new int[4] { 0, 0, 0, 0 };
        }
        else
        {
            return currentLevelAmmos;
        }
    }
}
