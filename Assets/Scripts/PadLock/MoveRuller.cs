
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveRuller : MonoBehaviour
{
    PadLockPassword _lockPassword;

    [HideInInspector]
    public List <GameObject> _rullers = new List<GameObject>();
    [HideInInspector]
    public int[] _numberArray = { 0, 0, 0, 0 };

    private float _rotationStep = 36f;  // Rotation step per press
    private float _initialRotation = 216f;  // Start at 0 equivalent
    private Dictionary<GameObject, float> _currentRotations = new Dictionary<GameObject, float>();

    void Awake()
    {
        _lockPassword = FindObjectOfType<PadLockPassword>();

        // Find and store rulers in the list
        _rullers.Add(GameObject.Find("Ruller1"));
        _rullers.Add(GameObject.Find("Ruller2"));
        _rullers.Add(GameObject.Find("Ruller3"));
        _rullers.Add(GameObject.Find("Ruller4"));

        // Initialize each ruler's starting rotation
        foreach (GameObject r in _rullers)
        {
            r.transform.localRotation = Quaternion.Euler(_initialRotation, 0, 0);
            _currentRotations[r] = _initialRotation;
        }

    }
    void Update()
    {
        _lockPassword.Password();
    }

    private void OnEnable()
    {
        foreach (var r in _rullers)
        {
            var interactable = r.GetComponent<XRSimpleInteractable>();
            interactable.selectEntered.AddListener(OnRulerSelected);
        }
    }

    private void OnDisable()
    {
        foreach (var r in _rullers)
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
        float newRotation = (_currentRotations[ruler] - _rotationStep + 360) % 360;

        // Apply the rotation only along the X-axis
        ruler.transform.localRotation = Quaternion.Euler(newRotation, 0, 0);

        // Store the new rotation value
        _currentRotations[ruler] = newRotation;

        // Update the number associated with the ruler
        UpdateRulerNumber(ruler, newRotation);
    }

    private void UpdateRulerNumber(GameObject ruler, float xangle)
    {
        // Calculate the index of the ruler
        int index = _rullers.IndexOf(ruler);

        // Adjust the angle to align with 216 degrees as the starting point
        int newValue = Mathf.FloorToInt((216 - xangle + 360) % 360 / 36);

        // Update the number array if it changes
        if (newValue != _numberArray[index])
        {
            _numberArray[index] = newValue;
            Debug.Log($"Ruler {index} changed to: {newValue}");
        }
    }
}
