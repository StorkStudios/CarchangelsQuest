using UnityEngine;

public class UIPlayerPrefsChecker : MonoBehaviour
{
    [SerializeField]
    private string keyToCheck;

    private void Start()
    {
        bool active = PlayerPrefs.HasKey(keyToCheck) && PlayerPrefs.GetInt(keyToCheck, 0) == 1;
        gameObject.SetActive(active);
    }
}
