using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGPS : MonoBehaviour
{
    private class GPSMarkerComparer : IComparer<GPSMarker>
    {
        private Transform transform;

        public GPSMarkerComparer(Transform transform)
        {
            this.transform = transform;
        }

        public int Compare(GPSMarker x, GPSMarker y)
        {
            float xDist = Vector3.Distance(transform.position, x.transform.position);
            float yDist = Vector3.Distance(transform.position, y.transform.position);
            return Comparer<float>.Default.Compare(xDist, yDist);
        }
    }

    [SerializeField]
    private SpriteRenderer arrow;
    [SerializeField]
    private float fadeOutDuration;
    [SerializeField]
    private RangeBoundariesFloat arrowScaleRange;
    [SerializeField]
    private RangeBoundariesFloat distanceScaleRange;

    private GPSMarker CurrentTarget => markers.FirstOrDefault();

    private List<GPSMarker> markers;
    private GPSMarkerComparer comparer;

    private void Awake()
    {
        comparer = new GPSMarkerComparer(transform);
    }

    private void Start()
    {
        markers = FindObjectsByType<GPSMarker>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
        SortMarkers();
    }

    private void LateUpdate()
    {
        if (CurrentTarget == null)
        {
            arrow.DOFade(0, fadeOutDuration);
            enabled = false;
        }

        SortMarkers();

        Vector2 targetPos = CurrentTarget.transform.position;
        Vector2 pos = transform.position;

        Vector2 toTarget = targetPos - pos;
        
        transform.up = toTarget.normalized;

        float distance = toTarget.magnitude;
        float t = Mathf.Clamp01(distanceScaleRange.NormalizeValue(distance));
        float scale = Mathf.Lerp(arrowScaleRange.Max, arrowScaleRange.Min, t);
        arrow.transform.localScale = Vector3.one * scale;
    }

    private void SortMarkers()
    {
        markers.Sort(comparer);
    }
}
