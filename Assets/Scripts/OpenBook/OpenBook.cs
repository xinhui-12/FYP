
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
    public Transform targetPosition;
    public float moveSpeed = 1f;

    private bool isHeld = false;

    private void Awake()
    {
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        //grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        //grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    private void Start()
    {
        bookCanva?.SetActive(false);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        
        GameObject handObj = args.interactorObject.transform.parent.gameObject;

        if (handObj == rightHand)
        {
            transform.rotation = Quaternion.Euler(90, 90, 0);
            isHeld = true;
            bookAnimator.SetBool("isOpen", false);
        }
        else if (handObj == leftHand && isHeld)
        {
            StartCoroutine(FlyAndOpenCoroutine());
        }
        else
        {
            grabInteractable.enabled = false;
            Invoke(nameof(ReenableGrabInteractable), 0.1f);
            return;
        }
    }

    IEnumerator FlyAndOpenCoroutine()
    {
        grabInteractable.enabled = false;

        while (Vector3.Distance(transform.position, targetPosition.position) > 0.1f)
        {
            transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime), Quaternion.RotateTowards(transform.rotation, targetPosition.rotation, moveSpeed * 50f * Time.deltaTime));
            yield return null;
        }

        transform.SetPositionAndRotation(targetPosition.position, targetPosition.rotation);

        bookAnimator?.SetBool("isOpen", true);

        // Wait until the "OpenBook" animation state starts
        while (true)
        {
            AnimatorStateInfo stateInfo = bookAnimator.GetCurrentAnimatorStateInfo(0);

            // Check if the Animator has entered the "OpenBook" state
            if (stateInfo.IsName("OpenBook"))
                break;

            yield return null; // Wait until the state changes
        }

        // Wait until the animation finishes
        while (true)
        {
            AnimatorStateInfo stateInfo = bookAnimator.GetCurrentAnimatorStateInfo(0);

            // Check if the animation is still playing 
            if (stateInfo.IsName("OpenBook") && stateInfo.normalizedTime >= 1.0f)
                break;

            yield return null; // Wait for the animation to complete
        }

        bookCanva?.SetActive(true);

        Invoke(nameof(ReenableGrabInteractable), 0.1f);
        isHeld = false;
    }

    private void ReenableGrabInteractable()
    {
        grabInteractable.enabled = true;
    }
}
