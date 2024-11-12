
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    // Countdown timer properties
    public float initialTime = 300f;
    private float currentTime = 0f;
    [HideInInspector]
    public bool isCountdownRunning = false;
    [HideInInspector]
    public bool timesOut = false;

    // TMP_Text to display countdown and events
    public TMP_Text countdownText;

    // Events for countdown callbacks
    public delegate void CountdownStart();
    public event CountdownStart OnCountdownStart;

    public delegate void CountdownComplete();
    public event CountdownComplete OnCountdownComplete;

    public OpenCloset[] closet;
    [HideInInspector]
    public bool closetIsOpen = false;

    private void Start()
    {
        // Initialize the countdown timer
        currentTime = initialTime;
        UpdateCountdownDisplay();
    }

    private void Update()
    {

        if (isCountdownRunning && closetIsOpen)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountdownRunning = false;
                timesOut = true;
                OnCountdownComplete?.Invoke();
            }

            UpdateCountdownDisplay();
        }
        else if(!isCountdownRunning && !closetIsOpen)
        {
            foreach (var closet in closet)
            {
                if (closet.isOpen)
                {
                    closetIsOpen = true;
                    StartCountdown();
                    break;
                }
            }
        }
    }

    public void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            timesOut = false;
            OnCountdownStart?.Invoke();
        }
    }

    public void StopCountdown()
    {
        isCountdownRunning = false;
    }

    private void UpdateCountdownDisplay()
    {
        if (countdownText != null)
        {
            countdownText.text = FormatTime(currentTime);
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}