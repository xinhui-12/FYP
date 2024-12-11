
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveRuller : MonoBehaviour
{
    PadLockPassword lockPassword;

    [HideInInspector]
    public List <GameObject> rullers = new List<GameObject>();
    [HideInInspector]
    public int[] numberArray = { 0, 0, 0, 0 };

    private float rotationStep = 36f;  // Rotation step per press
    private float initialRotation = 216f;  // Start at 0 equivalent
    private Dictionary<GameObject, float> currentRotations = new Dictionary<GameObject, float>();

    void Awake()
    {
        lockPassword = FindObjectOfType<PadLockPassword>();

        // Find and store rulers in the list
        rullers.Add(GameObject.Find("Ruller1"));
        rullers.Add(GameObject.Find("Ruller2"));
        rullers.Add(GameObject.Find("Ruller3"));
        rullers.Add(GameObject.Find("Ruller4"));

        // Initialize each ruler's starting rotation
        foreach (GameObject r in rullers)
        {
            r.transform.localRotation = Quaternion.Euler(initialRotation, 0, 0);
            currentRotations[r] = initialRotation;
        }

    }
    void Update()
    {
        lockPassword.Password();
    }

    private void OnEnable()
    {
        foreach (var r in rullers)
        {
            var interactable = r.GetComponent<XRSimpleInteractable>();
            interactable.selectEntered.AddListener(OnRulerSelected);
        }
    }

    private void OnDisable()
    {
        foreach (var r in rullers)
        {
            var interactable = r.GetComponent<XRSimpleInteractable>();
            interactable.selectEntered.RemoveListener(OnRulerSelected);
        }
    }

    private void OnRulerSelected(SelectEnterEventArgs args)
    {
        GameObject ruler = args.interactableObject.transform.gameObject;
        RotateRuler(ruler);
    }

    private void RotateRuler(GameObject ruler)
    {
        // Calculate the new rotation by decreasing 36 degrees
        float newRotation = (currentRotations[ruler] - rotationStep + 360) % 360;

        // Apply the rotation only along the X-axis
        ruler.transform.localRotation = Quaternion.Euler(newRotation, 0, 0);

        // Store the new rotation value
        currentRotations[ruler] = newRotation;

        // Update the number associated with the ruler
        UpdateRulerNumber(ruler, newRotation);
    }

    private void UpdateRulerNumber(GameObject ruler, float xangle)
    {
        // Calculate the index of the ruler
        int index = rullers.IndexOf(ruler);

        // Adjust the angle to align with 216 degrees as the starting point
        int newValue = Mathf.FloorToInt((216 - xangle + 360) % 360 / 36);

        // Update the number array if it changes
        if (newValue != numberArray[index])
        {
            numberArray[index] = newValue;
        }
    }
}
