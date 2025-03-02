using UnityEngine;

public class UIMainMenuShineAnimation : MonoBehaviour
{
    public enum Target { Left, Right }

    [SerializeField]
    private float animaitonDuration;
    [SerializeField]
    private RectTransform shineTransform;
    [SerializeField]
    private RectTransform leftMaskTransform;
    [SerializeField]
    private RectTransform rightMaskTransform;
    [SerializeField]
    private RectTransform rightMask2Transform;

    public float AnimationDuration => animaitonDuration;

    [HideInInspector]
    public Target currentTarget = Target.Right;

    private float targetX = 0;

    private void Start()
    {
        targetX = shineTransform.anchoredPosition.x;
    }

    private void Update()
    {
        Vector2 anchoredPosition = shineTransform.anchoredPosition;
        float dir = (currentTarget == Target.Right ? 1 : -1);
        float tX = targetX * dir;
        float step = dir * tX * 2 * Time.deltaTime / animaitonDuration;
        anchoredPosition.x = Mathf.MoveTowards(anchoredPosition.x, tX, step);
        shineTransform.anchoredPosition = anchoredPosition;
        leftMaskTransform.anchoredPosition = anchoredPosition;
        rightMaskTransform.anchoredPosition = anchoredPosition;
        rightMask2Transform.anchoredPosition = anchoredPosition;
    }
}
