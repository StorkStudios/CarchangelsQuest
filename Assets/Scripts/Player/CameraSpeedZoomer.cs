using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSpeedZoomer : MonoBehaviour
{
    [SerializeField]
    private float sleepCameraSize;
    [SerializeField]
    private float sleepSpeed;
    [SerializeField]
    private float sleepTime;
    [SerializeField]
    private RangeBoundariesFloat cameraSize;
    [SerializeField]
    private RangeBoundariesFloat speedRange;
    [SerializeField]
    private float cameraSpeed;

    [SerializeField]
    private Rigidbody2D playerRigidbody;

    private Camera camera;

    private float lastAwakeTime;
    private float cameraTarget;

    private void Start()
    {
        camera = GetComponent<Camera>();
        cameraTarget = sleepCameraSize;
        lastAwakeTime = Time.time - sleepTime;
    }

    private void Update()
    {
        float speed = playerRigidbody.linearVelocity.magnitude;
        if (speed <= sleepSpeed)
        {
            float sleepDuration = Time.time - lastAwakeTime;
            if (sleepDuration >= sleepTime)
            {
                cameraTarget = sleepCameraSize;
            }
        }
        else
        {
            float t = Mathf.Clamp01(speedRange.NormalizeValue(speed));
            cameraTarget = Mathf.Lerp(cameraSize.Min, cameraSize.Max, t);

            lastAwakeTime = Time.time;
        }

        camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, cameraTarget, cameraSpeed * Time.deltaTime);
    }
}
