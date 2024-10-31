using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjektSumperk
{
    public class UIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform dragObject;
        private Vector3 offset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Store the reference to the dragged object
            dragObject = GetComponent<RectTransform>();

            // Calculate the offset between the mouse position and the object's position
            offset = dragObject.position - (Vector3)eventData.position;

            // Set the object's sibling index to ensure it's rendered on top of other UI elements
            dragObject.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Update the object's position based on the mouse position and the offset
            dragObject.position = (Vector3)eventData.position + offset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Reset the reference to the dragged object
            dragObject = null;
        }
    }
}