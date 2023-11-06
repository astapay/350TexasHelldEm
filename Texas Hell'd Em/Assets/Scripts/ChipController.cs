using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.VFX;

public class ChipController : MonoBehaviour
{
    public int chipValue = 10; 
    private GameController gameController;
    
    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sheild"))
        {
            // Check if the chip collided with the shield side of the player
            if (IsCollidingWithShield(collision))
            {
                // Increase the chip counter
                gameController.UpdateChipCounter(chipValue);

                // Destroy the chip
                Destroy(gameObject);
            }
            else
            {
                // Handle cases where the chip collides with the player but not the shield
                // For example, inflict damage to the player or handle other game mechanics.
                // For now, destroy the chip.
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Collector"))
        {
            // Subtract points corresponding to the chip
            // GameController.updateScoreText();
            Destroy(gameObject);
        }
    }

    private bool IsCollidingWithShield(Collision2D collision)
    {
        return collision.transform.position.x > transform.position.x;
    }
}

