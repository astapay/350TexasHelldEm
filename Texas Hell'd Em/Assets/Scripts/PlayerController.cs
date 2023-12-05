/******************************************************************************
// File Name:       PlayerController.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         Controls the player's actions. Includes finding the mouse
                    position to find where to target the shield.
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CardController CardController;
    public PlayerInput playerInput;
    private InputAction chooseLeft;
    private InputAction chooseRight;
    private InputAction pauseToggle;
    private InputAction quit;
    CardData nullCard = new CardData(-1, -1);

    CardData[] hand = new CardData[7];
    int selectedCard;
    int handLevel;

    //card selector objects
    [SerializeField] private GameObject RightCardIndicator;
    [SerializeField] private GameObject LeftCardIndicator;
    private GameController gameController;

    /// <summary>
    /// Currently used for debugging to print our current poker hand
    /// </summary>
    public void Start()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i] = nullCard;
        }
        playerInput = GetComponent<PlayerInput>();
        chooseLeft = playerInput.currentActionMap.FindAction("SelectCardZero");
        chooseRight = playerInput.currentActionMap.FindAction("SelectCardOne");
        pauseToggle = playerInput.currentActionMap.FindAction("PauseToggle");
        quit = playerInput.currentActionMap.FindAction("Quit");
        chooseLeft.performed += chooseLeftPerformed;
        chooseRight.performed += chooseRightPerformed;
        pauseToggle.started += PauseToggleStarted;
        quit.started += QuitStarted;
        RightCardIndicator.SetActive(false);
        LeftCardIndicator.SetActive(true);
        gameController = FindObjectOfType<GameController>();
    }

    /// <summary>
    /// if quit has started
    /// </summary>
    /// <param name="obj"></param>
    private void QuitStarted(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }

    /// <summary>
    /// if pause has started
    /// </summary>
    /// <param name="obj"></param>
    private void PauseToggleStarted(InputAction.CallbackContext obj)
    {
        bool isPaused = gameController.IsPaused();
        if (isPaused == true)
        {
            gameController.TogglePaused();
        }
        else{
            gameController.TogglePaused();
        }
    }

    /// <summary>
    /// selects the right indicator
    /// </summary>
    /// <param name="context"></param>
    private void chooseRightPerformed(InputAction.CallbackContext context)
    {
        selectedCard = 1;
        Debug.Log(selectedCard);
        RightCardIndicator.SetActive(true);
        LeftCardIndicator.SetActive(false);
    }

    /// <summary>
    /// selects the left indicator
    /// </summary>
    /// <param name="context"></param>
    private void chooseLeftPerformed(InputAction.CallbackContext context)
    {
        selectedCard = 0;
        Debug.Log(selectedCard);
        RightCardIndicator.SetActive(false);
        LeftCardIndicator.SetActive(true);
    }


    // <summary>
    //returns the Player's hand array
    // </summary>
    public CardData[] getHand() {
        return hand;
    }

    // <summary>
    // Used to rotate the player's shield in the direction of the mouse
    // </summary>
    private void Update()
    {
        // Capture the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));

        // Calculate the angle between the object and the mouse
        float angle = Mathf.Atan2(worldMousePosition.y - transform.position.y, worldMousePosition.x - transform.position.x) * Mathf.Rad2Deg - 90;

        // Apply the angle to the object's z rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // <summary>
    // Adds a card to the player's hand
    // </summary>
    // <param name="card">
    // The card to be added to the hand
    // </param>
    public void addToHand(CardData card) {
        for (int i = 0; i < 2; i++) {
            if (hand[i].Equals(nullCard))
            {
                hand[i] = card;
                gameController.updateHandSprite(i, card);
                return;
            }
        }
        hand[selectedCard] = card;
        gameController.updateHandSprite(selectedCard, card);
    }

    // <summary>
    // Prints the current contents of the hand to the console
    // Used for debugging
    // </summary>
    private void logHand() {
        for (int i = 0; i < hand.Length; i++)
        {
            Debug.Log("Card slot " + i + ": " + hand[i].getRankName() + " of " + hand[i].getSuitName());
        }
    }

    /// <summary>
    /// called on destroy to clean
    /// </summary>
    private void OnDestroy()
    {
        chooseLeft.performed -= chooseLeftPerformed;
        chooseRight.performed -= chooseRightPerformed;
        pauseToggle.started -= PauseToggleStarted;
        quit.started -= QuitStarted;
    }

}
