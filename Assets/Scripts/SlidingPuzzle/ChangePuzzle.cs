
using UnityEngine;

public class ChangePuzzle : MonoBehaviour
{
    public SlidingPuzzle sliding8;
    public GameObject sliding16;

    void Start()
    {
        if(sliding8 != null)
        {
            sliding8.OnPuzzleSolved += ChangeSliding;
        }
    }

    void OnDisable()
    {
        if(sliding8 != null)
        {
            sliding8.OnPuzzleSolved -= ChangeSliding;
        }
    }

    void ChangeSliding()
    {
        Invoke("ChangeToSliding16", 1f);
    }

    public void ChangeToSliding16()
    {
        sliding8.gameObject.SetActive(false);
        sliding16.SetActive(true);
    }
}
