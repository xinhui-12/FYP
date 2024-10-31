using UnityEngine;

namespace ProjektSumperk
{
    public class GetVideoID : MonoBehaviour
    {
        private int vid;
        private VideoGallery videoGallery;

        private void Start()
        {
            videoGallery = FindObjectOfType<VideoGallery>();
        }

        public void SetVideoID(int id)
        {
            vid = id;
        }

        public void GetVideoIdOnClick()
        {
            if (videoGallery != null)
            {
                videoGallery.GetVideoID(vid);
            }
        }
    }
}