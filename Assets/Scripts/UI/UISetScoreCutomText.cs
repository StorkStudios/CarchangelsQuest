using System;
using TMPro;
using UnityEngine;

public class UISetScoreCutomText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI front;
    [SerializeField]
    private TextMeshProUGUI back;

    [SerializeField]
    private GameScore gameScore;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        front.text = back.text = ((int)gameScore.Score).ToString();
    }
}
