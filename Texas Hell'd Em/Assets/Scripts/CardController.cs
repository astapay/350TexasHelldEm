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

    public Card(int r, int s)
    {
        rank = r;
        suit = s;
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
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite[] cardSprites;
    [SerializeField] private GameObject cardPF;
    public Card cardStruct;
    private GameObject cardObject;
    private void Start()
    {
        
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

    public void createCard(Queue<Card> deck) {
        float angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        Vector2 cardPos = new Vector2(Mathf.Sin(angle) * 10, Mathf.Cos(angle) * 10); // random position
        cardObject = (GameObject)Instantiate(cardPF, cardPos, new Quaternion(0, 0, 0, 0)); //create card
        cardStruct = deck.Dequeue();
        cardObject.GetComponent<SpriteRenderer>().sprite = cardSprites[cardStruct.getSuit() * 13 + cardStruct.getRank()];
        cardObject.GetComponent<Rigidbody2D>().velocity = new Vector2(cardPos.x / -10, cardPos.y / -10);
    }
}
