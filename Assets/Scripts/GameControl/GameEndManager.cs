using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEndManager : MonoBehaviour
{
    [SerializeField]
    private GameScore gameScore;
    [SerializeField]
    private ScoreKeeper scoreKeeper;
    [SerializeField]
    private PlayerLoser playerLoser;
    [SerializeField]
    private bool hasGameEnded = false;

    void Start()
    {
        gameScore.Score = 0;
        playerLoser.CatchProgessChanged += OnCatchProgressChanged;
    }

    void OnCatchProgressChanged(float oldValue, float newValue) {
        if (newValue >= 1) {
            playerLoser.Lock();

            gameScore.Score = scoreKeeper.Score;
            hasGameEnded = true;
            GameEnded?.Invoke(this, new EventArgs());
        }
    }

     public event EventHandler GameEnded;

     public bool HasGameEnded => hasGameEnded;
}
