using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

namespace ProjektSumperk
{
    public class VideoPlayers : MonoBehaviour
    {
        public VideoPlayer vp;
        public GameObject videoPlayerPanel;
        public GameObject play;
        public GameObject pause;
        public RenderTexture rt;
        public Image seekbarFill; // Reference to the seekbar fill Image element
        public Slider volumeSlider; // Reference to the volume control Slider
        public Slider speedSlider; // Reference to the playback speed Slider
        public TMP_Text volumeText;
        public TMP_Text speedText;
        public TMP_Text status;
        private bool wasPaused = false;

        private bool hasStarted = false;

        private void OnEnable()
        {
            speedSlider.value = 1.0f;
            volumeSlider.value = 40.0f;
        }

        private void Update()
        {
            // Update the seekbar fill amount based on video progress
            if (vp.frameCount > 0)
            {
                float fillAmount = (float)vp.frame / vp.frameCount;
                seekbarFill.fillAmount = fillAmount;
            }

            // Update video volume based on the slider value
            vp.SetDirectAudioVolume(0, volumeSlider.value);
            volumeText.text = volumeSlider.value.ToString("F0");

            // Update video playback speed based on the slider value
            vp.playbackSpeed = speedSlider.value;
            speedText.text = speedSlider.value.ToString("F0")+"x";

            // Check for video events
            if (!hasStarted && vp.isPlaying)
            {
                // Video has started playing
                hasStarted = true;
                OnVideoStart();
            }
            else if (hasStarted && !vp.isPlaying)
            {
                // Video has been paused
                OnVideoPause();
            }
            else if (hasStarted && vp.isPlaying && !vp.isLooping && vp.time >= vp.clip.length)
            {
                // Video has completed playing
                OnVideoComplete();
            }

            // Check for Spacebar press to toggle play/pause
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (vp.isPlaying)
                {
                    PauseVideo();
                }
                else
                {
                    if (wasPaused)
                    {
                        OnVideoResume();
                        wasPaused = false;
                    }
                    PlayVideo();
                }
            }
        }

        private void OnVideoStart()
        {
            // Implement your logic when the video starts playing
            status.text = "Callback: Video Started!";
            
        }

        private void OnVideoPause()
        {
            // Implement your logic when the video is paused
            status.text = "Callback: Video Paused!";
        }

        private void OnVideoResume()
        {
            // Implement your logic when the video is resumed from pause
            status.text = "Callback: Video Resumed!";
        }

        private void OnVideoComplete()
        {
            // Implement your logic when the video completes playing (non-looping)
            status.text = "Callback: Video Completed!";
        }

        public void ExitVideo()
        {
            vp.Stop();
            videoPlayerPanel.SetActive(false);
            rt.Release();
        }

        public void PlayVideo()
        {
            play.SetActive(false);
            pause.SetActive(true);
            vp.Play();
            wasPaused = false;
        }

        public void PauseVideo()
        {
            play.SetActive(true);
            pause.SetActive(false);
            vp.Pause();
            wasPaused = true;
        }
    }
}