
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketNotifier : MonoBehaviour
{
    public BallManager ballManager;
    public OpenDoor door;
    public TMP_Text text;
    public AudioSource wordAudio;

    private XRSocketInteractor socketInteractor;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnObjectPlaced);
        socketInteractor.selectExited.AddListener(OnObjectRemoved);
    }

    private void OnDestroy()
    {
        socketInteractor.selectEntered.RemoveAllListeners();
        socketInteractor.selectExited.RemoveAllListeners();
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        GameObject placedObject = args.interactableObject.transform.gameObject;

        // Check the placement
        if (ballManager.IsObjectInCorrectSocket(placedObject, gameObject))
        {
            text.gameObject.SetActive(true);
            wordAudio.Play();
            ballManager.CheckAndOpenDoor(door);
        }
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        text.gameObject.SetActive(false);
    }
}
