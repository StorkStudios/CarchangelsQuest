using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(NavMeshObstacle))]
public class LampBehaviour : MonoBehaviour
{
    [SerializeField]
    private float minSpeedToBreak;

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private NavMeshObstacle obstacle;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player) || collision.CompareTag(Tags.Enemy))
        {
            float speed = collision.attachedRigidbody.linearVelocity.magnitude;

            if (speed >= minSpeedToBreak)
            {
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                obstacle.enabled = false;
            }
            collider.isTrigger = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!obstacle.enabled && (collision.collider.CompareTag(Tags.Player) || collision.collider.CompareTag(Tags.Enemy)))
        {
            collider.isTrigger = true;
        }
    }
}
