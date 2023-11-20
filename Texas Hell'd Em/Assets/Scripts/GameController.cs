using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject[] chipPFs;
    private int chipValue;

    public bool paused;

    [SerializeField] private Button continueBtn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button quitBtn;

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
        StartCoroutine(spawnChips());

        continueBtn.gameObject.SetActive(false);
        retryBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
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
        Debug.Log(spawns);
    }

    IEnumerator spawnChips() {
        while (true)
        {
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            Vector2 chipPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10);
            GameObject chip = Instantiate(chipPFs[UnityEngine.Random.Range(0, 6)], chipPos, Quaternion.identity);
            chip.GetComponent<Rigidbody2D>().velocity = new Vector2(chipPos.x / -10, chipPos.y / -10);                  //moves card toward player
            yield return new WaitForSeconds(.5f);
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

    IEnumerator RiverFlipper()
    {
        for(int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    for (int j = 0; j < 3; j++) {
                        riverUI[j].GetComponent<SpriteRenderer>().sprite = CardController.getCardSprites()[river[j].getSuit() * 13 + river[j].getRank()];
                    }
                    break;
                case 2:
                    riverUI[3].GetComponent<SpriteRenderer>().sprite = CardController.getCardSprites()[river[3].getSuit() * 13 + river[3].getRank()];

                    break;
                case 3:
                    riverUI[4].GetComponent<SpriteRenderer>().sprite = CardController.getCardSprites()[river[4].getSuit() * 13 + river[4].getRank()];
                    game = false;
                    break;
            }
            yield return new WaitForSeconds(5);
        }
        Debug.Log("Game end");
    }


    public void SetPaused(bool pauseStatus)
    {
        paused = pauseStatus;
        if(paused == true)
        {
            continueBtn.gameObject.SetActive(true);
            retryBtn.gameObject.SetActive(true);
            quitBtn.gameObject.SetActive(true);
        }
        else
        {
            continueBtn.gameObject.SetActive(false);
            retryBtn.gameObject.SetActive(false);
            quitBtn.gameObject.SetActive(false);
        }
    }

    public bool GetPaused()
    {
        return paused;
    }
}
