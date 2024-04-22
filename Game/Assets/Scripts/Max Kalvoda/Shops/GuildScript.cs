using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class GuildScript : MonoBehaviour
{
    Transform player;
    public GameObject guildMenu;
    public Text info; 
    public Text title; 
    public Text missionButtonTxt;
    public Text[] buttonTitles;
    public int MissionCount = 3;
    public int acceptLimit = 1;
    public Animator anim;


    MissionList missionList = new MissionList();
    int activeMission = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //loading missions from file
        if (File.Exists("missions.json"))
        {
            FileStream file = File.Open("missions.json", FileMode.Open);
            StreamReader fileReader = new StreamReader(file);
            string jsonStr = fileReader.ReadToEnd();
            fileReader.Close();

            MissionList list = JsonUtility.FromJson<MissionList>(jsonStr);
            Mission[] missions = list.missions;

            //randomly choosing tree missions
            missionList.missions = new Mission[MissionCount];
            for (int i = 0; i < MissionCount; i++)
            {
                int num = Random.Range(0, missions.Length);
                Mission mission = missions[num];

                missionList.missions[i] = mission;
                buttonTitles[i].text = mission.title;

                if(i == 0)
                {
                    info.text = mission.info + "\n price: " + mission.price;
                    title.text = mission.title;
                }

                foreach (Mission m in missionList.missions)
                {
                    if (m == null)
                        break;

                    if (m.title == mission.title)
                        continue;

                    if (m.title == mission.title)
                    {
                        i--;
                        break;
                    }
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //chcecking input for opening and guild UI
        if (!GameData.inMenu) {
            if (Input.GetKeyDown(OptionsScript.keys[9])) {
                if ((player.position - transform.position).magnitude < 2)
                {
                    GameData.inMenu = true;
                    guildMenu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
        if (guildMenu.activeSelf) {
            if (Input.GetKeyDown(OptionsScript.keys[11]))
            {
                Exit();
            }
        }
        
    }

    //closes inventory and gives control to the player
    public void Exit()
    {
        AudioManager.instance.Play("click");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        guildMenu.SetActive(false);
        GameData.inMenu = false;
    }

    //accepting the mission
    public void MissionButtonFunc()
    {
        Mission mission = missionList.missions[activeMission];
        if (GameData.activeMissions.Contains(mission))
        {
            GameData.activeMissions.Remove(mission);
            missionButtonTxt.text = "Accept";
            anim.Play("SucAnim");
            AudioManager.instance.Play("click");
        }
        else
        {
            if (GameData.activeMissions.Count < acceptLimit)
            {
                GameData.activeMissions.Add(mission);
                missionButtonTxt.text = "Cancel";
                anim.Play("SucAnim");
                AudioManager.instance.Play("click");
            }
            else
            {
                anim.Play("ErrAnim");
                AudioManager.instance.Play("error");
            }
        }

    }

    //switch to different mission
    public void SwitchMissionInfo(int num)
    {
        AudioManager.instance.Play("click");
        activeMission = num;
        Mission[] missions = missionList.missions;
        Mission mission = missions[activeMission];
        title.text = mission.title;
        info.text = mission.info + "\n price: " + mission.price;
        List<Mission> activeMissions = GameData.activeMissions;
        if (activeMissions.Contains(mission))
        {
            missionButtonTxt.text = "Cancel";
        }
        else
        {
            missionButtonTxt.text = "Accept";
        }
    }

}

