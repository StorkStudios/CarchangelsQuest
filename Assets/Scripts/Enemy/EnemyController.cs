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
    private Transform frontReverseStartRange;

    [SerializeField]
    private Transform frontReverseEndRange;

    [SerializeField]
    [ReadOnly]
    private State state = State.Chase;

    private void Start()
    {
        engine.gasPedal.Value = 1;
    }    

    private void Update()
    {
        if (state == State.Chase)
        {
            if (Physics2D.OverlapBox(frontReverseStartRange.position, frontReverseStartRange.localScale, frontReverseStartRange.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
            {
                state = State.Reverse;
                engine.gasPedal.Value = -1;
            }
        }
        else
        {
            if (!Physics2D.OverlapBox(frontReverseEndRange.position, frontReverseEndRange.localScale, frontReverseEndRange.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
            {
                state = State.Chase;
                engine.gasPedal.Value = 1;
            }
        }
        float angleTowardsTarget = Vector2.SignedAngle(transform.up, target.position - transform.position);
        engine.steeringWheel.Value = Mathf.Clamp(angleTowardsTarget * steeringSensitivty, -1, 1) * ((state == State.Reverse) ? -1 : 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = frontReverseStartRange.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.matrix = frontReverseEndRange.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
