using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject albumMenu;
    public GameObject settingMenu;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button albumButton;
    public Button settingButton;
    public Button exitButton;

    [Header("Album Menu Buttons")]
    public Button albumBackButton;

    [Header("Setting Menu")]
    public Slider soundEffectsSlider;
    public Slider musicSlider;
    public Slider brightnessSlider;
    public Button settingBackButton; 
    
    private SettingsManager settingsManager;

    // Start is called before the first frame update
    void Start()
    {
       settingsManager = FindObjectOfType<SettingsManager>();

        EnableMainMenu();

        // Hook up main menu buttons
        startButton.onClick.AddListener(() => { PlayClickSound(); StartGame(); });
        albumButton.onClick.AddListener(() => { PlayClickSound(); ShowAlbumMenu(); });
        settingButton.onClick.AddListener(() => { PlayClickSound(); ShowSettingMenu(); });
        exitButton.onClick.AddListener(() => { PlayClickSound(); QuitGame(); });

        // Hook up album menu buttons
        albumBackButton.onClick.AddListener(() => { PlayClickSound(); EnableMainMenu(); });

        // Hook up setting menu buttons
        settingBackButton.onClick.AddListener(() => { PlayClickSound(); EnableMainMenu(); });

        // Initialize the SettingsManager with the sliders
        if (settingsManager != null)
        {
            settingsManager.soundEffectsSlider = soundEffectsSlider;
            settingsManager.musicSlider = musicSlider;
            settingsManager.brightnessSlider = brightnessSlider;
        }

    }

    private void PlayClickSound()
    {
        settingsManager?.PlayClickSound();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    public void ShowAlbumMenu()
    {
        HideAll();
        albumMenu.SetActive(true);
    }

    public void ShowSettingMenu()
    {
        HideAll();
        settingMenu.SetActive(true);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        albumMenu.SetActive(false);
        settingMenu.SetActive(false);
    }

    public void EnableMainMenu()
    {
        HideAll();
        mainMenu.SetActive(true);
    }
}