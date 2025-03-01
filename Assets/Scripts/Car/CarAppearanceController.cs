using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CarAppearanceController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> frontWheels;

    [SerializeField]
    private List<Light2D> backLights;


    [SerializeField]
    private float backLightIntensityBreaking = 7;
    [SerializeField]
    private float backLightIntensityNormal = 3;


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
            carEngine.IsBreakingChanged += OnIsBreakingValueChanged;
            carEngine.steeringWheel.ValueChanged += OnSteeringWheelValueChanged;
        }
        foreach (Light2D light in backLights) 
        {
            light.intensity = backLightIntensityNormal;
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
    private void OnIsBreakingValueChanged(bool oldValue, bool newValue) 
    {
        foreach (Light2D light in backLights) 
        {
            light.intensity = newValue ? backLightIntensityBreaking : backLightIntensityNormal;
        }
    }
}
