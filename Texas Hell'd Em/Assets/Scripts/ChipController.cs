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

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

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
            gameController.updateScoreText(chipValue);
            Destroy(gameObject);
        }
    }

    private bool IsCollidingWithShield(Collision2D collision)
    {
        return collision.transform.position.x > transform.position.x;
    }

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
            // Add cases for other chip colors as needed
            default:
                return 0;
        }
    }
}

