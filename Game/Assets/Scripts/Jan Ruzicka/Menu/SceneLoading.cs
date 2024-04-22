using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public GameObject loader;
    public Slider slider;
    float progress = 100;
 

    // Start is called before the first frame update
    void Start()
    {
        SceneGame.NextLevel();

        Time.timeScale = 1;
        StartCoroutine(LoadAsyncsOperation());
    }

    //If scene is ready then start scene 
    IEnumerator LoadAsyncsOperation()
    {

        AsyncOperation game = SceneManager.LoadSceneAsync(String.Format("Scenes/Floor {0}/Floor {1}_{2}", GameData.floor, GameData.levelfloor, GameData.version));
        

        loader.SetActive(true);
        //Progress value from how much time is neeeded to ready scene
        while(!game.isDone)
        {
            progress = Mathf.Clamp01(game.progress / 0.9f);

            slider.value = progress;

            yield return null;
        }
 
    }
}
