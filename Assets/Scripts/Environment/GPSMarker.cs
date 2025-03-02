using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class GPSMarker : MonoBehaviour
{
    public event System.Action<GPSMarker> Destroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player))
        {
            OnPlayerEntered();
        }
    }

    private void OnPlayerEntered()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
