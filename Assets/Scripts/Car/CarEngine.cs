using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarEngine : MonoBehaviour
{
    [SerializeField]
    private float accelerationForce;
    [SerializeField]
    private float steeringForce;
    [SerializeField]
    private float driftFactor;
    [SerializeField]
    private float breakDriftFactor;
    [SerializeField]
    private AnimationCurve steeringCurve;
    [SerializeField]
    private float handbreakLinearDamping;
    [SerializeField]
    private float handbreakGasMultiplier;
    [SerializeField]
    private float maxForwardSpeed;
    [SerializeField]
    private float maxReverseSpeed;
    [SerializeField]
    private float minLatheralDriftDetectionSpeed;

    [SerializeField]
    private float breakingSpeedThreshold = 2;

    public readonly ObservableVariable<float> steeringWheel = new ObservableVariable<float>(0);
    public readonly ObservableVariable<float> gasPedal = new ObservableVariable<float>(0);
    public readonly ObservableVariable<bool> handbreak = new ObservableVariable<bool>(false);

    public event ObservableVariable<bool>.ValueChangedDelegate IsDriftingChanged
    {
        add => isDrifting.ValueChanged += value;
        remove => isDrifting.ValueChanged -= value;
    }

    public bool IsDrifting => isDrifting.Value;

    private readonly ObservableVariable<bool> isDrifting = new ObservableVariable<bool>(false);
    
    public event ObservableVariable<bool>.ValueChangedDelegate IsBreakingChanged
    {
        add => isBreaking.ValueChanged += value;
        remove => isBreaking.ValueChanged -= value;
    }

    public bool IsBreaking => isBreaking.Value;

    private readonly ObservableVariable<bool> isBreaking = new ObservableVariable<bool>(false);
    

    private float rotationAngle;
    private float baseDamping;

    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rotationAngle = rigidbody.rotation;
        baseDamping = rigidbody.linearDamping;
    }

    private void FixedUpdate()
    {
        if (handbreak.Value)
        {
            rigidbody.linearDamping = Mathf.Lerp(rigidbody.linearDamping, handbreakLinearDamping, Time.fixedDeltaTime * 3);
        }
        else
        {
            rigidbody.linearDamping = baseDamping;
        }

        ApplyEngineForce();

        float forwardSpeed = Vector2.Dot(rigidbody.linearVelocity, transform.up);
        Vector2 forwardVelocity = transform.up * forwardSpeed;
        Vector2 lateralVelocity = transform.right * Vector2.Dot(rigidbody.linearVelocity, transform.right);
        Vector2 newLateralVelocity = lateralVelocity * (handbreak.Value ? breakDriftFactor : driftFactor);
        rigidbody.linearVelocity = forwardVelocity + newLateralVelocity;

        float carSteerAmount = steeringCurve.EvaluateUnclamped(rigidbody.linearVelocity.magnitude);
        float rotationAmount = steeringWheel.Value * steeringForce * carSteerAmount;
        if (forwardSpeed >= 0)
        {
            rotationAngle += rotationAmount;
        }
        else
        {
            rotationAngle -= rotationAmount;
        }
        rigidbody.MoveRotation(rotationAngle);

        isBreaking.Value = CheckIsBreaking();
        isDrifting.Value = isBreaking.Value || CheckDrift();
    }

    private void ApplyEngineForce()
    {
        float speed = Vector2.Dot(rigidbody.linearVelocity, transform.up);

        if (speed >= maxForwardSpeed && gasPedal.Value > 0)
        {
            return;
        }

        if (speed <= -maxReverseSpeed && gasPedal.Value < 0)
        {
            return;
        }

        Vector2 engineForce = transform.up * accelerationForce * gasPedal.Value;
        engineForce *= (handbreak.Value ? handbreakGasMultiplier : 1);
        rigidbody.AddForce(engineForce);
    }

    private bool CheckDrift()
    {
        float lateralSpeed = Vector2.Dot(rigidbody.linearVelocity, transform.right);

        if (handbreak.Value)
        {
            return true;
        }

        if (Mathf.Abs(lateralSpeed) > minLatheralDriftDetectionSpeed)
        {
            return true;
        }

        return false;
    }

    private bool CheckIsBreaking()
    {
        float forwardSpeed = Vector2.Dot(rigidbody.linearVelocity, transform.up);
        return ((forwardSpeed > 0 && gasPedal.Value < 0) ||
            (forwardSpeed < 0 && gasPedal.Value > 0)) && rigidbody.linearVelocity.magnitude > breakingSpeedThreshold;
    }
}
