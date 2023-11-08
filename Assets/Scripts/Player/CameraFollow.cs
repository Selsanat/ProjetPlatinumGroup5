using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 center;
    public bool FollowPlayers;

    void Start()
    {
        CameraTransition.Instance.cameraFollow = this;
    }
    void Update()
    {
        if (FollowPlayers)
        {
            foreach(var player in RoundManager.Instance.alivePlayers)
            {
                center += player._playerStateMachine.transform.position;
            }
            center /= RoundManager.Instance.alivePlayers.Count;
            center.z = transform.position.z;
            center.x= Mathf.Clamp(center.x, -CameraTransition.Instance.cameraParams.cameraXmax/2, CameraTransition.Instance.cameraParams.cameraXmax/2);
            center.y =Mathf.Clamp(center.y, -CameraTransition.Instance.cameraParams.cameraYmax / 2, CameraTransition.Instance.cameraParams.cameraYmax/2);
            transform.position = center;
        }
    }
}
