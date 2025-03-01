using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : Singleton<ScoreKeeper>
{
    private readonly ObservableVariable<float> score = new(0);

    private readonly ObservableVariable<int> humansKilled = new(0);

    private readonly ObservableVariable<float> averageSpeed = new(0);

    private readonly LinkedList<float> speedHistory = new();
    private float speedHistorySum = 0;

    [SerializeField]
    private float scorePerHumanKilled = 1;
    [SerializeField]
    private float scorePerAverageSpeed = 1;

    [SerializeField]
    private float speedWindowSeconds = 5;
    
    [SerializeField]
    private PlayerKillDetector playerKillDetector;
    [SerializeField]
    private Rigidbody2D carBody;

    void Start()
    {
        playerKillDetector.HumanKilled += () => {
            humansKilled.Value += 1;
            score.Value += scorePerHumanKilled;
        };
    }

    void FixedUpdate()
    {
        int maxWindowSize = Math.Max((int)(speedWindowSeconds / Time.fixedDeltaTime), 1);
        
        float currentSpeed = carBody.linearVelocity.magnitude;

        speedHistorySum += currentSpeed;
        speedHistory.AddFirst(currentSpeed);

        while (speedHistory.Count > maxWindowSize) {
            float poppedSpeed = speedHistory.Last.Value;

            speedHistorySum -= poppedSpeed;
            speedHistory.RemoveLast();
        }
        
        averageSpeed.Value = speedHistorySum / speedHistory.Count;

        score.Value += averageSpeed.Value * scorePerAverageSpeed / Time.fixedDeltaTime;
    }


    public event ObservableVariable<float>.ValueChangedDelegate scoreChanged
    {
        add => score.ValueChanged += value;
        remove => score.ValueChanged -= value;
    }

    public float Score => score.Value;


    public event ObservableVariable<int>.ValueChangedDelegate humansKilledChanged
    {
        add => humansKilled.ValueChanged += value;
        remove => humansKilled.ValueChanged -= value;
    }

    public int HumanKilled => humansKilled.Value;

    public event ObservableVariable<float>.ValueChangedDelegate averageSpeedChanged
    {
        add => averageSpeed.ValueChanged += value;
        remove => averageSpeed.ValueChanged -= value;
    }

    public float AverageSpeed => averageSpeed.Value;
}
