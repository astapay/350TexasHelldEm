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
    public int score;
    private bool game = true;

    [SerializeField] private Queue<Card> deck;
    private Card[] river = new Card[5];
    [SerializeField] private GameObject cardPF;


    private void Start()
    {
        score = 0;
        updateScoreText();
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

    static Queue<Card> ShuffleDeck()
    {
        Card[] allCards = new Card[52];
        int deckIndex = 0;
        Queue<Card> deck = new Queue<Card>();

        for (int r = 0; r < 13; r++)
        {
            for (int s = 0; s < 4; s++)
            {
                allCards[deckIndex] = new Card(r, s);
                deckIndex++;
            }
        }

        System.Random rng = new System.Random();

        for (int i = allCards.Length - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            Card temp = allCards[i];
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
