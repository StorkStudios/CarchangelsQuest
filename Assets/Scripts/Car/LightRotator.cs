using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightRotator : MonoBehaviour
{

    [SerializeField]
    private float rotationDegreesPerSecond = 120;
    [SerializeField]
    private float dutyCycle = 0.5f;
    [SerializeField]
    private float cycleLength = 1;
    [SerializeField]
    private float cycleOffset = 0;
    private float randomCycleOffset;

    private Light2D current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current = GetComponent<Light2D>();
        var random = new System.Random(transform.parent.GetHashCode());
        randomCycleOffset =  (float)random.NextDouble() * cycleLength;
    }

    // Update is called once per frame
    void Update()
    {
        current.transform.Rotate(rotationDegreesPerSecond * Time.deltaTime * Vector3.forward);
        current.enabled = (((Time.time + cycleOffset + randomCycleOffset) % cycleLength) / cycleLength) < dutyCycle;
    }
}
