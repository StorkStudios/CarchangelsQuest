using UnityEngine;

public class PlayerKillDetector : MonoBehaviour
{
    public event System.Action HumanKilled;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Tags.Human))
        {
            HumanKilled?.Invoke();
        }
    }
}
