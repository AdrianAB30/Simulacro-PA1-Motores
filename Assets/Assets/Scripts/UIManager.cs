using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;

    private void Start()
    {
        UpdateScore();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }
    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
