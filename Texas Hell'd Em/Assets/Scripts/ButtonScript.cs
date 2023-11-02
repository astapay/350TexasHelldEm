using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
/*
* ******************************************************************
File name: PlayButtonScript
author: David Henvick
Creation date: 10/21/23
summary: this is the script that is used control the overall game and player
*/
public class PlayButtonScript : MonoBehaviour
{
    [SerializeField] private string game = "BulletHellScene";
    [SerializeField] private string settings = "Settings";
    [SerializeField] private string credits = "Credits";
    [SerializeField] private string tutorial = "Tutorial";
    [SerializeField] private string menu = "MainMenu";

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
        UnityEditor.EditorApplication.isPlaying = false;
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
}
