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

    public void Start()
    {
        logHand();
    }

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

    private void logHand() {
        for (int i = 0; i < hand.Length; i++)
        {
            Debug.Log("Card slot " + i + ": " + hand[i].getRankName() + " of " + hand[i].getSuitName());
        }
    }
}
