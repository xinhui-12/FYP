using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Sliders")]
    public Slider soundEffectsSlider;
    public Slider musicSlider;
    public Slider brightnessSlider;

    private AudioSource soundEffectsSource;
    private AudioSource musicSource;
    private Light directionalLight;

    [Header("Audio Clips")]
    public AudioClip clickSound;

    private void Start()
    {
        // Assume you have assigned these AudioSources in the Inspector or find them in the scene
        soundEffectsSource = GameObject.Find("SoundEffects").GetComponent<AudioSource>();
        musicSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();

        // Get all lights in the scene to adjust brightness
        directionalLight = GameObject.FindObjectOfType<Light>();
        if (directionalLight == null || directionalLight.type != LightType.Directional)
        {
            Debug.LogError("Directional Light not found or incorrect type!");
        }

        // Add listeners for slider value changes
        soundEffectsSlider.onValueChanged.AddListener(OnSoundEffectsSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        brightnessSlider.onValueChanged.AddListener(OnBrightnessSliderChanged);

        // Optionally, initialize sliders with saved values
        InitializeSettings();

        // Initialize volume settings
        if (musicSource != null) musicSource.volume = musicSlider.value;
        if (soundEffectsSource != null) soundEffectsSource.volume = soundEffectsSlider.value;
        if (directionalLight != null) directionalLight.intensity = brightnessSlider.value;
}

    private void InitializeSettings()
    {
        // Load saved values or set to default if not available
        soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume", 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);
    }

    private void OnSoundEffectsSliderChanged(float value)
    {
        if (soundEffectsSource != null)
        {
            soundEffectsSource.volume = value;
        }
        PlayerPrefs.SetFloat("SoundEffectsVolume", value);
    }

    private void OnMusicSliderChanged(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
        }
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnBrightnessSliderChanged(float value)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = value;
        }
        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void PlayClickSound()
    {
        if (soundEffectsSource != null && clickSound != null)
        {
            soundEffectsSource.PlayOneShot(clickSound);
        }
    }

}
