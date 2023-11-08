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
    public GameObject[] handUI;
    public GameObject[] riverUI;
    public int score;
    private int chipCounter = 0; // New variable to keep track of collected chips
    private bool game = true;

    public Queue<CardData> deck;
    private CardData[] river = new CardData[5];
    [SerializeField] private GameObject cardPF;
    private int chipValue;

    private void Start()
    {
        score = 0;
        updateScoreText(chipValue);
        updateChipCounterText(); // Initialize the chip counter text
        deck = ShuffleDeck();
        for (int i = 0; i < 5; i++)
        {
            river[i] = deck.Dequeue();
        }
        StartCoroutine(CreateACard());
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            if (!PlayerController.getHand()[i].isNull())
            {
                handUI[i].GetComponent<SpriteRenderer>().sprite = CardController.getCardSprites()[PlayerController.getHand()[i].getSuit() * 13 + PlayerController.getHand()[i].getRank()];
            }
        }
    }

    public void updateScoreText(int chipValue)
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
