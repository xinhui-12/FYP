using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Ensure the camera is not null
        if (mainCamera != null)
        {
            // Calculate the rotation to face the camera
            Vector3 toCamera = mainCamera.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(toCamera, Vector3.up);

            // Apply the rotation to the UI element
            transform.rotation = rotation;
        }
    }
}