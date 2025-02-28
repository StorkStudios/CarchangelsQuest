using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HumanMovement : MonoBehaviour
{
    private enum State {Rotating, Walking}
    [SerializeField]
    private RangeBoundariesFloat walkDuration;

    [SerializeField]
    private RangeBoundariesFloat walkSpeedRange;
    [SerializeField]
    private RangeBoundariesFloat rotationSpeedRange;
    [SerializeField]
    [ReadOnly]
    private float targetRotation;

    private Rigidbody2D rigidbody;

    private Coroutine currentCoroutine;
    private State state = State.Rotating;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        currentCoroutine = StartCoroutine(Walk());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (state == State.Rotating)
        {
            return;
        }
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Walk());
    }

    private IEnumerator Walk()
    {
        while (true)
        {
            state = State.Rotating;
            rigidbody.linearVelocity = Vector2.zero;
            targetRotation = Random.Range(0, 360);
            float rotationSpeed = rotationSpeedRange.GetRandomBetween();
            while(Mathf.Abs(GetPositiveAngle(rigidbody.rotation) - targetRotation) > 0.1)
            {
                rigidbody.MoveRotation(Mathf.MoveTowardsAngle(GetPositiveAngle(rigidbody.rotation), targetRotation, rotationSpeed * Time.deltaTime));
                yield return null;
            }
            state = State.Walking;
            rigidbody.linearVelocity = transform.up * walkSpeedRange.GetRandomBetween();
            yield return new WaitForSeconds(walkDuration.GetRandomBetween());
        }
    }

    private float GetPositiveAngle(float angle)
    {
        while(angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
}
