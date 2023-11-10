using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* ******************************************************************
File name: MainUIScript
author: David Henvick
Creation date: 11/8/23
summary: this is the script that is used swap out the ui for the main game when cards are updated
*/
public class MainUIScript : MonoBehaviour
{
    //player cards
    [SerializeField] private GameObject playerCard1;
    [SerializeField] private GameObject playerCard2;
    //river cards
    [SerializeField] private GameObject riverCard1;
    [SerializeField] private GameObject riverCard2;
    [SerializeField] private GameObject riverCard3;
    [SerializeField] private GameObject riverCard4;
    [SerializeField] private GameObject riverCard5;

    public void SwapSprite(int card, Sprite sprite)
    {
        if(card == 0)
        {
            playerCard1.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else if(card == 1)
        {
            playerCard2.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else if (card == 2)
        {
            riverCard1.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else if (card == 3)
        {
            riverCard2.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else if (card == 4)
        {
            riverCard3.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else if (card == 5)
        {
            riverCard4.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else if (card == 6)
        {
            riverCard5.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        //delete before release---------------------------------------------------------------------------------------
        else
        {
            Debug.Log(card);
        }
    }
}
