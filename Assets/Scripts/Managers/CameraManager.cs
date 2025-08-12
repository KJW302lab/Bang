using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    #endregion

    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineVirtualCameraBase vCam;

    public void SetFollow(Transform target)
    {
        vCam.Follow = vCam.LookAt = target;
    }
}