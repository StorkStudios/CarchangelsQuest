using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    [System.Serializable]
    private class Scene
    {
        public Sprite background;
        public AudioClip voiceover;
        [TextArea]
        public string text;
        public Vector2 delays;
    }

    [SerializeField]
    private List<Scene> scenes;
    [SerializeField]
    private string sceneToLoad;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Image courtain;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private float textFadeDuration;
    [SerializeField]
    private float backgroundFadeDuration;

    private void Start()
    {
        PlayerPrefs.SetInt("CutsceneWatched", 1);
        PlayerPrefs.Save();
        StartCoroutine(CutsceneCoroutine());
    }

    private IEnumerator CutsceneCoroutine()
    {
        foreach (var scene in scenes)
        {
            background.sprite = scene.background;
            audioSource.clip = scene.voiceover;
            text.text = scene.text;

            yield return courtain.DOFade(0, backgroundFadeDuration / 2).WaitForCompletion();

            yield return new WaitForSeconds(scene.delays.x);

            canvasGroup.DOFade(1, textFadeDuration);
            audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);

            canvasGroup.DOFade(0, textFadeDuration);

            yield return new WaitForSeconds(scene.delays.y);

            yield return courtain.DOFade(1, backgroundFadeDuration / 2).WaitForCompletion();
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
