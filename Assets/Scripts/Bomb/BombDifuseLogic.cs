
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BombDifuseLogic : MonoBehaviour
{
    public TMP_Text stage;
    public TMP_Text display;
    public TMP_Text[] selectionDisplay;
    public TMP_Text[] stageHint = new TMP_Text[3];
    public TMP_Text bombDefuseDisplay;
    public CountdownTimer countdownTimer;
    public AudioSource correctAudio;
    public AudioSource explosionAudio;
    public AudioSource parentScoldAudio;
    public FadeScreen fadeScreen;

    private int stageNumber;
    private int displayNumber;
    private int triggerIndex;
    private int[] selectionNumber;
    private int stage2Num;
    private int stage2Index;
    private bool explode = false;
    [HideInInspector]
    public static bool Defuse { get; private set; } = false;
    private bool isExploring = false;

    public void Start()
    {
        stageNumber = 1;
        selectionNumber = new int[selectionDisplay.Length];
        for (int i = 0; i < selectionDisplay.Length; i++)
        {
            selectionNumber[i] = i + 1;
        }
        InitSelectionDisplay();

        if (countdownTimer == null)
        {
            countdownTimer = GetComponent<CountdownTimer>();
        }
        isExploring = false;
    }

    private void InitSelectionDisplay()
    {
        stage.SetText("Stage " + stageNumber.ToString());
        displayNumber = Random.Range(1, 5);
        display.SetText(displayNumber.ToString());

        for (int i = 0; i < selectionNumber.Length; i++)
        {
            int randomIndex = Random.Range(i, selectionNumber.Length);
            int temp = selectionNumber[i];
            selectionNumber[i] = selectionNumber[randomIndex];
            selectionNumber[randomIndex] = temp;
        }
        for (int i = 0; i < selectionDisplay.Length; i++)
        {
            selectionDisplay[i].SetText(selectionNumber[i].ToString());
        }
    }

    public void Update()
    {
        if (Defuse) return;

        if (countdownTimer.timesOut)
        {
            explode = true;
        }

        if (explode && !isExploring)
        {
            isExploring = true;
            HandleExplosion();
        }
    }

    private void OnEnable()
    {
        BombSelectionTrigger.OnColliderTriggered += HandleColliderTrigger;
    }

    private void OnDisable()
    {
        BombSelectionTrigger.OnColliderTriggered -= HandleColliderTrigger;
    }

    private void HandleColliderTrigger(Collider collider)
    {
        correctAudio.Play();

        if (collider.gameObject.name == "Selection1Collider")
        {
            triggerIndex = 1;
        }
        else if (collider.gameObject.name == "Selection2Collider")
        {
            triggerIndex = 2;
        }
        else if (collider.gameObject.name == "Selection3Collider")
        {
            triggerIndex = 3;
        }
        else if (collider.gameObject.name == "Selection4Collider")
        {
            triggerIndex = 4;
        }

        //Debug.Log($"trigger index: {triggerIndex}");
        bool isCorrect = false;
        switch (stageNumber)
        {
            case 1:
                isCorrect = Stage1(triggerIndex);
                break;
            case 2:
                isCorrect = Stage2(triggerIndex);
                break;
            case 3:
                isCorrect = Stage3(triggerIndex);
                if (isCorrect)
                {
                    Defuse = true;
                    HandleDefuse();
                    return;
                }
                break;
            default: break;
        }
        //Debug.Log($"Is correct: {isCorrect}");
        if (isCorrect && stageNumber < 3)
        {
            stageNumber++;
            foreach (TMP_Text hint in stageHint)
            {
                hint.gameObject.SetActive(false);
            }
            stageHint[stageNumber - 1].gameObject.SetActive(true);
            InitSelectionDisplay();
        }
        else if (!isCorrect)
        {
            explode = true;
        }

    }

    private bool Stage1(int triggerIndex)
    {
        bool isCorrect = false;
        switch (displayNumber)
        {
            case 1:
                if (triggerIndex == 1)
                    isCorrect = true;
                break;
            case 2:
                if (triggerIndex == 3)
                    isCorrect = true;
                break;
            case 3:
                if (triggerIndex == 2)
                    isCorrect = true;
                break;
            case 4:
                if (triggerIndex == 4)
                    isCorrect = true;
                break;
        }
        return isCorrect;
    }

    private bool Stage2(int triggerIndex)
    {
        bool isCorrect = false;
        int triggerNumber = selectionNumber[triggerIndex - 1];
        switch (displayNumber)
        {
            case 1:
                if (triggerNumber == 1)
                {
                    stage2Index = triggerIndex;
                    stage2Num = triggerNumber;
                    isCorrect = true;
                }
                break;
            case 2:
                if (triggerNumber == 3)
                {
                    stage2Index = triggerIndex;
                    stage2Num = triggerNumber;
                    isCorrect = true;
                }
                break;
            case 3:
                if (triggerNumber == 3)
                {
                    stage2Index = triggerIndex;
                    stage2Num = triggerNumber;
                    isCorrect = true;
                }
                break;
            case 4:
                if (triggerNumber == 2)
                {
                    stage2Index = triggerIndex;
                    stage2Num = triggerNumber;
                    isCorrect = true;
                }
                break;
        }
        return isCorrect;
    }

    private bool Stage3(int triggerIndex)
    {
        bool isCorrect = false;
        int triggerNumber = selectionNumber[triggerIndex - 1];
        switch (displayNumber)
        {
            case 1:
                if (triggerIndex == 1)
                    isCorrect = true;
                break;
            case 2:
                if (triggerIndex == stage2Index)
                    isCorrect = true;
                break;
            case 3:
                if (triggerNumber == 3)
                    isCorrect = true;
                break;
            case 4:
                if (triggerNumber == stage2Num)
                    isCorrect = true;
                break;
        }
        return isCorrect;
    }

    private void HandleDefuse()
    {
        countdownTimer.StopCountdown();
        Debug.Log("Bomb defused!");
        stage.gameObject.SetActive(false);
        display.gameObject.SetActive(false);
        foreach (TMP_Text selection in selectionDisplay)
        {
            selection.gameObject.SetActive(false);
        }
        foreach (TMP_Text hint in stageHint)
        {
            hint.gameObject.SetActive(false);
        }
        bombDefuseDisplay.gameObject.SetActive(true);
    }

    private void HandleExplosion()
    {
        if (explosionAudio != null && fadeScreen != null)
        {
            explosionAudio.PlayOneShot(explosionAudio.clip);
            fadeScreen.fadeDuration = 3;
            fadeScreen.FadeOut();
        }
        if (parentScoldAudio != null)
        {
            Invoke(nameof(PlayScoldAudio), explosionAudio.clip.length);
        }

        float resetDelay = explosionAudio.clip.length + parentScoldAudio.clip.length + 1;
        Debug.Log(resetDelay);
        Invoke(nameof(ResetScene), resetDelay);
    }

    private void PlayScoldAudio()
    {
        parentScoldAudio.Play();
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
