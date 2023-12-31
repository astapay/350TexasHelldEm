using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/*
* ******************************************************************
File name: ButtonScript
author: David Henvick
Creation date: 10/21/23
summary: this is the script that is used control button inputs and events
*/
public class ButtonScript : MonoBehaviour
{
    [SerializeField] private string game = "SampleScene";
    [SerializeField] private string settings = "Settings";
    [SerializeField] private string credits = "Credits";
    [SerializeField] private string tutorial = "TutorialMenu";
    [SerializeField] private string menu = "MainMenu";
    [SerializeField] private string poker = "PokerTutorial";
    [SerializeField] private string bulletHell = "BulletHellTutorial";

    private GameController gameController;

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // <summary>
    // Loads the game scene into the game
    // </summary>
    public void PlayButton()
    {
        SceneManager.LoadScene(game);
    }

    // <summary>
    // Quits the game and exits the application
    // </summary>
    public void QuitButton()
    {
        Application.Quit();
    }

    // <summary>
    // Loads the settings menu
    // </summary>
    public void SettingsButton()
    {
        SceneManager.LoadScene(settings);
    }

    // <summary>
    // Loads the credits menu
    // </summary>
    public void CreditsButton()
    {
        SceneManager.LoadScene(credits);
    }

    // <summary>
    // Loads the tutorial for the game
    // </summary>
    public void TutorialButton()
    {
        SceneManager.LoadScene(tutorial);
    }

    // <summary>
    // Loads the main menu
    // </summary>
    public void MenuButton()
    {
        SceneManager.LoadScene(menu);
    }

    // <summary>
    // Loads the poker tutorial
    // </summary>
    public void PokerButton()
    {
        SceneManager.LoadScene(poker);
    }

    // <summary>
    // Loads the bullethell tutorial
    // </summary>
    public void BulletHellButton()
    {
        SceneManager.LoadScene(bulletHell);
    }


    // <summary>
    // Quits the game and exits the application
    // </summary>
    public void ContinueButton()
    {
        gameController.TogglePaused();
    }
}
