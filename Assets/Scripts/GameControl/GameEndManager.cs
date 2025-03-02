using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : Singleton<GameEndManager>
{
    [SerializeField]
    private GameScore gameScore;
    [SerializeField]
    private float timeScaleAnimationDuration;
    [SerializeField]
    private float timeToGameOverScene;
    [SerializeField]
    private string gameOverSceneName;

    public bool HasGameEnded { get; private set; }

    public event Action GameEnded;

    private ScoreKeeper scoreKeeper;
    private PlayerLoser playerLoser;

    private void Start()
    {
        gameScore.Score = 0;

        scoreKeeper = ScoreKeeper.Instance;
        playerLoser = FindAnyObjectByType<PlayerLoser>();

        playerLoser.CatchProgessChanged += OnCatchProgressChanged;
    }

    private void OnCatchProgressChanged(float oldValue, float newValue) 
    {
        if (newValue >= 1) 
        {
            EndGame();
        }
    }

    private void Update()
    {
        if (HasGameEnded)
        {
            Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0, Time.unscaledDeltaTime / timeScaleAnimationDuration);
        }
    }

    private void EndGame()
    {
        HasGameEnded = true;
        playerLoser.Lock();
        gameScore.Score = scoreKeeper.Score;
        GameEnded?.Invoke();
        StartCoroutine(LoadGameEndScene());
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        HasGameEnded = false;
        Time.timeScale = 1;
    }

    private IEnumerator LoadGameEndScene()
    {
        yield return new WaitForSecondsRealtime(timeToGameOverScene);

        SceneManager.LoadScene(gameOverSceneName);
    }
}
