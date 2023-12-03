/******************************************************************************
// File Name:       GameController.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         Controls the majority of the game and the ui
******************************************************************************/
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

    [SerializeField] private GameObject cutScene;

    /// <summary>
    /// called on start, used to set vaiables and start ruitines
    /// </summary>
    private void Start()
    {
        cutScene.SetActive(false);
        timer = 20f;
        score = 0;
        updateScoreText();
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

    /// <summary>
    /// called every fram, used to get hand and set timer
    /// </summary>
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

    /// <summary>
    /// updates score text
    /// </summary>
    public void updateScoreText()
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// used to update the chip counter text
    /// </summary>
    public void updateChipCounterText()
    {
        chipCounterText.text =  chipCounter.ToString();
    }

    /// <summary>
    /// used to update the chip counter variable, to update the text
    /// </summary>
    /// <param name="chipValue"></param> value added to chipcounter
    public void UpdateChipCounter(int chipValue)
    {
        chipCounter += chipValue;
        updateChipCounterText(); // Update the chip counter text when it changes
    }

    /// <summary>
    /// creates a card
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateACard()
    {
        int spawns = 0;
        while (game)
        {
            if (paused)
            {
                yield return new WaitForNextFrameUnit();
            }
            else
            {
                CardController.createCard(deck);
                spawns++;

                yield return new WaitForSeconds(MathF.Exp((spawns - 1) / 20));
            }
        }
        for(int i = 0; i < 2;i++)
        {
            handAI1[i] = deck.Dequeue();
            handAI2[i] = deck.Dequeue();
            handAI3[i] = deck.Dequeue();

        }
    }

    /// <summary>
    /// creates a chip
    /// </summary>
    /// <returns></returns> 
    IEnumerator spawnChips() {
        while (game)
        {
            if (paused)
            {
                yield return new WaitForNextFrameUnit();
            }
            else
            {
                float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
                Vector2 chipPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10);
                GameObject chip = Instantiate(chipPFs[UnityEngine.Random.Range(0, 6)], chipPos, Quaternion.identity);
                chip.GetComponent<Rigidbody2D>().velocity = new Vector2(chipPos.x / -10, chipPos.y / -10);                  //moves chip toward player
                yield return new WaitForSeconds(.5f);
            }
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

    /// <summary>
    /// used to flip the river on time
    /// </summary>
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

    /// <summary>
    /// toggles the pause bool and changes based on bool
    /// </summary>
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

    /// <summary>
    /// returns if the game is paused or not
    /// </summary>
    /// <returns></returns>
    public bool IsPaused()
    {
        return paused;
    }

    /// <summary>
    /// returns the hands of the ai
    /// </summary>
    /// <returns></returns> hand of ai
    public CardData[][] getAIHands()
    {
        return new CardData[][] { handAI1, handAI2, handAI3 };
    }

    /// <summary>
    /// returns the river
    /// </summary>
    /// <returns></returns> returns the river cards
    public CardData[] getRiver()
    {
        return river;
    }

    /// <summary>
    /// called on game end, updates ui
    /// </summary>
    private void GameEnd()
    {
        paused = true;
        continueBtn.gameObject.SetActive(false);
        retryBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        chipCounterText.gameObject.SetActive(false);
}


    /// <summary>
    /// called 50 times a second, used to update counter, flip river, and end game
    /// </summary>
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
        else if(timer <= 0)
        {
            GameEnd();
            cutScene.SetActive(true);
            game = false;
            cutScene.GetComponent<CutUiScript>().Activate();
        }
    }
}
