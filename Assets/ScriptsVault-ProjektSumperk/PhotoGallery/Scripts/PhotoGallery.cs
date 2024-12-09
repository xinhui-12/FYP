
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
    public AudioSource soundEffectsSource;

    // List of unlocked photos
    public List<string> unlockedPhotos = new List<string>();

    // Dictionary to track instantiated images
    private Dictionary<string, GameObject> instantiatedImages = new();

    void Start()
    {
        path = Application.dataPath + "/2D images/Album";
        LoadUnlockedPhotos();
        LoadImageGallery();
        UnlockPhoto("pic1.png"); // Test Unlock
    }

    public void GetPhotoImages(Sprite sprite)
    {
        PlayClickSound();
        FullScreenPanel.SetActive(true);
        FullScreenPanel.GetComponent<Image>().sprite = sprite;
    }

    public void PlayClickSound()
    {
        if (soundEffectsSource != null)
        {
            soundEffectsSource.PlayOneShot(soundEffectsSource.clip);
        }
    }

    void LoadImageGallery()
    {
        if (Directory.Exists(path))
        {
            DirectoryInfo d = new(path);

            foreach (var extension in new string[] { "*.png", "*.jpg", "*.jpeg" })
            {
                foreach (var file in d.GetFiles(extension))
                {
                    // Skip already instantiated images
                    if (instantiatedImages.ContainsKey(file.Name))
                        continue;

                    GameObject images = Instantiate(imagePrefab, parent);
                    images.name = file.Name;
                    string fileURL = "file:///" + file.FullName;

                    Image imageComponent = images.GetComponent<Image>();
                    StartCoroutine(LoadImage(fileURL, imageComponent, file.Name));

                    // Add the GetPhoto component and set it up
                    GetPhoto getPhoto = images.GetComponent<GetPhoto>();
                    getPhoto.photoGallery = this;

                    // Lock or unlock the photo based on its status
                    bool isUnlocked = unlockedPhotos.Contains(file.Name);
                    LockOrUnlockImage(imageComponent, getPhoto, isUnlocked);

                    // Add to instantiated images dictionary
                    instantiatedImages[file.Name] = images;
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
            // Enable click functionality
            getPhoto.gameObject.GetComponent<Button>().enabled = true;
        }
        else
        {
            //img.color = Color.gray; // Grayscale color
            img.color = new Color(0.2f, 0.2f, 0.2f);
            // Disable click functionality
            getPhoto.gameObject.GetComponent<Button>().enabled = false;
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
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), pivot, 100.0f);
            if (img)
            {
                img.sprite = sprite;

                // Update GetPhoto component sprite
                if (img.gameObject.TryGetComponent<GetPhoto>(out var getPhoto))
                {
                    getPhoto.sprite = sprite;
                }
            }
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

            // Update existing instantiated image if it exists
            if (instantiatedImages.ContainsKey(photoName))
            {
                GameObject imageObj = instantiatedImages[photoName];
                Image img = imageObj.GetComponent<Image>();
                GetPhoto getPhoto = imageObj.GetComponent<GetPhoto>();
                LockOrUnlockImage(img, getPhoto, true);
            }
        }
    }

    void LoadUnlockedPhotos()
    {
        unlockedPhotos.Clear();

        foreach (var key in PlayerPrefs.GetString("UnlockedPhoto_", "").Split(','))
        {
            if (!string.IsNullOrEmpty(key))
                unlockedPhotos.Add(key);
        }
    }
}
