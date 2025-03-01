using UnityEngine;

[ExecuteAlways]
public class TransformFollower : MonoBehaviour
{
    [SerializeField]
    private Transform transformToFollow;
    [SerializeField]
    private Vector3 offset;

    private void LateUpdate()
    {
        transform.position = transformToFollow.position + offset;
    }
}
