using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.IO;

//It can adjust the graphics, sounds and controls of the game. It can also load and save all settings to a Json file
public class OptionsScript : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown, QualityDropdown;
    public Slider MasterVolSlider, SfxVolSlider, MusicVolSlider;
    public AudioMixer mixer;
    public GameObject keySettings;
    public Text[] keyTexts = new Text[18];

    //0 - forward, 1 - backward, 2 - left, 3 - right, 4 - jump, 5 - crouch, 6 - sprint, 
    //7 - Pattack, 8 - Sattack, 9 - interact, 10 - use, 11 - pause
    //12 - inventory, 13 - dash, 14 - item1, 15 - item2, 16 - item3, 17 - item4
    public static KeyCode[] keys = new KeyCode[18];
    static bool fullscreen;
    static int resolution, quality;
    static float MasterVol, SfxVol, MusicVol;

    Resolution[] resolutions;
    bool waitingKey, clicked, lastClicked;
    Event keyEvent;
    KeyCode newKeyCode;

    void Start()
    {
        Load();

        QualitySettings.SetQualityLevel(quality);
        QualityDropdown.value = quality;
        Screen.fullScreen = fullscreen;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        if (resolution == -1)
            resolution = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.value = resolution;

        MasterVolSlider.value = MasterVol;
        MusicVolSlider.value = MusicVol;
        SfxVolSlider.value = SfxVol;

        SetMasterVolume(MasterVol);
        SetMusicVolume(MusicVol);
        SetSfxVolume(SfxVol);

        for (int i = 0; i < keyTexts.Length; i++)
        {
            keyTexts[i].text = keys[i].ToString();
        }
        waitingKey = false;
    }
    //Set the resolution
    public void SetResolution(int resolutionIndex)
    {
        resolution = resolutionIndex;
        Resolution newNesolution = resolutions[resolutionIndex];
        Screen.SetResolution(newNesolution.width, newNesolution.height, Screen.fullScreen);
        AudioManager.instance.Play("click");
    }
    //Set the quality of the game
    public void SetQuality(int qualityIndex)
    {
        quality = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
        AudioManager.instance.Play("click");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
        AudioManager.instance.Play("click");
    }
    //Open the key-settings panel
    public void KeySettings()        
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keyTexts[i].text = keys[i].ToString().ToUpper();
        }
        keySettings.SetActive(true);
        AudioManager.instance.Play("click");
    }
    //sets keyEvent for key binding
    private void OnGUI()
    {
        keyEvent = Event.current;

        if (waitingKey && !clicked)
        {
            if (lastClicked)
            {
                lastClicked = clicked;
                return;
            }

            if (keyEvent.isKey || keyEvent.isMouse)
            {
                if (keyEvent.isMouse)
                {
                    newKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Mouse" + keyEvent.button); ;
                }
                else
                {
                    newKeyCode = keyEvent.keyCode;
                }

                clicked = true;
                waitingKey = false;
            }
        }

    }

    private void Update()
    {
        if (clicked)
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                lastClicked = true;
                clicked = false;
            }
        }
    }
    //Set the key to a specific action (jump, attack, etc.)
    public void SetKey(int KeyNum)
    {
        if (clicked)
            return;

        clicked = true;
        if (!waitingKey)
            StartCoroutine(GetKey(KeyNum));
    }

    public IEnumerator GetKey(int KeyNum)
    {
        waitingKey = true;

        while (waitingKey)
        {
            yield return null;
        }

        foreach (KeyCode k in keys)
        {
            if (k == newKeyCode)
            {
                AudioManager.instance.Play("click");
                yield break;
            }
        }

        keys[KeyNum] = newKeyCode;
        keyTexts[KeyNum].text = newKeyCode.ToString().ToUpper();
        AudioManager.instance.Play("click");
    }

    //Closes the key-settings panel
    public void Back()
    {
        Save();
        keySettings.SetActive(false);
        AudioManager.instance.Play("click");
    }
    //Saves the current settings to a file
    public static void Save()
    {
        StreamWriter writer = new StreamWriter(File.Open("MySettings.txt", FileMode.OpenOrCreate));

        for (int i = 0; i < keys.Length; i++)
        {
            writer.WriteLine(keys[i]);
        }

        writer.WriteLine(resolution);
        writer.WriteLine(quality);
        writer.WriteLine(fullscreen);
        writer.WriteLine(MasterVol);
        writer.WriteLine(SfxVol);
        writer.WriteLine(MusicVol);

        writer.Close();
    }
    //Loads the current settings from a file
    public static void Load()
    {
        if (File.Exists("MySettings.txt"))
        {
            StreamReader reader = new StreamReader(File.Open("MySettings.txt", FileMode.Open));

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = (KeyCode) Enum.Parse(typeof(KeyCode), reader.ReadLine()); 
            }

            resolution = int.Parse(reader.ReadLine());
            quality = int.Parse(reader.ReadLine()); ;
            fullscreen = bool.Parse(reader.ReadLine());
            MasterVol = float.Parse(reader.ReadLine());
            SfxVol = float.Parse(reader.ReadLine());
            MusicVol = float.Parse(reader.ReadLine());

            reader.Close();
        }
        else
        {

            keys = new KeyCode[] {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space, KeyCode.C, KeyCode.LeftShift, 
                                  KeyCode.Mouse0, KeyCode.Mouse1, KeyCode.F, KeyCode.E, KeyCode.Escape,
                                  KeyCode.I, KeyCode.U,KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4};

            resolution = -1;
            quality = 3;
            fullscreen = true;
            MasterVol = 1;
            SfxVol = 1;
            MusicVol = 1;

            Save();
        }
    }
    //Set the master, stf  and music volume
    public void SetMasterVolume(float volume)
    {
        MasterVol = volume;
        mixer.SetFloat("MasterVolume", Mathf.Log10(MasterVol) * 20);
    }
    public void SetSfxVolume(float volume)
    {
        SfxVol = volume;
        mixer.SetFloat("SFXVolume", Mathf.Log10(SfxVol) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        MusicVol = volume;
        mixer.SetFloat("MusicVolume", Mathf.Log10(MusicVol) * 20);
    }

}
