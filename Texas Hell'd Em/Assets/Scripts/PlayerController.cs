/******************************************************************************
// File Name:       PlayerController.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         Controls the player's actions. Includes finding the mouse
                    position to find where to target the shield.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CardController CardController;
    CardData nullCard = new CardData(-1, -1);
    CardData[] hand = { new CardData(-1, -1), new CardData(-1, -1)};
    int selectedCard;
    int handLevel;

    // <summary>
    // Currently used for debugging to print our current poker hand
    // </summary>
    public void Start()
    {
        logHand();
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
        for (int i = 0; i < hand.Length; i++) {
            if (hand[i].Equals(nullCard))
            {
                hand[i] = card;
                logHand();
                return;
            }
        }
        hand[selectedCard] = card;
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
}
