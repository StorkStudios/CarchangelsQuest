using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailController : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
        CarEngine carEngine = GetComponentInParent<CarEngine>();
        carEngine.IsDriftingChanged += OnIsDriftingChanged;
    }

    private void OnIsDriftingChanged(bool oldValue, bool newValue)
    {
        trailRenderer.emitting = newValue;
    }
}
