/******************************************************************************
// File Name:       ChipController.cs
// Author:          Alex Kalscheur
// Creation date:   10/21/2023
// Summary:         Creates a poker chip in the scene. This script is currently
                    unused.
******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipController : MonoBehaviour
{
    //spawn rate: y=6ln(x)
    //x = # spawns
    //y = secs between spawns

    // <summary>
    // Called when a collision is detected
    // Used for the logic between a chip and the player
    // </summary>
    // <param name="collision">
    // Information as to what triggered the collision
    // </param>
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
