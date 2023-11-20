using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
* ******************************************************************
File name: PokerTutorialScript
author: David Henvick
Creation date: 10/21/23
summary: this is the script in charge of the poker tutorial page;
*/
public class PokerTutorialScript : MonoBehaviour
{
    //game object
    [SerializeField] private GameObject tutorial; 
    //tutorial pages
    [SerializeField] private Sprite one;
    [SerializeField] private Sprite two;
    [SerializeField] private Sprite three;
    [SerializeField] private Sprite four;
    [SerializeField] private Sprite five;
    [SerializeField] private Sprite six;
    [SerializeField] private Sprite seven;
    [SerializeField] private Sprite eight;
    [SerializeField] private Sprite nine;
    [SerializeField] private Sprite ten;
    [SerializeField] private Sprite eleven;
    //buttons
    [SerializeField] private GameObject next;
    [SerializeField] private GameObject prev;

    //int for keeping track of what page we are on
    private int page;
    // Start is called before the first frame update
    private void Start()
    {
        page = 1;
        tutorial.GetComponent<SpriteRenderer>().sprite = one;
        prev.SetActive(false);
        next.SetActive(true);
    }

    // Update is called once per frame

    private void UpdateText(int currPage)
    {
        if (currPage == 1)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = one;
            prev.SetActive(false);
        }
        else if (currPage == 2)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = two;
            prev.SetActive(true);
        }
        else if (currPage == 3)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = three;
        }
        else if (currPage == 4)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = four;
        }
        else if (currPage == 5)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = five;
        }
        else if (currPage == 6)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = six;
        }
        else if (currPage == 7)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = seven;
        }
        else if (currPage == 8)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = eight;
        }
        else if (currPage == 9)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = nine;
        }
        else if (currPage == 10)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = ten;
            next.SetActive(true);
        }
        else if (currPage == 11)
        {
            tutorial.GetComponent<SpriteRenderer>().sprite = eleven;
            next.SetActive(false);
        }
    }

    public void NextButton()
    {
        page++;
        UpdateText(page);
    }

    public void previousButton()
    {
        page--;
        UpdateText(page);
    }

    public void backButton()
    {
        SceneManager.LoadScene("TutorialMenu");
    }
}
