using UnityEngine;
using TMPro;

namespace ProjektSumperk
{
    public class Stopwatch : MonoBehaviour
    {
        // Stopwatch properties
        private float elapsedTime = 0f;
        private bool isStopwatchRunning = false;

        // TMP_Text to display stopwatch and events
        public TMP_Text stopwatchText;

        private string eventLog = "";

        // Events for stopwatch callbacks
        public delegate void StopwatchStart();
        public event StopwatchStart OnStopwatchStart;

        public delegate void StopwatchStop();
        public event StopwatchStop OnStopwatchStop;

        public delegate void StopwatchReset();
        public event StopwatchReset OnStopwatchReset;

        public delegate void StopwatchLap(float lapTime);
        public event StopwatchLap OnStopwatchLap;

        private void Start()
        {
            // Initialize the stopwatch
            UpdateStopwatchDisplay();
            UpdateEventLog("Stopwatch Initialized");

            // Register custom callback methods for all events
            OnStopwatchStart += HandleStopwatchStart;
            OnStopwatchStop += HandleStopwatchStop;
            OnStopwatchReset += HandleStopwatchReset;
            OnStopwatchLap += HandleStopwatchLap;
        }

        private void Update()
        {
            if (isStopwatchRunning)
            {
                elapsedTime += Time.deltaTime;
                UpdateStopwatchDisplay();
            }
        }

        public void StartStopwatch()
        {
            if (!isStopwatchRunning)
            {
                isStopwatchRunning = true;
                OnStopwatchStart?.Invoke();
            }
        }

        public void StopStopwatch()
        {
            if (isStopwatchRunning)
            {
                isStopwatchRunning = false;
                OnStopwatchStop?.Invoke();
            }
        }

        public void ResetStopwatch()
        {
            elapsedTime = 0f;
            UpdateStopwatchDisplay();
            OnStopwatchReset?.Invoke();
        }

        public void LapStopwatch()
        {
            if (isStopwatchRunning)
            {
                OnStopwatchLap?.Invoke(elapsedTime);
            }
        }

        private void UpdateStopwatchDisplay()
        {
            if (stopwatchText != null)
            {
                stopwatchText.text = FormatTime(elapsedTime) + "\n\n" + eventLog;
            }
        }

        private void UpdateEventLog(string log)
        {
            eventLog += log + "\n";
            UpdateStopwatchDisplay();
        }

        private string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            int milliseconds = Mathf.FloorToInt((timeInSeconds * 1000) % 1000);
            return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }

        // Custom callback methods for all events
        private void HandleStopwatchStart()
        {
            // Your custom logic for stopwatch start here
            UpdateEventLog("Stopwatch Started");
        }

        private void HandleStopwatchStop()
        {
            // Your custom logic for stopwatch stop here
            UpdateEventLog("Stopwatch Stopped");
        }

        private void HandleStopwatchReset()
        {
            // Your custom logic for stopwatch reset here
            UpdateEventLog("Stopwatch Reset");
        }

        private void HandleStopwatchLap(float lapTime)
        {
            // Your custom logic for stopwatch lap here
            UpdateEventLog("Lap Time: " + FormatTime(lapTime));
        }
    }
}