
using UnityEngine;

public class UnlockHospitalAlbum : MonoBehaviour
{
    public PhotoGallery photoGallery;

    void Start()
    {
        photoGallery.UnlockPhoto(photoGallery.customOrder[0]);
    }
}
