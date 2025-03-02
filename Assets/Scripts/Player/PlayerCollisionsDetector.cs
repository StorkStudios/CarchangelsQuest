using System;
using UnityEngine;

[RequireComponent(typeof(CarEngine))]
public class PlayerCollisionsDetector : MonoBehaviour
{
    [SerializeField]
    private float minSpeed;

    private CarEngine engine;

    private float lastForwardSpeed;

    public event Action CollisionEvent;
    public event Action HumanHit;

    private void Start()
    {
        engine = GetComponent<CarEngine>();
    }

    private void FixedUpdate()
    {
        lastForwardSpeed = engine.ForwardSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Abs(lastForwardSpeed) > minSpeed)
        {
            CollisionEvent?.Invoke();
        }
        if (collision.gameObject.CompareTag(Tags.Human))
        {
            HumanHit?.Invoke();
        }
    }
}
