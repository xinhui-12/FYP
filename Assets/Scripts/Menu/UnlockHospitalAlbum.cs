
using UnityEngine;

public class UnlockHospitalAlbum : MonoBehaviour
{
    public PhotoGallery photoGallery;
    public AudioSource sonStruggleSound;

    void Start()
    {
        photoGallery.UnlockPhoto(photoGallery.customOrder[0]);
        sonStruggleSound.Play();
    }
}
