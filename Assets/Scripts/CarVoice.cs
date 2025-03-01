using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CarVoice : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField]
    private List<AudioClip> gameStartClips;

    [SerializeField]
    private List<AudioClip> randomClips;

    [SerializeField]
    private List<AudioClip> loreClips;

    [Header("Config")]
    [SerializeField]
    private float loreChance;

    [SerializeField]
    private float clipDelay;

    private AudioSource audioSource;
    private IEnumerable<AudioClip> randomClipsShuffled;
    private IEnumerator<AudioClip> randomClipsEnumerator;
    private IEnumerable<AudioClip> loreClipsShuffled;
    private IEnumerator<AudioClip> loreClipsEnumerator;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(gameStartClips.GetRandomElement());
        randomClipsShuffled = randomClips.Shuffled();
        randomClipsEnumerator = randomClipsShuffled.GetEnumerator();
        randomClipsEnumerator.MoveNext();
        loreClipsShuffled = loreClips.Shuffled();
        loreClipsEnumerator = loreClipsShuffled.GetEnumerator();
        loreClipsEnumerator.MoveNext();

        StartCoroutine(CarVoiceMainCoroutine());
    }

    private IEnumerator CarVoiceMainCoroutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            yield return new WaitForSeconds(clipDelay);

            if (Random.value <= loreChance)
            {
                PlayNextClip(loreClipsShuffled, loreClipsEnumerator);
            }
            else
            {
                PlayNextClip(randomClipsShuffled, randomClipsEnumerator);
            }
        }
    }

    private void PlayNextClip(IEnumerable<AudioClip> enumerable, IEnumerator<AudioClip> enumerator)
    {
        audioSource.PlayOneShot(enumerator.Current);
        if (!enumerator.MoveNext())
        {
            enumerator = enumerable.GetEnumerator();
            enumerator.MoveNext();
        }
    }
}
