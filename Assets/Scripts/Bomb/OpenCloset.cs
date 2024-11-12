
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenCloset : XRGrabInteractable
{
    public Vector3 targetRotation;
    public float rotationSpeed = 2.0f;
    [HideInInspector]
    public bool isOpen = false;
    private Quaternion finalRotation;
    private bool isRotating = false;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        finalRotation = Quaternion.Euler(targetRotation);
        isRotating = true;
        isOpen = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isRotating = false; // Stop rotating when object is released
    }

    void Update()
    {
        if (isRotating)
        {
            // Check if the rotation is close enough to the target
            if (Quaternion.Angle(transform.rotation, finalRotation) < 0.1f)
            {
                transform.rotation = finalRotation; // Snap to the final rotation
                isRotating = false; // Stop rotation once reached
            }

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                finalRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
