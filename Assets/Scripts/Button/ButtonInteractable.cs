
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
    public Drawer_Pull_X drawer;
    public bool correctButton;
    public GameObject alarmClock;
    public SlidingPuzzle sliding16;

    [Header("Game Scene 3")]
    public GameObject sceneHint;

    [Header("Game Scene 4")]
    public GameObject passwordHint;

    [Header("Game Scene 5")]
    public GameObject mapShowHint;
    public GameObject map;

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
                    if (drawer.open)
                    {
                        if (correctButton)
                        {
                            StartCoroutine(CompleteSliding16());
                        }
                        else
                        {
                            StartCoroutine(AlarmClockSound());
                        }
                    }
                    break;
                case 3:
                    StartCoroutine(ActivateSceneHint());
                    break;
                case 4:
                    StartCoroutine(ActivatePasswordHint());
                    break;
                case 5:
                    StartCoroutine(ActivateMap());
                    break;
                default:
                    Debug.Log("No function");
                    break;
            }

        }
    }

    private IEnumerator AnimationRun()
    {
        // Wait until the "Button" animation state starts
        while (true)
        {
            AnimatorStateInfo stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);

            // Check if the Animator has entered the "OpenBook" state
            if (stateInfo.IsName("ButtonPress"))
                break;

            yield return null; // Wait until the state changes
        }

        // Wait until the animation finishes
        while (true)
        {
            AnimatorStateInfo stateInfo = btnAnimation.GetCurrentAnimatorStateInfo(0);

            // Check if the animation is still playing 
            if (stateInfo.IsName("ButtonPress") && stateInfo.normalizedTime > 0.9f)
                break;

            yield return null; // Wait for the animation to complete
        }
    }

    private IEnumerator ActivateMathHint()
    {
        yield return AnimationRun();

        mathHint.SetActive(true);
    }

    private IEnumerator CompleteSliding16()
    {
        yield return AnimationRun();

        if (sliding16.gameObject.activeSelf)
        {
            sliding16.CompletePuzzleDirectly();
        }
    }

    private IEnumerator AlarmClockSound()
    {
        yield return AnimationRun();

        if (alarmClock != null)
        {
            alarmClock.GetComponent<AudioSource>().Play();
            alarmClock.GetComponentInChildren<Animator>().SetTrigger("Ring");
        }
    }

    private IEnumerator ActivateSceneHint()
    {
        yield return AnimationRun();

        sceneHint.SetActive(true);
    }

    private IEnumerator ActivatePasswordHint()
    {
        yield return AnimationRun();

        passwordHint.SetActive(true);
    }

    private IEnumerator ActivateMap()
    {
        yield return AnimationRun();

        mapShowHint.SetActive(true);
        map.SetActive(true);
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("correctButton"));

            if (!script.correctButton)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alarmClock"));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sliding16"));
            }
        }
        else if (script.gameScene == 3)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneHint"));
        }
        else if (script.gameScene == 4)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("passwordHint"));
        }
        else if (script.gameScene == 5)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mapShowHint"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("map"));
        }

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
