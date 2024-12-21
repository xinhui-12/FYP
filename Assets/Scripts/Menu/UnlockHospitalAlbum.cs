
using UnityEngine;

public class UnlockHospitalAlbum : MonoBehaviour
{
    public PhotoGallery photoGallery;
    public AudioSource sonStruggleSound;

    void Start()
    {
        photoGallery.UnlockPhoto(photoGallery.photoSprites[0].name);
        sonStruggleSound.Play();
    }
}
