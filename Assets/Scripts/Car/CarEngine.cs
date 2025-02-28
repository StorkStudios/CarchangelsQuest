using UnityEngine;

public class CarEngine : MonoBehaviour
{
    [SerializeField]
    private float accelerationForce;
    [SerializeField]
    private float steeringForce;
    [SerializeField]
    private float breakForce;

    public readonly ObservableVariable<float> steerWheel = new ObservableVariable<float>(0);
    public readonly ObservableVariable<float> gasPedal = new ObservableVariable<float>(0);
    public readonly ObservableVariable<bool> handbreak = new ObservableVariable<bool>(false);

}
