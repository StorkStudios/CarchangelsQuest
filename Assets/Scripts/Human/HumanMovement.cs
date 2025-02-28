using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HumanMovement : MonoBehaviour
{
    [SerializeField]
    private RangeBoundariesFloat walkDuration;

    [SerializeField]
    private RangeBoundariesFloat walkSpeed;

    private Rigidbody2D rigidbody;

    private Coroutine currentCoroutine;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        currentCoroutine = StartCoroutine(Walk());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            rigidbody.linearVelocity = Random.insideUnitCircle.normalized * walkSpeed.GetRandomBetween();
            yield return new WaitForSeconds(walkDuration.GetRandomBetween());
        }
    }
}
