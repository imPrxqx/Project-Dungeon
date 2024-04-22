using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUnlock : MonoBehaviour
{
    public Text lvlTxt;
    public Text skillPointsTxt;

    public Button[] unlockButtons;

    PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        movement =  player.GetComponent<PlayerMovement>();

        lvlTxt.text = "Level: " + GameData.level;
        skillPointsTxt.text = "Skill points: " + GameData.skillPoints;

        bool[] unlocks = GameData.unlocks;

        if (unlocks[0])
            if (unlocks[1])
                unlockButtons[2].interactable = true;

        if (unlocks[2])
        {
            unlockButtons[3].interactable = true;
            unlockButtons[4].interactable = true;
        }

        for(int i = 0; i < unlockButtons.Length; i++)
        {
            if (unlocks[i])
            {
                unlockButtons[i].interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //try unlock new abylity
    public void Unlock(int skillIndex)
    {
        bool succes = GameData.Unlock(skillIndex);
        if (succes)
        {
            AudioManager.instance.Play("upgrade");
        }
        else
        {
            AudioManager.instance.Play("error");
        }

        movement.UpdateUnlocks();

        lvlTxt.text = "Level: " + GameData.level;
        skillPointsTxt.text = "Skill points: " + GameData.skillPoints;

        bool[] unlocks = GameData.unlocks;

        if (unlocks[0])
            if (unlocks[1])
                unlockButtons[2].interactable = true;

        if (unlocks[2])
        {
            unlockButtons[3].interactable = true;
            unlockButtons[4].interactable = true;
        }

        for (int i = 0; i < unlockButtons.Length; i++)
        {
            if (unlocks[i])
            {
                unlockButtons[i].interactable = false;
            }
        }
    }

}
