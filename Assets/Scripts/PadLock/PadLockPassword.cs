
using System.Linq;
using System.Collections;
using UnityEngine;

public class PadLockPassword : MonoBehaviour
{
    MoveRuller _moveRull;

    public int[] _numberPassword = {0,0,0,0};
    public Animator _padlockAnimator;
    public OpenDoor _openDoor;

    private void Awake()
    {
        _moveRull = FindObjectOfType<MoveRuller>();
    }

    public void Password()
    {
        if (_moveRull._numberArray.SequenceEqual(_numberPassword))
        {
            _padlockAnimator.SetBool("Locked", false);
            StartCoroutine(PadlockDisappear());
        }
    }

    private IEnumerator PadlockDisappear()
    {
        // Wait until the "Opened" animation is playing
        while (!_padlockAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opened"))
        {
            yield return null;
        }

        // Wait until the "Opened" animation has fully completed
        while (_padlockAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        // After the animation is done, set the padlock object to inactive
        gameObject.SetActive(false);
        _openDoor.isLocked = false;
    }
}
