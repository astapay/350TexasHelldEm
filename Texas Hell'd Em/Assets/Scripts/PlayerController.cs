using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CardController CardController;
    Card[] hand = new Card[2];
    int selectedCard;
    int handLevel;

    public void addToHand(Card card) { 
        for(int i = 0; i < hand.Length; i++) {
            if (hand[i].Equals(default(Card)))
            {
                hand[i] = card;
            }
        }
        hand[selectedCard] = card;
    }
}
