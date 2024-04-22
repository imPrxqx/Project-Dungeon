using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGame : MonoBehaviour
{
    static int sceneCount;

    public static List<string> GetAllScenes()
    {
        List<string> list = new List<string>();

        //Get all scenes from build 
        sceneCount = SceneManager.sceneCountInBuildSettings;

        //Add full path of scenes to list
        for (int i = 0; i < sceneCount; i++)
        {
            list.Add(SceneUtility.GetScenePathByBuildIndex(i));
        }

        return list;
    }

    public static List<string> GetAllOnlyFloors()
    {
        List<string> list = new List<string>();

        string patternFloors = "(Floor [0-9]/Floor [0-9]_[0-9])";

        //Get only scenes with string "Floor []/Floor []_[]"
        foreach (string floors in GetAllScenes())
        {
            Match regexMatch = Regex.Match(floors, patternFloors, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));

            if (regexMatch.Success)
            {
                
                list.Add(floors);
                       
            }
        }

        return list;
    }

    public static int GetNumberOfFloors()
    {
        List<string> list = new List<string>();

        string uniqueFloors = "(Floor [0-9])";

        foreach (string floors in GetAllOnlyFloors())
        {
            Match regexMatch = Regex.Match(floors, uniqueFloors, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));

            if (regexMatch.Success)
            {
                if (!list.Contains(regexMatch.Groups[1].Value))
                {
                    list.Add(regexMatch.Groups[1].Value);
                }              
            }
        }

        return list.Count();
    }

    public static int GetNumberOfLevelsOnFloor()
    {

        List<string> list = new List<string>();

        string uniqueFloors = String.Format("(Floor {0}/Floor [0-9])", GameData.floor);

        foreach (string floors in GetAllOnlyFloors())
        {
            Match regexMatch = Regex.Match(floors, uniqueFloors, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));

            if (regexMatch.Success)
            {
                if (!list.Contains(regexMatch.Groups[0].Value))
                {
                    list.Add(regexMatch.Groups[0].Value);
                }
            }
        }

        return list.Count();
    }

    public static int GetNumberOfVersionsOnLevel()
    {
        List<string> list = new List<string>();

        string uniqueFloors = String.Format("(Floor {0}/Floor {1}_[0-9])", GameData.floor, GameData.levelfloor);

        foreach (string floors in GetAllOnlyFloors())
        {
            Match regexMatch = Regex.Match(floors, uniqueFloors, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(10));

            if (regexMatch.Success)
            {
                if (!list.Contains(regexMatch.Groups[0].Value))
                {
                    list.Add(regexMatch.Groups[0].Value);
                }
            }
        }

        return list.Count();
    }

    public static string NextLevel()
    {

        GameData.LoadGameData();

        GameData.levelfloor += 1;

        if(GameData.levelfloor >= GetNumberOfLevelsOnFloor())
        {
            GameData.floor += 1;
            GameData.levelfloor = 0;
        }

        GameData.version = GlobalFunctions.RandomValue(GetNumberOfVersionsOnLevel());

        if (GameData.floor >= GetNumberOfFloors())
        {
            GameData.floor = 0;
            GameData.levelfloor = 0;
            GameData.version = 0;
        }

        GameData.SaveGameData();

        return String.Format("Floor {0}/Floor {1}_{2}", GameData.floor, GameData.levelfloor, GameData.version);
    }

}
