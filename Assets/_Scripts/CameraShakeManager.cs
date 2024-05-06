using UnityEngine;
using Cinemachine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager _Instance { get; } = new();
    [SerializeField] private float _globalShakeForce = 1f;

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(_globalShakeForce);
    }
}
