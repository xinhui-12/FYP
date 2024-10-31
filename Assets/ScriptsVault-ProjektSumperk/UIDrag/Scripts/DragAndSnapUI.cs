using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace ProjektSumperk
{
    public class DragAndSnapUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public delegate void SnapCallback(Transform snappedObject);
        public event SnapCallback OnSnap;

        public Transform[] snapPoints; // Array of snap points where the UI can snap
        public float snapDistanceThreshold = 50f; // Distance threshold for snapping
        public TMP_Text logText; // Reference to the TMP_Text component to log results

        private RectTransform dragObject;
        private Vector2 offset; // Changed type to Vector2 for offset

        private Canvas canvas;

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();

            // Subscribe to the OnSnap event
            OnSnap += HandleSnap;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Store the reference to the dragged object
            dragObject = GetComponent<RectTransform>();

            // Calculate the offset between the mouse position and the object's position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(dragObject, eventData.position, canvas.worldCamera, out offset);

            // Set the object's sibling index to ensure it's rendered on top of other UI elements
            dragObject.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Update the object's position based on the mouse position and the offset
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out localPointerPosition))
            {
                dragObject.localPosition = localPointerPosition - offset; // Corrected subtraction
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Find the nearest snap point and snap the UI to it
            Transform snappedObject = SnapToNearestPoint();
            if (snappedObject != null && OnSnap != null)
            {
                OnSnap.Invoke(snappedObject);
            }
        }

        private Transform SnapToNearestPoint()
        {
            if (snapPoints == null || snapPoints.Length == 0)
                return null;

            float minDistance = float.MaxValue;
            Transform nearestSnapPoint = null;

            // Iterate through all snap points to find the nearest one
            foreach (Transform snapPoint in snapPoints)
            {
                float distance = Vector2.Distance(dragObject.position, snapPoint.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestSnapPoint = snapPoint;
                }
            }

            // Snap the UI to the nearest snap point if it's within the threshold distance
            if (nearestSnapPoint != null && minDistance <= snapDistanceThreshold)
            {
                dragObject.position = nearestSnapPoint.position;
                return nearestSnapPoint;
            }
            else
            {
                return null;
            }
        }

        // Method to handle the OnSnap event
        private void HandleSnap(Transform snappedObject)
        {
            // Log the result on TMP_Text
            if (logText != null)
            {
                logText.text = gameObject.name + " Snapped to: " + snappedObject.name; // Update text with the name of the snapped object
            }
        }
    }
}