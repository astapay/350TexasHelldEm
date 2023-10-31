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
    public void PlayButton()
    {
        SceneManager.LoadScene(game);
    }
    public void QuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void SettingsButton()
    {
        SceneManager.LoadScene(settings);
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene(credits);
    }
    public void TutorialButton()
    {
        SceneManager.LoadScene(tutorial);
    }
}
