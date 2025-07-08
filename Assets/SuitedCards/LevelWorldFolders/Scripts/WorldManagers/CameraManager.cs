using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public enum CameraType
{
    RoamingCamera, // Free exploration camera
    CombatCamera // Combat camera
}
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera combatCamera;
    private CinemachineCamera activeCamera;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        activeCamera = playerCamera; // Set the initial active camera to the roaming camera
    }
    public void SwitchToCamera(CameraType cameraType)
    {
        switch (cameraType)
        {
            case CameraType.RoamingCamera:
                DisableCurrentCamera(); // Disable current camera before switching
                playerCamera.Priority = 1; // Enable roaming camera
                break;
            case CameraType.CombatCamera:
                DisableCurrentCamera(); // Disable current camera before switching
                combatCamera.Priority = 1; // Enable combat camera
                break;
            default:
                Debug.LogWarning("Unknown camera type");
                break;
        }
    }
    private void DisableCurrentCamera()
    {
        activeCamera.Priority = 0; // Disable current camera
    }
}
