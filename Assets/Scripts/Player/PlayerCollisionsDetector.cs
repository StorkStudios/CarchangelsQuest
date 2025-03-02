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
        if (Mathf.Abs(lastForwardSpeed) > minSpeed &&
            !collision.gameObject.CompareTag("Human"))
        {
            CollisionEvent?.Invoke();
        }
    }
}
