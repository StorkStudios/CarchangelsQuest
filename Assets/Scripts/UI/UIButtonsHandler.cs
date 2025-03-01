using UnityEngine;

public class UIButtonsHandler : MonoBehaviour
{

    [SerializeField]
    private GameObject creditsPane;
    [SerializeField]
    private GameObject settingsPane;
    [SerializeField]
    private GameObject mainMenuPane;

    public void OnCredits() {
        mainMenuPane.SetActive(false);
        creditsPane.SetActive(true);
    }
    public void OnSettings() {
        mainMenuPane.SetActive(false);
        settingsPane.SetActive(true);
    }

    public void OnBack() {
        mainMenuPane.SetActive(true);
        settingsPane.SetActive(false);
        creditsPane.SetActive(false);
    }
    public void OnExit() => Application.Quit();
}
