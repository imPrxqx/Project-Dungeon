using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainCanvas, optionsCanvas;

    //Get options and data
    public void Start()
    {
        OptionsScript.Load();
        GameData.LoadGameData();
    }
    //Start the game
    public void Play()
    {
        GameData.LoadGameData();
        SceneManager.LoadScene(String.Format("Scenes/Floor {0}/Floor {1}_{2}", GameData.floor, GameData.levelfloor, GameData.version));
    }
    //Open option panel
    public void Options()
    {
        mainCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    //Go back from option panel
    public void Back()
    {
        OptionsScript.Save();
        optionsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
    //Quit the game   
    public void Quit()
    {
        GameData.SaveGameData();
        Application.Quit();    
    } 
}
