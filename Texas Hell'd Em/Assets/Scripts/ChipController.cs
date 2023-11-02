using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipController : MonoBehaviour
{
    //spawn rate: y=6ln(x)
    //x = # spawns
    //y = secs between spawns

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            //add points corresponding to chip
            //destroy chip
        }
        else if (collision.gameObject.tag == "Collector")
        {
            //subtract points corresponding to chip
            //GameController.updateScoreText();
            //destroy chip
        }
    }
}
