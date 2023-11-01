using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardData cardStruct;
    private GameController gameController;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            gameController.deck.Enqueue(cardStruct);
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Collector")
        {
            playerController.addToHand(cardStruct);
            Destroy(gameObject);
        }
        
    }

    public void setCardData(CardData cardData) { 
        cardStruct = cardData;
    }
}
