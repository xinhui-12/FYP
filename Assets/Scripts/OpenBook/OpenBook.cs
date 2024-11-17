
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenBook : MonoBehaviour
{
    public Animator bookAnimator;
    public XRGrabInteractable grabInteractable;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject bookCanva;

    private bool isHeld = false;
    [HideInInspector]
    public bool isBookOpen = false;

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        transform.rotation = Quaternion.Euler(90, 90, 0);
        GameObject handObj = args.interactorObject.transform.parent.gameObject;

        if (handObj == rightHand)
        {
            isHeld = true;
        }
        else if (handObj == leftHand && isHeld)
        {
            isBookOpen = true;
            bookAnimator.SetBool("isOpen", isBookOpen);
            StartCoroutine(EnableCanva());
        }
        else
        {
            grabInteractable.enabled = false;
            Invoke(nameof(ReenableGrabInteractable), 0.1f);
            return;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.gameObject == rightHand)
            isHeld = false;
        isBookOpen = false;
        bookAnimator.SetBool("isOpen", isBookOpen);
        bookCanva.SetActive(false);
    }

    IEnumerator EnableCanva()
    {
        AnimatorStateInfo stateInfo = bookAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle")) yield return null;
        while (stateInfo.IsName("OpenBook") && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
        }
        bookCanva.SetActive(true);
    }

    private void ReenableGrabInteractable()
    {
        grabInteractable.enabled = true;
    }
}
