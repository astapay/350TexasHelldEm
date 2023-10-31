using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.VFX;

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
    private bool game = true;
    public Card[] deck;
    [SerializeField] private GameObject cardPF;
    public void endGame() { 
        game = false;
    }

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
        StartCoroutine(CreateACard());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shield") {
            //destroy card
        }
        else if(collision.gameObject.tag == "Collector"){ 
            //add card to hand
            //destroy
        }
    }

    IEnumerator CreateACard()
    {
        int spawns = 0;
        while(game)
        {
            float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            Vector2 cardPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10); // random position

            GameObject card = (GameObject)Instantiate(cardPF, cardPos, new Quaternion(0,0,angle,0) ); //create ball
            card.GetComponent<Rigidbody2D>().velocity = new Vector2(cardPos.x / -10, cardPos.y / -10);
            spawns++;

            yield return new WaitForSeconds(MathF.Exp((spawns - 1)/20));
        }
    }
}
