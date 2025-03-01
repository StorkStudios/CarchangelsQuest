using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CarVoice : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> gameStartClips;

    [SerializeField]
    private List<AudioClip> randomClips;

    [SerializeField]
    private float clipDelay;

    private AudioSource audioSource;
    private IEnumerable<AudioClip> randomClipsShuffled;
    private IEnumerator<AudioClip> randomClipsEnumerator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(gameStartClips.GetRandomElement());
        randomClipsShuffled = randomClips.Shuffled();
        randomClipsEnumerator = randomClipsShuffled.GetEnumerator();
        randomClipsEnumerator.MoveNext();

        StartCoroutine(CarVoiceMainCoroutine());
    }

    private IEnumerator CarVoiceMainCoroutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            yield return new WaitForSeconds(clipDelay);

            audioSource.PlayOneShot(randomClipsEnumerator.Current);
            if (!randomClipsEnumerator.MoveNext())
            {
                randomClipsEnumerator = randomClipsShuffled.GetEnumerator();
                randomClipsEnumerator.MoveNext();
            }
        }
    }
}
