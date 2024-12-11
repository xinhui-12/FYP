
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallManager : MonoBehaviour
{
    [Tooltip("List of object-socket pairs.")]
    public List<SocketObjectPair> socketObjectPairs;
    private Dictionary<GameObject, GameObject> objectToSocketMap;
    public PhotoGallery photoGallery;

    private void Awake()
    {
        objectToSocketMap = new Dictionary<GameObject, GameObject>();
        foreach (var pair in socketObjectPairs)
            objectToSocketMap.Add(pair.objectPair, pair.socketArea);
    }

    public bool IsObjectInCorrectSocket(GameObject obj, GameObject socket)
    {
        if (objectToSocketMap.TryGetValue(obj, out GameObject correctSocket))
            return correctSocket == socket;
        return false;
    }

    public bool AreAllPairsCorrect()
    {
        foreach (var pair in socketObjectPairs)
        {
            XRSocketInteractor socketInteractor = pair.socketArea.GetComponent<XRSocketInteractor>();
            if (socketInteractor && socketInteractor.firstInteractableSelected?.transform.gameObject != pair.objectPair)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckAndOpenDoor(OpenDoor openDoor)
    {
        if (AreAllPairsCorrect())
        {
            Debug.Log("All pairs are correct!");
            openDoor.isLocked = false;
            photoGallery.UnlockPhoto(photoGallery.customOrder[7]);
        }
    }

    public void ActivateCheatMode()
    {
        foreach (var pair in socketObjectPairs)
        {
            // Get the socket interactor for the socket area
            XRSocketInteractor socketInteractor = pair.socketArea.GetComponent<XRSocketInteractor>();

            if (socketInteractor != null)
            {
                // Get the interactable component for the object
                IXRSelectInteractable interactable = pair.objectPair.GetComponent<IXRSelectInteractable>();

                if (interactable != null)
                {
                    // Simulate placing the object in the socket
                    socketInteractor.StartManualInteraction(interactable);
                    Debug.Log($"Placing {pair.objectPair.name} in {pair.socketArea.name}");
                }
            }
        }

        // Check and unlock the door if all pairs are correct
        CheckAndOpenDoor(FindObjectOfType<OpenDoor>());
    }

}

[System.Serializable]
public class SocketObjectPair
{
    public GameObject objectPair;
    public GameObject socketArea;
}
