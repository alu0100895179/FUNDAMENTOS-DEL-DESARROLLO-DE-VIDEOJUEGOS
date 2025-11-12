using UnityEngine;
using UnityEngine.InputSystem; 
using Unity.Cinemachine; 

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineCamera followCamera;
    public CinemachineCamera groupCamera;

    private bool isFollowCameraActive = true;

    void Start()
    {
        SwitchToFollowCamera();
    }

    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (isFollowCameraActive)
            {
                SwitchToGroupCamera();
            }
            else
            {
                SwitchToFollowCamera();
            }
        }
    }

    public void SwitchToGroupCamera()
    {
        groupCamera.Priority = 20;
        followCamera.Priority = 10;
        isFollowCameraActive = false;
    }

    public void SwitchToFollowCamera()
    {
        followCamera.Priority = 20;
        groupCamera.Priority = 10;
        isFollowCameraActive = true;
    }
}