using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum State
    {
        Chase,
        Reverse
    }

    [SerializeField]
    private Transform target;

    [SerializeField]
    private CarEngine engine;

    [SerializeField]
    private float steeringSensitivty;

    [SerializeField]
    private Transform frontSensor;

    [SerializeField]
    [ReadOnly]
    private State state = State.Chase;

    private void Update()
    {
        if (Physics2D.OverlapBox(frontSensor.position, frontSensor.localScale, frontSensor.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
        {
            state = State.Reverse;
            engine.gasPedal.Value = -1;
        }
        else
        {
            state = State.Chase;
            engine.gasPedal.Value = 1;
        }
        float angleTowardsTarget = Vector2.SignedAngle(transform.up, target.position - transform.position);
        engine.steeringWheel.Value = Mathf.Clamp(angleTowardsTarget * steeringSensitivty, -1, 1) * ((state == State.Reverse) ? -1 : 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = frontSensor.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
