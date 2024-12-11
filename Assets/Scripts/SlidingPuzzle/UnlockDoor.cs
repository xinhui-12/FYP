
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public SlidingPuzzle sliding16;
    public OpenDoor openDoor;
    public PhotoGallery photoGallery;

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
        photoGallery.UnlockPhoto(photoGallery.customOrder[4]);
    }
}
