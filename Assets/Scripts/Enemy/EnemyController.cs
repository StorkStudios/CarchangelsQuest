using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private CarEngine engine;

    [SerializeField]
    private float steeringSensitivty;

    private void Start()
    {
        engine.gasPedal.Value = 1;   
    }

    private void Update()
    {
        float angleTowardsTarget = Vector2.SignedAngle(transform.up, target.position - transform.position);
        engine.steeringWheel.Value = Mathf.Clamp(angleTowardsTarget * steeringSensitivty, -1, 1);
    }
}
