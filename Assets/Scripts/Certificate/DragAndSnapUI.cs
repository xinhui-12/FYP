
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DragAndSnapUI : XRGrabInteractable
{
    public CertificatePosition certificateScript;
    public float snapDistanceThreshold = 1f;

    private Transform initialParent;

    protected override void Awake()
    {
        base.Awake();
        initialParent = transform.parent;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        // Ensure UI element stays above others during drag
        transform.SetAsLastSibling();
        transform.SetParent(initialParent);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        SnapToNearestPoint();
        transform.SetParent(initialParent);
    }

    private void SnapToNearestPoint()
    {
        if (certificateScript.snapPoints == null || certificateScript.snapPoints.Length == 0) return;

        float minDistance = float.MaxValue;
        RectTransform nearestSnapPoint = null;
        ClearCurrentSnapPoint();

        // Iterate through all snap points to find the nearest one
        foreach (RectTransform snapPoint in certificateScript.snapPoints)
        {

            if (certificateScript.snapPointOccupancy[snapPoint] == null)
            {
                float distance = Vector2.Distance(transform.position, snapPoint.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSnapPoint = snapPoint;
                }
            }
        }

        // Snap the UI to the nearest snap point if it's within the threshold distance
        if (nearestSnapPoint != null && minDistance <= snapDistanceThreshold)
        {
            transform.position = nearestSnapPoint.position;
            certificateScript.snapPointOccupancy[nearestSnapPoint] = (RectTransform)transform;
            certificateScript.CheckCorrectPosition();
        }

        if(transform.localPosition.z > 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }
    }
    private void ClearCurrentSnapPoint()
    {
        // Iterate over the snap points to clear any previous occupancy if this object was already assigned to a snap point
        foreach (var entry in certificateScript.snapPointOccupancy)
        {
            if (entry.Value == transform)
            {
                certificateScript.snapPointOccupancy[entry.Key] = null;
                break;
            }
        }
    }
}
