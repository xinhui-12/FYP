using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ProjektSumperk
{
    public class PhotoGallery : MonoBehaviour
    {
        string path;
        public GameObject imagePrefab;
        public Transform parent;
        public GameObject FullScreenPanel;

        void Start()
        {
            path = Application.dataPath + "/ScriptsVault-ProjektSumperk/PhotoGallery/Photos";
            LoadImageGallery();
            Debug.Log(path);
        }

        public void GetPhotoImages(Sprite sprite)
        {
            FullScreenPanel.SetActive(true);
            FullScreenPanel.GetComponent<Image>().sprite = sprite;
        }

        void LoadImageGallery()
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo d = new DirectoryInfo(path);

                foreach (var extension in new string[] { "*.png", "*.jpg", "*.jpeg" })
                {
                    foreach (var file in d.GetFiles(extension))
                    {
                        GameObject images = Instantiate(imagePrefab, parent);
                        images.name = file.Name;
                        string fileURL = "file:///" + file.FullName;
                        StartCoroutine(LoadImage(fileURL, images.GetComponent<Image>()));
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(path);
                return;
            }
        }


        IEnumerator LoadImage(string url, Image img)
        {
            string extension = Path.GetExtension(url).ToLower();

            UnityWebRequest www;

            if (extension == ".png")
            {
                www = UnityWebRequestTexture.GetTexture(url);
            }
            else if (extension == ".jpg" || extension == ".jpeg")
            {
                www = UnityWebRequestTexture.GetTexture(url, true);
            }
            else
            {
                Debug.LogError("Unsupported image format: " + extension);
                yield break;
            }

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D textur = DownloadHandlerTexture.GetContent(www);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(textur, new Rect(0.0f, 0.0f, textur.width, textur.height), pivot, 100.0f);
                if (img) { img.sprite = sprite; }
            }
            else
            {
                Debug.LogError("Error loading image: " + www.error);
            }
        }
    }
}