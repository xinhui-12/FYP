
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallInteractable : XRGrabInteractable
{
    public Canvas objectCanvas;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (args.interactorObject is XRSocketInteractor)
        {
            objectCanvas.gameObject.SetActive(false);
            return;
        }
        objectCanvas.gameObject.SetActive(true);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        objectCanvas.gameObject.SetActive(false);
    }

}
