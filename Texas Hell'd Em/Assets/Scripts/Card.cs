/******************************************************************************
// File Name:       Card.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         Represents a playing card
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData cardStruct;
    private GameController gameController;
    private PlayerController playerController;
    private Vector2 storedVelocity;

    // <summary>
    // Start is called before the first frame update
    // Used to initialize our global variables
    // </summary>
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // <summary>
    // Update checks if the game has been paused or unpaused, and stops/starts the card's velocity accordingly
    // </summary>
    private void Update()
    {
        if (gameController.IsPaused() && gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            storedVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if (!gameController.IsPaused() && gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = storedVelocity;
        }
    }

    // <summary>
    // Called when this object collides with another object
    // Used for logic when blocking or grabbing the card
    // <summary>
    // <param name="collision">
    // Information as to what caused the collision
    // </param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // First check for shield
        if (collision.gameObject.tag == "Shield")
        {
            gameController.deck.Enqueue(cardStruct);
            Destroy(gameObject);
        }
        // Then the collector
        else if (collision.gameObject.tag == "Collector")
        {
            playerController.addToHand(cardStruct);
            Destroy(gameObject);
        }
        
    }

    // <summary>
    // Sets the data for this card
    // </summary>
    // <param name="cardData">
    // The data to be stored in this card
    // </param>
    public void setCardData(CardData cardData) { 
        cardStruct = cardData;
    }
}
