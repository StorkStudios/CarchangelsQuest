using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [SerializeField]
    private float accelerationForce;
    [SerializeField]
    private float steeringForce;
    [SerializeField]
    private float breakForce;

    public readonly ValueWithEvents<float> steerWheel = new ValueWithEvents<float>(0);
    public readonly ValueWithEvents<float> gasPedal = new ValueWithEvents<float>(0);
    public readonly ValueWithEvents<bool> handbreak = new ValueWithEvents<bool>(false);

}
