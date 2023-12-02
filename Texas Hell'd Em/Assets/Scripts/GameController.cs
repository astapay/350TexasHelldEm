using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private CardData[] handAI1;
    private CardData[] handAI2;
    private CardData[] handAI3;
    public int score;
    public int chipCounter = 0; // New variable to keep track of collected chips
    private bool game = true;

    public Queue<CardData> deck;
    private CardData[] river = new CardData[5];
    [SerializeField] private GameObject cardPF;
    [SerializeField] private GameObject[] chipPFs;
    private int chipValue;

    public bool paused;
    private int riverFlipperStage = 0;

    [SerializeField] private Button continueBtn;
    [SerializeField] private Button retryBtn;
    [SerializeField] private Button quitBtn;

    public float timer;

    [SerializeField] private TMP_Text timerText;

    private void Start()
    {
        timer = 20f;
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
        timerText.SetText(timer.ToString("0.00"));
    }

    public void updateScoreText(int chipValue)
    {
        scoreText.text = score.ToString();
    }

    public void updateChipCounterText()
    {
        chipCounterText.text =  chipCounter.ToString();
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
        for(int i = 0; i < 2;i++)
        {
            handAI1[i] = deck.Dequeue();
            handAI2[i] = deck.Dequeue();
            handAI3[i] = deck.Dequeue();

        }
    }

    IEnumerator spawnChips() {
        while (true)
        {
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            Vector2 chipPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10);
            GameObject chip = Instantiate(chipPFs[UnityEngine.Random.Range(0, 6)], chipPos, Quaternion.identity);
            chip.GetComponent<Rigidbody2D>().velocity = new Vector2(chipPos.x / -10, chipPos.y / -10);                  //moves chip toward player
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

    void RiverFlipper()
    {
        switch (riverFlipperStage)
        {
            case 0:
                break;
            case 1:
                for (int j = 0; j < 3; j++)
                {
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
        riverFlipperStage++;
    }


    public void TogglePaused()
    {
        if(paused == false)
        {
            paused = true;
            continueBtn.gameObject.SetActive(true);
            retryBtn.gameObject.SetActive(true);
            quitBtn.gameObject.SetActive(true);
        }
        else
        {
            paused = false;
            continueBtn.gameObject.SetActive(false);
            retryBtn.gameObject.SetActive(false);
            quitBtn.gameObject.SetActive(false);
        }
    }

    public bool IsPaused()
    {
        return paused;
    }

    public CardData[][] getAIHands()
    {
        return new CardData[][] { handAI1, handAI2, handAI3 };
    }

    public CardData[] getRiver()
    {
        return river;
    }

    private void FixedUpdate()
    {
        if(!paused)
        {
            timer = timer - .02f;
        }

        if (timer <= 20 - (riverFlipperStage * 5))
        {
            RiverFlipper();
        }

        if(timer <= 0)
        {
            SceneManager.LoadScene("CutScene");
        }
    }
}
