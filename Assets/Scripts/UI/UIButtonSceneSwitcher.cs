using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonSceneSwitcher : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    public void OnButtonPress() => SceneManager.LoadScene(sceneName);
}
