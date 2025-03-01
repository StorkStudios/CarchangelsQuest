using UnityEngine;

public class PlayerTargetPoints : MonoBehaviour
{
    [SerializeField]
    private Transform leftTarget;
    public Transform LeftTarget => leftTarget;

    [SerializeField]
    private Transform rightTarget;
    public Transform RightTarget => rightTarget;
}
