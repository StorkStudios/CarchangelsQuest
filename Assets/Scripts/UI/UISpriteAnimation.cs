using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISpriteAnimation : MonoBehaviour
{
    private Image image;

    
    [Serializable]
    struct AnimationFrame {
        public Sprite sprite;
        public float delay;
    }

    [SerializeField]
    private List<AnimationFrame> animationFrames = new();

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        StartCoroutine(Animate());
    }

    public IEnumerator Animate() {
        while (true) {
            foreach(AnimationFrame animationFrame in animationFrames) {
                image.sprite = animationFrame.sprite;
                yield return new WaitForSeconds(animationFrame.delay);
            }
        }
    }

}
