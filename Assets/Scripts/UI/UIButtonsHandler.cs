using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonsHandler : MonoBehaviour
{
    
    public void OnStartGame() {
        SceneManager.LoadScene("City");
    }
    public void OnCredits() {
        // TODO:
    }
    public void OnExit() {
        Application.Quit();
    }
}
