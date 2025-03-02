using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSpeedZoomer : MonoBehaviour
{
    [SerializeField]
    private RangeBoundariesFloat cameraSize;

    [SerializeField]
    private RangeBoundariesFloat speedRange;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();   
    }

    private void Update()
    {
        float t = Mathf.Clamp01(speedRange.NormalizeValue(playerRigidbody.linearVelocity.magnitude));

        camera.orthographicSize = Mathf.Lerp(cameraSize.Min, cameraSize.Max, t);
    }
}
