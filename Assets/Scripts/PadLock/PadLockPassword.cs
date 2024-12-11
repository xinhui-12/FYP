
using System.Linq;
using System.Collections;
using UnityEngine;

public class PadLockPassword : MonoBehaviour
{
    MoveRuller moveRull;

    public int[] numberPassword = {0,0,0,0};
    public Animator padlockAnimator;
    public OpenDoor openDoor;

    public PhotoGallery photoGallery;

    private void Awake()
    {
        moveRull = FindObjectOfType<MoveRuller>();
    }

    public void Password()
    {
        if (moveRull.numberArray.SequenceEqual(numberPassword))
        {
            padlockAnimator.SetBool("Locked", false);
            photoGallery.UnlockPhoto(photoGallery.customOrder[2]);
            StartCoroutine(PadlockDisappear());
        }
    }

    private IEnumerator PadlockDisappear()
    {
        // Wait until the "Opened" animation is playing
        while (!padlockAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opened"))
        {
            yield return null;
        }

        // Wait until the "Opened" animation has fully completed
        while (padlockAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // After the animation is done, set the padlock object to inactive
        gameObject.SetActive(false);
        openDoor.isLocked = false;
        
    }
}
