
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool activePauseMenuUI = true;

    [Header("UI Pages")]
    public GameObject gameMenu;
    public GameObject pauseMenuUI;
    public GameObject albumMenu;
    public GameObject settingMenu;

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

    public AudioSource soundEffectsSource;
    public AudioSource musicSource;
    public List<Light> environmentLights;

    public static bool pause = false;


    // Start is called before the first frame update
    void Start()
    {

        // Hook up pause menu buttons
        albumButton.onClick.AddListener(() => { PlayClickSound(); ShowAlbumMenu(); });
        settingButton.onClick.AddListener(() => { PlayClickSound(); ShowSettingMenu(); });
        exitButton.onClick.AddListener(() => { PlayClickSound(); ExitGame(); });

        // Hook up album menu buttons
        albumBackButton.onClick.AddListener(() => { PlayClickSound(); BackToPauseMenu(); });

        // Hook up setting menu buttons
        settingBackButton.onClick.AddListener(() => { PlayClickSound(); BackToPauseMenu(); });

        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);

        soundEffectsSlider.onValueChanged.AddListener(OnSoundEffectsSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderChanged);

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
        pauseMenuUI.SetActive(true);
    }

    public void DisplayPauseMenuUI()
    {
        if (activePauseMenuUI)
        {
            pause = false;
            HideAll();
            pauseMenuUI.SetActive(false);
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
            pauseMenuUI.SetActive(true);
            activePauseMenuUI = true;
            Time.timeScale = 0;
        }
    }

    private void PlayClickSound()
    {
        if (soundEffectsSource != null)
        {
            soundEffectsSource.PlayOneShot(soundEffectsSource.clip);
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

    public void HideAll()
    {
        pauseMenuUI.SetActive(false);
        albumMenu.SetActive(false);
        settingMenu.SetActive(false);
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
