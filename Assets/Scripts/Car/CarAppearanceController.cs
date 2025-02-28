using System.Collections.Generic;
using UnityEngine;

public class CarAppearanceController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> frontWheels;
    [SerializeField]
    private float wheelSteerSpeed;
    [SerializeField]
    private float wheelSteerAngle;

    private float steerAngleTarget = 0;

    private void Start()
    {
        CarEngine carEngine = GetComponentInParent<CarEngine>();
        if (carEngine != null)
        {
            carEngine.steeringWheel.ValueChanged += OnSteeringWheelValueChanged;
        }
    }

    private void Update()
    {
        foreach (Transform wheel in frontWheels)
        {
            var localEuler = wheel.localEulerAngles;
            localEuler.z = Mathf.MoveTowardsAngle(localEuler.z, steerAngleTarget, wheelSteerSpeed * Time.deltaTime);
            wheel.localEulerAngles = localEuler;
        }
    }

    private void OnSteeringWheelValueChanged(float oldValue, float newValue)
    {
        steerAngleTarget = newValue * wheelSteerAngle;
    }
}
