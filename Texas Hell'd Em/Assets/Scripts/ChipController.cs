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

    public Sprite[] sprites;

    public float speed = 50.0f;

    public void SpawnAndAttackPlayer(Vector3 spawnPosition, Transform playerTransform)
    {
        GameObject chip = Instantiate(gameObject, spawnPosition, Quaternion.identity);
        ChipController chipController = chip.GetComponent<ChipController>();
        chipController.StartCoroutine(AttackPlayer(playerTransform));
    }

    private IEnumerator AttackPlayer(Transform playerTransform)
    {
        float timeToReachPlayer = Vector3.Distance(transform.position, playerTransform.position) / speed;
        float elapsedTime = 0f;

        while (elapsedTime < timeToReachPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, elapsedTime / timeToReachPlayer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Chip has reached the player, apply damage or do other actions
        int chipValue = GetChipValueByTag();
        // Call a method in your GameController or Player script to apply damage or handle the attack
        gameController.AttackPlayer(chipValue);

        Destroy(gameObject);
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

    public int GetChipValueByTag()
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

