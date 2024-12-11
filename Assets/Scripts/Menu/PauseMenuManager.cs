
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public bool activePauseMenuUI = true;

    [Header("UI Pages")]
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject albumMenu;
    public GameObject settingMenu;
    public GameObject warningOfExit;

    [Header("Pause Menu Buttons")]
    public Button resumeButton;
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

    [Tooltip("The element 0 should be the click sound.")]
    public List<AudioSource> soundEffectsSource;
    public AudioSource musicSource;
    public List<Light> environmentLights;
    private float[] lightIntensityOriginal;

    public static bool pause = false;

    [Header("Warning Exit Dialog")]
    public Button confirmButton;
    public Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {
        // Hook up pause menu buttons
        resumeButton.onClick.AddListener(() => { PlayClickSound(); ResumeGame(); });
        albumButton.onClick.AddListener(() => { PlayClickSound(); ShowAlbumMenu(); });
        settingButton.onClick.AddListener(() => { PlayClickSound(); ShowSettingMenu(); });
        exitButton.onClick.AddListener(() => { PlayClickSound(); ShowWarningDialog(); });

        // Hook up album menu buttons
        albumBackButton.onClick.AddListener(() => { PlayClickSound(); BackToPauseMenu(); });

        // Hook up setting menu buttons
        settingBackButton.onClick.AddListener(() => { PlayClickSound(); BackToPauseMenu(); });

        lightIntensityOriginal = new float[environmentLights.Count];
        for(int i = 0; i < environmentLights.Count; i++)
        {
            lightIntensityOriginal[i] = environmentLights[i].intensity;
        }

        // Get same value from PlayerPrefs for all scene
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

        confirmButton.onClick.AddListener(() => { PlayClickSound(); ExitGame(); });
        cancelButton.onClick.AddListener(() => { PlayClickSound(); BackToPauseMenu(); });

        HideAll();
        DisplayPauseMenuUI();
    }

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            DisplayPauseMenuUI();
    }

    public void BackToPauseMenu()
    {
        HideAll();
        pauseMenu.SetActive(true);
    }

    public void DisplayPauseMenuUI()
    {
        if (activePauseMenuUI)
        {
            pause = false;
            HideAll();
            pauseMenu.SetActive(false);
            activePauseMenuUI = false;
            Time.timeScale = 1.0f;

        }

        else if (!activePauseMenuUI)
        {
            // Position the pause menu in front of the user
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            gameMenu.transform.position = cameraPosition + cameraForward * 2.0f; // Adjust the distance as needed
            gameMenu.transform.rotation = Quaternion.LookRotation(cameraForward);

            pause = true;
            HideAll();
            pauseMenu.SetActive(true);
            activePauseMenuUI = true;
            Time.timeScale = 0;
        }
    }

    private void PlayClickSound()
    {
        if (soundEffectsSource != null)
        {
            soundEffectsSource[0].PlayOneShot(soundEffectsSource[0].clip);
        }
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

    public void ShowWarningDialog()
    {
        HideAll();
        warningOfExit.SetActive(true);
    }

    public void HideAll()
    {
        pauseMenu.SetActive(false);
        albumMenu.SetActive(false);
        settingMenu.SetActive(false);
        warningOfExit.SetActive(false);
    }


    private void OnSoundEffectsSliderChanged(float value)
    {
        foreach(AudioSource sound in soundEffectsSource)
        {
            sound.volume = value;
        }
        PlayerPrefs.SetFloat("SoundEffectsVolume", value);
    }

    private void OnMusicSliderChanged(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnBrightnessSliderChanged(float value)
    {
        for (int i = 0; i < environmentLights.Count; i++)
        {
            environmentLights[i].intensity = value * lightIntensityOriginal[i];
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
    public void ResumeGame()
    {
        activePauseMenuUI = true;
        DisplayPauseMenuUI();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
