using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseOverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Vector2 direction = new(1, -1);
    [SerializeField]
    private float animationDuration;
    [SerializeField]
    private Transform front;
    [SerializeField]
    private Transform back;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAnimations();
        front.DOLocalMove(direction, animationDuration);
        back.DOLocalMove(-direction, animationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAnimations();
        front.DOLocalMove(Vector3.zero, animationDuration);
        back.DOLocalMove(Vector3.zero, animationDuration);
    }

    private void StopAnimations()
    {
        front.DOKill();
        back.DOKill();
    }
}
