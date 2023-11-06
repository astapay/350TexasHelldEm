using System;
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
    public TMP_Text chipCounterText; // Add a Text component to display the chip counter
    public int score;
    private int chipCounter = 0; // New variable to keep track of collected chips
    private bool game = true;

    public Queue<CardData> deck;
    private CardData[] river = new CardData[5];
    [SerializeField] private GameObject cardPF;

    private void Start()
    {
        score = 0;
        updateScoreText();
        updateChipCounterText(); // Initialize the chip counter text
        deck = ShuffleDeck();
        for (int i = 0; i < 5; i++)
        {
            river[i] = deck.Dequeue();
        }
        StartCoroutine(CreateACard());
    }

    public void updateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void updateChipCounterText()
    {
        chipCounterText.text = "Chips: " + chipCounter.ToString();
    }

    public void UpdateChipCounter(int chipValue)
    {
        chipCounter += chipValue;
        updateChipCounterText(); // Update the chip counter text when it changes
    }

    IEnumerator CreateACard()
    {
        int spawns = 0;
        while (game)
        {
            CardController.createCard(deck);
            spawns++;

            yield return new WaitForSeconds(MathF.Exp((spawns - 1) / 20));
        }
    }
}
