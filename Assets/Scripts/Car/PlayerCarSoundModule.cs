using System.Collections.Generic;
using UnityEngine;

public class PlayerCarSoundModule : MonoBehaviour
{
    [SerializeField]
    private AudioSource collision;
    [SerializeField]
    private AudioSource personDeath;
    [SerializeField]
    private List<AudioClip> deathSounds;

    private PlayerCollisionsDetector collisionsDetector;

    private void Start()
    {
        collisionsDetector = GetComponentInParent<PlayerCollisionsDetector>();
        collisionsDetector.CollisionEvent += () => collision.Play();
        collisionsDetector.HumanHit += () => personDeath.PlayOneShot(deathSounds.GetRandomElement());
    }
}
