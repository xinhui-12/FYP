using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

namespace ProjektSumperk
{
    public class VideoGallery : MonoBehaviour
    {
        [SerializeField] private VideoClip[] clips;
        [SerializeField] private Sprite[] thumbnails;
        [SerializeField] private GameObject videoThumbnailsPrefab;
        [SerializeField] private Transform parent;
        [SerializeField] private GameObject videoPlayer;

        private void Start()
        {
            for (int i = 0; i < clips.Length; i++)
            {
                GameObject video = Instantiate(videoThumbnailsPrefab, parent);
                video.GetComponent<GetVideoID>().SetVideoID(i);
                video.GetComponentInChildren<TMP_Text>().text = FormatTime(clips[i].length);
                video.GetComponent<Image>().sprite = thumbnails[i];
            }
        }

        private string FormatTime(double timeInSeconds)
        {
            int len = Mathf.RoundToInt((float)timeInSeconds);
            int minutes = len / 60;
            int seconds = len % 60;
            return $"{minutes:00}:{seconds:00}";
        }

        public void GetVideoID(int id)
        {
            videoPlayer.SetActive(true);
            VideoPlayer vp = videoPlayer.GetComponent<VideoPlayer>();
            if (vp != null)
            {
                vp.clip = clips[id];
                vp.Play();
            }
        }
    }
}