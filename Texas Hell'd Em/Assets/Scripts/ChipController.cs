/******************************************************************************
// File Name:       ChipController.cs
// Author:          Dalton Mullis 
// Creation date:   10/21/2023
// Summary:         Contains a chipData struct to store information for a chjip.
                    Also contains code to instantiate each card in the scene.
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.VFX;

public class ChipController : MonoBehaviour
{
    private GameController gameController;

    public float speed = 50.0f;
    
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite[] chipSprites;
    [SerializeField] private GameObject RedChipPF;
    [SerializeField] private GameObject GreenChipPF;
    [SerializeField] private GameObject BlackChipPF;
    [SerializeField] private GameObject PurpleChipPF;
    [SerializeField] private GameObject BlueChipPF;
    [SerializeField] private GameObject WhiteChipPF;

    private Vector2 storedVelocity;
    [SerializeField] private bool isCardController;

    /// <summary>
    /// called on game start
    /// </summary>
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    /// <summary>
    /// Update checks if the game has been paused or unpaused, and stops/starts the chip's velocity accordingly
    /// </summary>
    private void Update()
    {
        if (!isCardController && gameController.IsPaused() && gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            storedVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if (!isCardController && !gameController.IsPaused() && gameObject.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = storedVelocity;
        }
    }

    /// <summary>
    /// updates score text
    /// </summary>
    /// <param name="playerTransform"></param> what direction the player is facing
    /// <returns></returns> 
    private IEnumerator updateScoreText(Transform playerTransform)
    {
        float timeToReachPlayer = Vector3.Distance(transform.position, playerTransform.position) / speed;
        float elapsedTime = 0f;

        while (elapsedTime < timeToReachPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, elapsedTime / timeToReachPlayer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        int chipValue = GetChipValueByTag();

        gameController.updateScoreText();

        Destroy(gameObject);
    }

    /// <summary>
    /// called on collision
    /// </summary>
    /// <param name="collision"></param> the collision information
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            if (IsCollidingWithShield(collision))
            {
                int chipValue = GetChipValueByTag();
                gameController.UpdateChipCounter(chipValue);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Collector"))
        {
            int chipValue = GetChipValueByTag();
                gameController.updateScoreText();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// sets bool if coliding with player sheild
    /// </summary>
    /// <param name="collision"></param> collision deets
    /// <returns></returns> if collides with shield
    private bool IsCollidingWithShield(Collision2D collision)
    {
        return collision.transform.position.x > transform.position.x;
    }

    /// <summary>
    /// returns chip value based on the tag it has
    /// </summary>
    /// <returns></returns> chip value
    private int GetChipValueByTag()
    {
        // Return the chip value based on the chip's tag
        switch (gameObject.tag)
        {
            case "WhiteChip":
                return 10;
            case "RedChip":
                return 20;
            case "PurpleChip":
                return 30;
            case "GreenChip":
                return 40;
            case "BlackChip":
                return 50;
            case "BlueChip":
                return 60;
            default:
                return 0;
        }
    }
}

