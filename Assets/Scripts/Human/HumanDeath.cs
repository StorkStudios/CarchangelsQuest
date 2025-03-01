using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public class HumanDeath : MonoBehaviour
{
    [SerializeField]
    private float linearDamping;
    [SerializeField]
    private LayerMask corpseLayer;
    [SerializeField]
    private float hitForce;
    [SerializeField]
    private GameObject humanSprite;
    [SerializeField]
    private GameObject corpseSprite;
    [SerializeField]
    private Transform bloodSprite;
    [SerializeField]
    private float bloodShowDuration;

    public event System.Action Died;

    private Rigidbody2D rigidbody;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rigidbody.AddForce(collision.relativeVelocity.normalized * hitForce);
            Died?.Invoke();
            StartCoroutine(DeathCoroutine());
        }
    }

    private IEnumerator DeathCoroutine()
    {
        rigidbody.linearDamping = linearDamping;
        gameObject.layer = corpseLayer.GetLayers()[0];
        humanSprite.SetActive(false);
        corpseSprite.SetActive(true);
        trailRenderer.emitting = true;
        yield return new WaitUntil(() => rigidbody.linearVelocity.magnitude < 0.001f);
        trailRenderer.emitting = false;
        bloodSprite.DOScale(1, bloodShowDuration);
    }
}
