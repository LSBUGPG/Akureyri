using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public static float g_Volume;
    public static int g_Quality;
    public static int g_Resoultion;
    public static bool g_Fullscreen;
    public GameObject PauseUI;
    public GameObject SettingsUI;
    public GameObject InGameUI;
    public GameObject WinUI;
    public GameObject WinStoryUI;
    public GameObject CreditsUI;
    public GameObject HelpUI;
    public string MainMenu;
    public string MainGame;
    public AudioMixer AM_Music;
    public AudioMixer AM_SFX;
    public AudioMixer AM_Ambience;
    public AudioMixer AM_Main;
    public TMP_Dropdown ResolutionDropdown;

    public bool IsMainMenu = false;

    //private bool IsFullscreen = true;
    private Resolution[] Resolutions;

    private void Start()
    {
        Resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        IsPaused = false;
        int CurrentResolutionIndex = 0;

        for ( int i = 1; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height;
            options.Add(option);

            if(Resolutions[i].width == Screen.currentResolution.width && Resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.SetValueWithoutNotify(CurrentResolutionIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMainMenu)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PauseUI.SetActive(false);
        CreditsUI.SetActive(false);
        HelpUI.SetActive(false);
        WinUI.SetActive(false);
        WinUI.SetActive(false);
        WinStoryUI.SetActive(false);
        SettingsUI.SetActive(false);

        InGameUI.SetActive(true);

        IsPaused = false;
    }

    public void Credits()
    {
        PauseUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    public void CreditsBack()
    {

        PauseUI.SetActive(true);
        CreditsUI.SetActive(false);
    }

    public void Help()
    {
        PauseUI.SetActive(false);
        HelpUI.SetActive(true);
    }

    public void HelpBack()
    {
        PauseUI.SetActive(true);
        HelpUI.SetActive(false);
    }

    public void WinStory()
    {        
        if (IsMainMenu)
        {
            PauseUI.SetActive(false);
            WinStoryUI.SetActive(true);
        }
        else
        {
            WinUI.SetActive(false);
            WinStoryUI.SetActive(true);
        }
    }

    public void WinStoryBack()
    {
        if (IsMainMenu)
        {
            PauseUI.SetActive(true);
            WinStoryUI.SetActive(false);
        }
        else
        {
            WinUI.SetActive(true);
            WinStoryUI.SetActive(false);
        }
    }

    public void WinPause()
    {
        PauseUI.SetActive(false);
        InGameUI.SetActive(false);
        WinUI.SetActive(true);

        Time.timeScale = 0;
        IsPaused = true;
    }

    public void LoadMenu()
    {
        IsPaused = false;
        SceneManager.LoadScene(MainMenu);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(MainGame);
    }

    public void Restart()
    {
        IsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSettings()
    {
        PauseUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void UnloadSettings()
    {
        PauseUI.SetActive(true);
        SettingsUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetVolumeMain(float Volume)
    {
        SetVolume(AM_Main, "MainVol", Volume);
        SetVolume(AM_Ambience, "AmbienceVol", PlayerPrefs.GetFloat("AmbienceVol") * Volume);
        SetVolume(AM_Music, "MusicVol", PlayerPrefs.GetFloat("MusicVol") * Volume);
        SetVolume(AM_SFX, "SFXVol", PlayerPrefs.GetFloat("SFXVol") * Volume);
    }

    public void SetVolumeAmbience(float Volume)
    {
        SetVolume(AM_Ambience, "AmbienceVol", Volume);
    }

    public void SetVolumeMusic(float Volume)
    {
        SetVolume(AM_Music, "MusicVol", Volume);
    }

    public void SetVolumeSFX(float Volume)
    {
        SetVolume(AM_SFX, "SFXVol", Volume);
    }

    public void SetVolume(AudioMixer AM, string name, float Volume)
    {
        g_Volume = Volume;
        if (Volume == 0)
        {
            AM.SetFloat(name, Mathf.Log10(0.00001f) * 20);

        }
        else
        {
            AM.SetFloat(name, Mathf.Log10(Volume) * 20);
        }
    }

    public void SetQuality(int QualityIndex)
    {
        g_Quality = QualityIndex;
        QualitySettings.SetQualityLevel(QualityIndex);
    }

    public void SetFullscreen(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;
    }

    public void SetResolution(int ResolutionIndex)
    {
        g_Resoultion = ResolutionIndex;
        Resolution res = Resolutions[ResolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        PauseUI.SetActive(true);
        InGameUI.SetActive(false);

        Time.timeScale = 0;
        IsPaused = true;
    }
}
