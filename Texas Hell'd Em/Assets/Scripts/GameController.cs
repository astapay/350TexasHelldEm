using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public CardController CardController;
    public ChipController ChipController;
    public PlayerController PlayerController;
    public TMP_Text scoreText;
    public int score;

    private void Start()
    {
        score = 0;
        updateScoreText();
    }

    public void updateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
