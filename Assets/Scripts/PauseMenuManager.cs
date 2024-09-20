using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool activePauseMenuUI = true;
    public Slider soundEffectsSlider;
    public Slider musicSlider;
    public Slider brightnessSlider;
    public AudioSource soundEffectsSource;
    public AudioSource musicSource;
    public List<Light> environmentLights;


    // Start is called before the first frame update
    void Start()
    {
        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);

        soundEffectsSlider.onValueChanged.AddListener(OnSoundEffectsSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderChanged);

        DisplayPauseMenuUI();
    }


    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            DisplayPauseMenuUI();

    }
    public void DisplayPauseMenuUI()
    {
        if (activePauseMenuUI)
        {
            pauseMenuUI.SetActive(false);
            activePauseMenuUI = false;
            Time.timeScale = 1.0f;

        }

        else if (!activePauseMenuUI)
        {
            // Position the pause menu in front of the user
            Vector3 cameraPosition = Camera.main.transform.position;
            Vector3 cameraForward = Camera.main.transform.forward;
            pauseMenuUI.transform.position = cameraPosition + cameraForward * 2.0f; // Adjust the distance as needed
            pauseMenuUI.transform.rotation = Quaternion.LookRotation(cameraForward);

            pauseMenuUI.SetActive(true);
            activePauseMenuUI = true;
            Time.timeScale = 0;
        }
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

        pauseMenuUI.SetActive(false);
        activePauseMenuUI = false;
        Time.timeScale = 1.0f;

    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
}
