using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject environmentMenu;
    public GameObject settingMenu;

    [Header("Main Menu Buttons")]
    public Button selectEnvironmentButton;
    public Button settingButton;
    public Button exitButton;

    [Header("Environment Menu Buttons")]
    public Button homeButton;
    public Button schoolButton;
    public Button environmentBackButton;

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
        selectEnvironmentButton.onClick.AddListener(() => { PlayClickSound(); ShowEnvironmentMenu(); });
        settingButton.onClick.AddListener(() => { PlayClickSound(); ShowSettingMenu(); });
        exitButton.onClick.AddListener(() => { PlayClickSound(); QuitGame(); });

        // Hook up environment menu buttons
        homeButton.onClick.AddListener(() => { PlayClickSound(); SelectEnvironment(1); });
        schoolButton.onClick.AddListener(() => { PlayClickSound(); SelectEnvironment(2); });
        environmentBackButton.onClick.AddListener(() => { PlayClickSound(); EnableMainMenu(); });

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

    public void SelectEnvironment(int sceneIndex)
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(sceneIndex);
    }

    public void ShowEnvironmentMenu()
    {
        HideAll();
        environmentMenu.SetActive(true);
    }

    public void ShowSettingMenu()
    {
        HideAll();
        settingMenu.SetActive(true);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        environmentMenu.SetActive(false);
        settingMenu.SetActive(false);
    }

    public void EnableMainMenu()
    {
        HideAll();
        mainMenu.SetActive(true);
    }
}