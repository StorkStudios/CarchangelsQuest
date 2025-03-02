using UnityEngine;

public class CarSoundModule : MonoBehaviour
{
    private enum EngineState { Running, Starting, Off }

    [Header("Engine")]
    [SerializeField]
    private AudioSource engineRunning;
    [SerializeField]
    private AudioSource engineStart;
    [SerializeField]
    private float engineTurnOffTime;
    [SerializeField]
    private float engineTurnOnDuration;
    [SerializeField]
    private float engineFadeDuration;
    [SerializeField]
    private AnimationCurve enginePitchBySpeed;

    [Header("Tires")]
    [SerializeField]
    private AudioSource tireScreech;

    [Header("Horn")]
    [SerializeField]
    private AudioSource horn;

    [Header("Collision")]
    [SerializeField]
    private PlayerCollisionsDetector collisionsDetector;

    [SerializeField]
    private AudioSource collision;

    private EngineState engineState = EngineState.Off;

    private CarEngine engine;

    private float engineTimestamp = 0;

    private void Start()
    {
        engine = GetComponentInParent<CarEngine>();
        engineRunning.pitch = enginePitchBySpeed.Evaluate(0);
        engine.IsDriftingChanged += OnIsDriftingChanged;
        engine.horn.ValueChanged += OnHornChanged;
        if (collisionsDetector != null)
        {
            collisionsDetector.CollisionEvent += () => collision.Play();
        }
    }

    private void OnHornChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            horn.Play();
        }
        else
        {
            horn.Stop();
        }
    }

    private void OnIsDriftingChanged(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            tireScreech.Play();
        }
        else
        {
            tireScreech.Stop();
        }
    }

    private void Update()
    {
        SimulateEngine();
    }

    private void SimulateEngine()
    {
        switch (engineState)
        {
            case EngineState.Running:
                if (Time.time - engineTimestamp >= engineTurnOffTime && engine.gasPedal.Value == 0)
                {
                    engineState = EngineState.Off;
                    engineRunning.Stop();
                    break;
                }
                float volumeTarget = 0;
                if (engine.gasPedal.Value != 0)
                {
                    engineTimestamp = Time.time;
                    volumeTarget = 1;
                }

                engineRunning.volume = Mathf.MoveTowards(engineRunning.volume, volumeTarget, Time.deltaTime / engineFadeDuration);
                engineRunning.pitch = enginePitchBySpeed.EvaluateUnclamped(engine.ForwardSpeed / engine.MaxSpeed);

                break;
            case EngineState.Starting:
                if (Time.time - engineTimestamp >= engineTurnOnDuration)
                {
                    engineState = EngineState.Running;
                    engineTimestamp = Time.time;
                    engineRunning.Play();
                }
                break;
            case EngineState.Off:
                if (engine.gasPedal.Value != 0)
                {
                    engineStart.Play();
                    engineState = EngineState.Starting;
                    engineTimestamp = Time.time;
                }
                break;
        }
    }
}
