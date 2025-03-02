using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSpeedZoomer : MonoBehaviour
{
    [SerializeField]
    private float minCameraSize;

    [SerializeField]
    private float maxCameraSize;

    [SerializeField]
    private float speedForMaxSize;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();   
    }

    private void Update()
    {
        camera.orthographicSize = Mathf.Lerp(minCameraSize, maxCameraSize, playerRigidbody.linearVelocity.magnitude / speedForMaxSize);
    }
}
