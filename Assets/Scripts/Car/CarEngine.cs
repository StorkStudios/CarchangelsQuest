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
    private float velocitySteerDivider;
    [SerializeField]
    private float handbreakLinearDamping;
    [SerializeField]
    private float maxForwardSpeed;
    [SerializeField]
    private float maxReverseSpeed;

    public readonly ObservableVariable<float> steeringWheel = new ObservableVariable<float>(0);
    public readonly ObservableVariable<float> gasPedal = new ObservableVariable<float>(0);
    public readonly ObservableVariable<bool> handbreak = new ObservableVariable<bool>(false);

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

        float carSteerAmount = Mathf.Clamp01(rigidbody.linearVelocity.magnitude / velocitySteerDivider);
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

        if (handbreak.Value)
        {
            return;
        }

        Vector2 engineForce = transform.up * accelerationForce * gasPedal.Value;
        rigidbody.AddForce(engineForce);
    }
}
