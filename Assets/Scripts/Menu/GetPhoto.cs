
using UnityEngine;

public class GetPhoto : MonoBehaviour
{
    public Sprite sprite;
    public PhotoGallery photoGallery;

    public void GetPhotoImagesOnClick()
    {
        if (photoGallery != null && sprite != null)
        {
            photoGallery.GetPhotoImages(sprite);
        }
    }
}