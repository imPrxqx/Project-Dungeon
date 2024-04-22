using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//It is used to control the pause menu and always sets the scene at the beginning of the level 
public class GameManagerScript : MonoBehaviour
{
    public GameObject mainCanvas, optionsCanvas;
    private bool lastInMenu = false;
    public GameObject[] weapons;
    
    public void Start()
    {
 
        //Open data from start
        OptionsScript.Load();
        GameData.LoadGameData();

        //Get a compoments
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement PM = player.GetComponent<PlayerMovement>();
        PM.UpdateUnlocks();

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

        //Get a weapon
        GameObject weapon = Instantiate(weapons[GameData.equipedWeaponNum], camera.transform);
        WeaponScript WS = weapon.GetComponent<WeaponScript>();
        WS.UpdateUnlocks();

        //Get items from database 
        DatabaseItems.addItems();

        StartCoroutine(CloseOptionsDelayed());
    }

    private void Update()
    {
        //If clicked on keyboard then open pause menu
        if (Input.GetKeyDown(OptionsScript.keys[11]))
        {
            if (mainCanvas.activeSelf)
            {
                Resume();
            }
            else if (optionsCanvas.activeSelf)
            {
                Back();
            }
            else if (!lastInMenu)
            {
                GameData.inMenu = true;
                lastInMenu = true;
                mainCanvas.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        lastInMenu = GameData.inMenu;
    }
    //Resume a game that was paused
    public void Resume()
    {
        AudioManager.instance.Play("click");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCanvas.SetActive(false);
        GameData.inMenu = false;
        lastInMenu = false;
    }
    //Open options menu
    public void Options()
    {
        AudioManager.instance.Play("click");
        mainCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }
    //Go back from options menu
    public void Back()
    {
        AudioManager.instance.Play("click");
        OptionsScript.Save();
        optionsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }
    //Quit the game 
    public void Quit()
    {
        AudioManager.instance.Play("click");
        GameData.inMenu = false;
        SceneManager.LoadScene(0);
    }

    IEnumerator CloseOptionsDelayed()
    {
        yield return new WaitForFixedUpdate();
        optionsCanvas.SetActive(false);
    }
}
