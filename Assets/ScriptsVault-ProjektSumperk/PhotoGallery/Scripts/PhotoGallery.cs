
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhotoGallery : MonoBehaviour
{
    string path;
    public GameObject imagePrefab;
    public Transform parent;
    public GameObject FullScreenPanel;

    // List of unlocked photos
    public List<string> unlockedPhotos = new List<string>();

    void Start()
    {
        path = Application.dataPath + "/2D images/Album";
        LoadImageGallery();
        UnlockPhoto("pic1");
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
                    Image imageComponent = images.GetComponent<Image>();
                    StartCoroutine(LoadImage(fileURL, imageComponent, file.Name));

                    // Add the GetPhoto component and set it up
                    GetPhoto getPhoto = images.AddComponent<GetPhoto>();
                    getPhoto.photoGallery = this;

                    // Lock or unlock the photo based on its status
                    bool isUnlocked = unlockedPhotos.Contains(file.Name);
                    LockOrUnlockImage(imageComponent, getPhoto, isUnlocked);
                }
            }
        }
        else
        {
            Directory.CreateDirectory(path);
            return;
        }
    }

    void LockOrUnlockImage(Image img, GetPhoto getPhoto, bool isUnlocked)
    {
        if (isUnlocked)
        {
            img.color = Color.white; // Normal color
            getPhoto.enabled = true; // Enable click functionality
        }
        else
        {
            img.color = Color.gray; // Grayscale color
            getPhoto.enabled = false; // Disable click functionality
        }
    }

    IEnumerator LoadImage(string url, Image img, string imageName)
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

    public void UnlockPhoto(string photoName)
    {
        if (!unlockedPhotos.Contains(photoName))
        {
            unlockedPhotos.Add(photoName);
            PlayerPrefs.SetString("UnlockedPhoto_" + photoName, "true");
        }

        LoadImageGallery(); // Reload the gallery to apply changes
    }
}
