using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int[] currentLevelAmmos;

    private PlayerShooting playerShooting;
    private CharacterHealth playerHealth;

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
            transform.SetParent(null);

            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetPlayerShooting(PlayerShooting player)
    {
        this.playerShooting = player;
    }

    public void SetPlayerHealth(CharacterHealth playerHealth)
    {
        this.playerHealth = playerHealth;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void EndLevel()
    {
        currentLevelAmmos = playerShooting.GetAmmos().ToList().ToArray();
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }

    public int[] GetLevelStartAmmos()
    {
        if (currentLevel == 0)
        {
            return new int[5] { 0, 0, 0, 0, 0 };
        }
        else
        {
            return currentLevelAmmos;
        }
    }

    public int[] GetCurrentAmmos()
    {
        return playerShooting.GetAmmos();
    }

    public float GetCurrentHealth()
    {
        return playerHealth.GetHealth();
    }

    public int GetActiveGun()
    {
        return playerShooting.GetActiveGun();
    }
}
