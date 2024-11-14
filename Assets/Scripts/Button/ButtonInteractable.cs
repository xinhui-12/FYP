
using System.Collections;
using UnityEditor;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    public Animator btnAnimation;
    public int gameScene;

    [Header("Game Scene 1")]
    public GameObject mathHint;

    [Header("Game Scene 2")]
    public SlidingPuzzle sliding16;
    public bool correctButton;
    public GameObject alarmClock;

    [Header("Game Scene 3")]
    public GameObject sceneHint;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            btnAnimation.SetTrigger("PlayAnimation");
            switch (gameScene)
            {
                case 1:
                    StartCoroutine(ActivateMathHint());
                    break;
                case 2:
                    if (correctButton)
                    {
                        StartCoroutine(CompleteSliding16());
                    }
                    else
                    {
                        StartCoroutine(AlarmClockSound());
                    }
                    break;
                case 3:
                    StartCoroutine(ActivateSceneHint());
                    break; 
                default:
                    Debug.Log("No function");
                    break;
            }
            
        }
    }

    private IEnumerator ActivateMathHint()
    {
        // Wait until the animation state "ButtonPress" is finished
        AnimatorStateInfo stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName("ButtonPress") && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        }

        mathHint.SetActive(true);
    }

    private IEnumerator CompleteSliding16()
    {
        // Wait until the animation state "ButtonPress" is finished
        AnimatorStateInfo stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName("ButtonPress") && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        }

        if(sliding16.gameObject.activeSelf)
        {
            sliding16.CompletePuzzleDirectly();
        }
    }

    private IEnumerator AlarmClockSound()
    {
        // Wait until the animation state "ButtonPress" is finished
        AnimatorStateInfo stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName("ButtonPress") && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        }

        if (alarmClock != null)
        {
            alarmClock.GetComponent<AudioSource>().Play();
            alarmClock.GetComponentInChildren<Animator>().SetTrigger("Ring");
        }
    }

    private IEnumerator ActivateSceneHint()
    {
        // Wait until the animation state "ButtonPress" is finished
        AnimatorStateInfo stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName("ButtonPress") && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);
        }

        sceneHint.SetActive(true);
    }
}

#if UNITY_EDITOR

// Custom editor class for ConditionalVariables
[CustomEditor(typeof(ButtonInteractable))]
public class ConditionalVariablesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ButtonInteractable script = (ButtonInteractable)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("btnAnimation"));
        script.gameScene = EditorGUILayout.IntField("Game Scene", script.gameScene);

        // Display specific fields
        if (script.gameScene == 1)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mathHint"));
        }
        else if (script.gameScene == 2)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sliding16"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("correctButton"));
            if (!script.correctButton)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alarmClock"));
            }
        }
        else if (script.gameScene == 3)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneHint"));
        }

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
