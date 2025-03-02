using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGPS : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer arrow;
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private RangeBoundariesFloat arrowScaleRange;
    [SerializeField]
    private RangeBoundariesFloat distanceScaleRange;

    private GPSMarker currentTarget;

    public void SetMarker(GPSMarker marker)
    {
        currentTarget = marker;
        marker.Destroyed += OnMarkerDestroyed;
        enabled = true;
        arrow.DOFade(1, fadeOutDuration);
    }

    private void OnMarkerDestroyed(GPSMarker marker)
    {
        marker.Destroyed -= OnMarkerDestroyed;
        currentTarget = null;
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
        {
            arrow.DOFade(0, fadeOutDuration);
            enabled = false;
            return;
        }

        Vector2 targetPos = currentTarget.transform.position;
        Vector2 pos = transform.position;

        Vector2 toTarget = targetPos - pos;
        
        transform.up = toTarget.normalized;

        float distance = toTarget.magnitude;
        float t = Mathf.Clamp01(distanceScaleRange.NormalizeValue(distance));
        float scale = Mathf.Lerp(arrowScaleRange.Max, arrowScaleRange.Min, t);
        arrow.transform.localScale = Vector3.one * scale;
    }
}
