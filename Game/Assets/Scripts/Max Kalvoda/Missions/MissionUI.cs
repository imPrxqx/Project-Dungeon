using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    public Text[] title;
    public Text[] description;
    public Text[] progress;

    private Mission[] activeMissions;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMissions();
    }

    private void OnEnable()
    {
        UpdateMissions();
    }

    //update all missions UI
    public void UpdateMissions()
    {
        activeMissions = GameData.activeMissions.ToArray();

        for (int i = 0; i < title.Length; i++)
        {
            if (i >= activeMissions.Length)
            {
                title[i].text = "Not Avalible";
                description[i].text = "";
                progress[i].text = "";
            }
            else
            {
                Mission mission = activeMissions[i];
                title[i].text = mission.title;
                string descriptionTxt = mission.info + "\n" + mission.price + "G";
                description[i].text = descriptionTxt;

                string progressTxt;
                if (mission.amount > 0)
                {
                    progressTxt = mission.amount + " " + mission.enemy + " remaining";
                }
                else
                {
                    progressTxt = "complete!";
                }
                progress[i].text = progressTxt;
            }
        }
    }
}
