using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public struct Card
{
    private int rank;  //0-8 represent ranks 2-10, 9 is Jack, 10 is Queen, 11 is King, and 12 is Ace
    private int suit; //0 is Club, 1 is Diamond, 2 is Heart, 3 is Spade
    private bool inUse;

    public Card(int r, int s)
    {
        rank = r;
        suit = s;
        inUse = false;
    }

    public int getRank()
    { 
        return rank; 
    }

    public int getSuit()
    {
        return suit;
    }
}

public class CardController : MonoBehaviour
{
    public Card[] deck;

    private void Start()
    {
        Card testCard;
        int deckIndex = 0;
        for(int r = 0; r < 13; r++)
        {
            for(int s = 0; s < 4; s++)
            {
                testCard = new Card(r, s);
                //deck[deckIndex] = new Card(r, s);
                deckIndex++;
            }
        }
    }
}
