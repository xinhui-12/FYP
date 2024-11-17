
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallManager : MonoBehaviour
{
    [Tooltip("List of object-socket pairs.")]
    public List<SocketObjectPair> socketObjectPairs;
    private Dictionary<GameObject, GameObject> objectToSocketMap;

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
        }
    }

}

[System.Serializable]
public class SocketObjectPair
{
    public GameObject objectPair;
    public GameObject socketArea;
}
