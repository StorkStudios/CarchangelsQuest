using UnityEngine;

public class TrailController : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private TrailRenderer bloodTrail;
    [SerializeField]
    private float bloodDuration;

    private CarEngine carEngine;
    private PlayerKillDetector playerKillDetector;

    private Coroutine currentBloodCoroutine;

    private void Awake()
    {
        trailRenderer.emitting = false;
        bloodTrail.emitting = false;
        carEngine = GetComponentInParent<CarEngine>();
        playerKillDetector = GetComponentInParent<PlayerKillDetector>();
        if (playerKillDetector != null)
        {
            playerKillDetector.HumanKilled += OnHumanKilled;
        }
        carEngine.IsDriftingChanged += OnIsDriftingChanged;
    }

    private void OnHumanKilled()
    {
        trailRenderer.emitting = false;
        bloodTrail.emitting = true;

        if (currentBloodCoroutine != null)
        {
            StopCoroutine(currentBloodCoroutine);
        }
        currentBloodCoroutine = this.CallDelayed(bloodDuration, () =>
        {
            bloodTrail.emitting = false;
            trailRenderer.emitting = carEngine.IsDrifting;
            currentBloodCoroutine = null;
        });
    }

    private void OnIsDriftingChanged(bool oldValue, bool newValue)
    {
        if (bloodTrail.emitting)
        {
            return;
        }

        trailRenderer.emitting = newValue;
    }
}
