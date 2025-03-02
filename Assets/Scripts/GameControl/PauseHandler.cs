using UnityEngine;

public class PauseHandler : Singleton<PauseHandler>
{
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private AudioSource audioSource;

    public bool IsPaused { get; private set; } = false;

    public void TogglePause()
    {
        if (GameEndManager.Instance.HasGameEnded)
        {
            return;
        }

        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;

        pausePanel.SetActive(IsPaused);
        audioSource.Play();
    }
}
