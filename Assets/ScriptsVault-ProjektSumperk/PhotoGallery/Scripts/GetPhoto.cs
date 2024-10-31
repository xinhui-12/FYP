using UnityEngine;
using UnityEngine.UI;

namespace ProjektSumperk
{
    public class GetPhoto : MonoBehaviour
    {
        public Sprite sprite;
        PhotoGallery photoGallery;

        public void GetPhotoImagesOnClick()
        {
            photoGallery = FindObjectOfType<PhotoGallery>();
            sprite = gameObject.GetComponent<Image>().sprite;
            photoGallery.GetPhotoImages(sprite);

        }
    }
}