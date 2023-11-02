/******************************************************************************
// File Name:       GameController.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         The game manager. Controls various aspects of the game that
                    the player does not control.
******************************************************************************/
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
    //public TMP_Text scoreText;
    public int score;
    private bool game = true;

    public Queue<CardData> deck;
    private CardData[] river = new CardData[5];
    [SerializeField] private GameObject cardPF;

    // <summary>
    // Used to initialize our global variables
    // </summary>
    private void Start()
    {
        score = 0;
        //updateScoreText();
        deck = ShuffleDeck();
        for (int i = 0; i < 5; i++)
        {
            river[i] = deck.Dequeue();
        }

        // We will start a coroutine to create cards
        StartCoroutine(CreateACard());
    }

    // <summary>
    // Updates the score on the bottom left of the screen
    // </summary>
    public void updateScoreText()
    {
        //scoreText.text = score.ToString();
    }

    // <summary>
    // Creates a card based on an equation that will send enough cards
    // at an increasing interval
    // </summary>
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

    // <summary>
    // Shuffles a deck of playing cards to use to send to the player
    // </summary>
    static Queue<CardData> ShuffleDeck()
    {
        CardData[] allCards = new CardData[52];
        int deckIndex = 0;
        Queue<CardData> deck = new Queue<CardData>();

        for (int r = 0; r < 13; r++)
        {
            for (int s = 0; s < 4; s++)
            {
                allCards[deckIndex] = new CardData(r, s);
                deckIndex++;
            }
        }

        System.Random rng = new System.Random();

        for (int i = allCards.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            CardData temp = allCards[i];
            allCards[i] = allCards[j];
            allCards[j] = temp;
        }

        for (int i = 0; i < 52; i++)
        {
            deck.Enqueue(allCards[i]);
        }
        return deck;
    }
}
