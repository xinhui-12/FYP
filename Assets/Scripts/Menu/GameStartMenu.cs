using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public AudioSource soundEffectsSource;
    public AudioSource musicSource;
    public List<Light> environmentLights;

    // Start is called before the first frame update
    void Start()
    {
        // Hook up main menu buttons
        startButton.onClick.AddListener(() => { PlayClickSound(); StartGame(); });
        albumButton.onClick.AddListener(() => { PlayClickSound(); ShowAlbumMenu(); });
        settingButton.onClick.AddListener(() => { PlayClickSound(); ShowSettingMenu(); });
        exitButton.onClick.AddListener(() => { PlayClickSound(); QuitGame(); });

        // Hook up album menu buttons
        albumBackButton.onClick.AddListener(() => { PlayClickSound(); EnableMainMenu(); });

        // Hook up setting menu buttons
        settingBackButton.onClick.AddListener(() => { PlayClickSound(); EnableMainMenu(); });

        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);

        // To syncronised the setting value
        OnSoundEffectsSliderChanged(soundEffectsSlider.value);
        OnMusicSliderChanged(musicSlider.value);
        OnBrightnessSliderChanged(brightnessSlider.value);

        // To add click sound on the slider
        AddPointerUpEvent(soundEffectsSlider);
        AddPointerUpEvent(musicSlider);
        AddPointerUpEvent(brightnessSlider);

        soundEffectsSlider.onValueChanged.AddListener(OnSoundEffectsSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderChanged);

        EnableMainMenu();
    }

    private void PlayClickSound()
    {
        if (soundEffectsSource != null)
        {
            soundEffectsSource.PlayOneShot(soundEffectsSource.clip);
        }
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

    private void OnSoundEffectsSliderChanged(float value)
    {
        soundEffectsSource.volume = value;
        PlayerPrefs.SetFloat("SoundEffectsVolume", value);
    }

    private void OnMusicSliderChanged(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnBrightnessSliderChanged(float value)
    {
        foreach (Light light in environmentLights)
        {
            light.intensity = value;
        }
        PlayerPrefs.SetFloat("Brightness", value);
    }

    private void AddPointerUpEvent(Slider slider)
    {
        // Add EventTrigger component if it doesn't exist
        EventTrigger trigger = slider.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slider.gameObject.AddComponent<EventTrigger>();
        }

        // Create and add the PointerUp event
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((data) => { PlayClickSound(); });
        trigger.triggers.Add(pointerUpEntry);
    }
}