using Unity.Cinemachine;
using UnityEngine;

public enum GameMode
{
    Roaming, // Free exploration mode
    Combat // Combat mode with enemies
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }
    private PlayerManager playerManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerManager = Object.FindFirstObjectByType<PlayerManager>();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Roaming:
                // Handle Roaming mode setup
                Debug.Log("Switched to Roaming mode");
                EnterRoamingMode();
                break;
            case GameMode.Combat:
                // Handle Combat mode setup
                Debug.Log("Switched to Combat mode");
                EnterCombatMode();
                break;
            default:
                Debug.LogWarning("Unknown game mode");
                break;
        }
    }

    private void EnterCombatMode()
    {
        CombatManager.Instance.gameObject.SetActive(true); // Enable combat manager
        if (playerManager != null)
        {
            playerManager.gameObject.SetActive(false); // Disable player in combat mode
            CameraManager.Instance.SwitchToCamera(CameraType.CombatCamera); // Switch to combat camera
        }
    }
    private void EnterRoamingMode()
    {
        if (playerManager != null)
        {
            playerManager.gameObject.SetActive(true); // Disable player in combat mode
            CameraManager.Instance.SwitchToCamera(CameraType.RoamingCamera); // Switch to combat camera
        }
    }
}

