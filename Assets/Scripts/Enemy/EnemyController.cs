using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private enum State
    {
        Chase,
        ReverseChase,
        GoingToWaypoint,
        ReverseWaypoint
    }

    [Header("References")]
    [SerializeField]
    public Transform Player;

    [SerializeField]
    private CarEngine engine;

    [Header("Config")]
    [SerializeField]
    private float steeringSensitivty;

    [SerializeField]
    private Transform frontReverseStartRange;

    [SerializeField]
    private Transform frontReverseEndRange;

    [SerializeField]
    private float pathRecalculationDelay = 1;

    [SerializeField]
    private float maxDistanceFromPlayer;

    [Header("Debug")]

    [SerializeField]
    [ReadOnly]
    private State state = State.Chase;

    public event Action<EnemyController> Despawned;

    private NavMeshAgent agent;
    private NavMeshPath path;
    private Vector3 waypoint;

    private ContactFilter2D wallContactFilter = new ContactFilter2D();
    private RaycastHit2D[] raycastResults = new RaycastHit2D[10];

    private void Start()
    {
        engine.gasPedal.Value = 1;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        path = new NavMeshPath();

        LayerMask wallMask = new LayerMask();
        wallMask.value = LayerMask.GetMask("Wall");
        wallContactFilter.SetLayerMask(wallMask);
        wallContactFilter.useLayerMask = true;
    }

    private void Update()
    {
        if ((transform.position - Player.transform.position).magnitude > maxDistanceFromPlayer)
        {
            Debug.Log($"Enemy {gameObject.name} despawned");
            Despawned?.Invoke(this);
            Destroy(gameObject);
        }

        switch (state)
        {
            case State.Chase:
                {
                    int hits = WallRaycast();
                    if (hits > 0)
                    {
                        //PrintRaycastResults(hits);
                        state = State.GoingToWaypoint;
                        StartCoroutine(RecalculatePathCoroutine());
                    }
                    else
                    {
                        if (Physics2D.OverlapBox(frontReverseStartRange.position, frontReverseStartRange.localScale, frontReverseStartRange.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
                        {
                            state = State.ReverseChase;
                            engine.gasPedal.Value = -1;
                        }
                    }
                }
                break;
            case State.ReverseChase:
                if (!Physics2D.OverlapBox(frontReverseEndRange.position, frontReverseEndRange.localScale, frontReverseEndRange.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
                {
                    state = State.Chase;
                    engine.gasPedal.Value = 1;
                }
                break;
            case State.GoingToWaypoint:
                {
                    int hits = WallRaycast();
                    if (hits == 0)
                    {
                        state = State.Chase;
                    }
                    else
                    {
                        //PrintRaycastResults(hits);
                        if (Physics2D.OverlapBox(frontReverseStartRange.position, frontReverseStartRange.localScale, frontReverseStartRange.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
                        {
                            state = State.ReverseWaypoint;
                            engine.gasPedal.Value = -1;
                        }
                    }
                }
                break;
            case State.ReverseWaypoint:
                if (!Physics2D.OverlapBox(frontReverseEndRange.position, frontReverseEndRange.localScale, frontReverseEndRange.rotation.eulerAngles.z, LayerMask.GetMask("Wall")))
                {
                    state = State.GoingToWaypoint;
                    engine.gasPedal.Value = 1;
                }
                break;
        }

        bool toPlayer = state == State.Chase || state == State.ReverseChase;
        float angleTowardsTarget = Vector2.SignedAngle(transform.up, (toPlayer ? Player.position : waypoint) - transform.position);
        engine.steeringWheel.Value = Mathf.Clamp(angleTowardsTarget * steeringSensitivty, -1, 1) * ((state == State.ReverseChase || state == State.ReverseWaypoint) ? -1 : 1);
    }

    private IEnumerator RecalculatePathCoroutine()
    {
        while (state == State.GoingToWaypoint || state == State.ReverseWaypoint)
        {
            agent.enabled = true;
            if (agent.CalculatePath(Player.position, path))
            {
                waypoint = path.corners[1];
            }
            else
            {
                Debug.LogWarning($"No valid path for enemy {gameObject.name}");
            }
            agent.enabled = false;
            yield return new WaitForSeconds(pathRecalculationDelay);
        }
    }

    private int WallRaycast()
    {
        float raycastLength = (Player.position - transform.position).magnitude;
        return Physics2D.Raycast(transform.position, Player.position - transform.position, wallContactFilter, raycastResults, raycastLength);
    }

    private void PrintRaycastResults(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log($"RaycastHit: {raycastResults[i].point}, object: {raycastResults[i].collider.gameObject.name}");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = frontReverseStartRange.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.matrix = frontReverseEndRange.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.magenta;
        bool toPlayer = state == State.Chase || state == State.ReverseChase;
        Gizmos.DrawLine(transform.position, toPlayer ? Player.position : waypoint);
    }
}
