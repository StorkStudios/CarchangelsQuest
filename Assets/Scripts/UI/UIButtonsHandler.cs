using UnityEngine;

public class UIButtonsHandler : MonoBehaviour
{
    [SerializeField]
    private UIMainMenuShineAnimation animation;
    [SerializeField]
    private GameObject creditsPane;
    [SerializeField]
    private GameObject settingsPane;
    [SerializeField]
    private GameObject mainMenuPane;

    private void Start()
    {
        UIAudioSettings audioSettings = settingsPane.GetComponentInChildren<UIAudioSettings>(true);
        audioSettings.OnEnable();
        audioSettings.OnDisable();
    }

    public void OnCredits() {
        creditsPane.SetActive(true);
        animation.currentTarget = UIMainMenuShineAnimation.Target.Left;
        this.CallDelayed(animation.AnimationDuration, () => mainMenuPane.SetActive(false));
    }

    public void OnSettings() {
        settingsPane.SetActive(true);
        animation.currentTarget = UIMainMenuShineAnimation.Target.Left;
        this.CallDelayed(animation.AnimationDuration, () => mainMenuPane.SetActive(false));
    }

    public void OnBack() {
        mainMenuPane.SetActive(true);
        animation.currentTarget = UIMainMenuShineAnimation.Target.Right;
        this.CallDelayed(animation.AnimationDuration, () =>
        {
            settingsPane.SetActive(false);
            creditsPane.SetActive(false);
        });
    }

    public void OnExit() => Application.Quit();
}
