
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RulerHandler : XRGrabInteractable
{
    public Transform tableTransform;
    private Vector3 resetPosition;
    private Quaternion resetRotation;
    public Vector3 positionLimitMin;
    public Vector3 positionLimitMax;
    private bool selected = false;

    void Start()
    {
        resetPosition = transform.position;
        resetRotation = transform.rotation;
    }

    private void Update()
    {
        if (selected)
            return;
        if (IsOutOfBounds(transform.position))
            ResetRulerPosition();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        transform.SetParent(tableTransform.parent);
        selected = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        selected = false;
    }

    private bool IsOutOfBounds(Vector3 position)
    {
        return position.x < positionLimitMin.x || position.x > positionLimitMax.x ||
               position.y < positionLimitMin.y || position.y > positionLimitMax.y ||
               position.z < positionLimitMin.z || position.z > positionLimitMax.z;
    }

    private void ResetRulerPosition()
    {
        transform.SetParent(tableTransform);
        transform.SetPositionAndRotation(resetPosition, resetRotation);
    }
}
