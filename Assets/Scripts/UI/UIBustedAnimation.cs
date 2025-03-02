using DG.Tweening;
using UnityEngine;

public class UIBustedAnimation : MonoBehaviour
{
    [SerializeField]
    private RectTransform mask;
    [SerializeField]
    private float animationDuration;

    private void Start()
    {
        GameEndManager.Instance.GameEnded += StartAnimation;
    }

    private void StartAnimation()
    {
        Vector3 position = mask.position;

        Vector2 pivot = mask.pivot;
        pivot.x = 1;
        mask.pivot = pivot;

        Vector2 anchorMin = mask.anchorMin;
        anchorMin.x = 0;
        mask.anchorMin = anchorMin;

        Vector2 anchorMax = mask.anchorMax;
        anchorMax.x = 0;
        mask.anchorMax = anchorMax;

        mask.position = position;

        mask.DOAnchorPosX(0, animationDuration).SetEase(Ease.Linear).SetUpdate(true);
    }
}
