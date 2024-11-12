
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public SlidingPuzzle sliding16;
    public OpenDoor openDoor;

    void Start()
    {
        if (sliding16 != null)
        {
            sliding16.OnPuzzleSolved += UnlockDoorFunction;
        }
    }

    void OnDisable()
    {
        if (sliding16 != null)
        {
            sliding16.OnPuzzleSolved -= UnlockDoorFunction;
        }
    }

    void UnlockDoorFunction()
    {
        openDoor.isLocked = false;
    }
}
