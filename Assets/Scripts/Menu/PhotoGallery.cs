
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

    // Define custom order
    public List<string> customOrder = new();

    void Start()
    {
        path = Application.dataPath + "/2D images/Album";
        LoadUnlockedPhotos();
        LoadImageGallery();
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
            foreach (string fileName in customOrder)
            {
                string filePath = Path.Combine(path, fileName);

                // Check if the file exists in the directory
                if (File.Exists(filePath))
                {
                    // Skip already instantiated images
                    if (instantiatedImages.ContainsKey(fileName))
                        continue;

                    GameObject images = Instantiate(imagePrefab, parent);
                    images.name = fileName;
                    string fileURL = "file:///" + filePath;

                    Image imageComponent = images.GetComponent<Image>();
                    StartCoroutine(LoadImage(fileURL, imageComponent, fileName));

                    // Add the GetPhoto component and set it up
                    GetPhoto getPhoto = images.GetComponent<GetPhoto>();
                    getPhoto.photoGallery = this;

                    // Lock or unlock the photo based on its status
                    bool isUnlocked = unlockedPhotos.Contains(fileName);
                    LockOrUnlockImage(imageComponent, getPhoto, isUnlocked);

                    // Add to instantiated images dictionary
                    instantiatedImages[fileName] = images;
                }
                else
                {
                    Debug.LogWarning($"File {fileName} not found in directory.");
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
        GameObject lockIcon = img.transform.Find("LockIcon").gameObject;

        if (isUnlocked)
        {
            // Normal color
            img.color = Color.white;
            
            // Enable click functionality
            getPhoto.gameObject.GetComponent<Button>().enabled = true;

            // Hide the lock icon
            lockIcon?.SetActive(false);
        }
        else
        {
            // Grayscale color
            img.color = new Color(0.2f, 0.2f, 0.2f);

            // Disable click functionality
            getPhoto.gameObject.GetComponent<Button>().enabled = false;

            lockIcon?.SetActive(true);
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

            // Save the updated list as a single comma-separated string
            string unlockedPhotosString = string.Join(",", unlockedPhotos);
            PlayerPrefs.SetString("UnlockedPhotos", unlockedPhotosString);
            PlayerPrefs.Save();

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

        // Retrieve the list as a single comma-separated string
        string unlockedPhotosString = PlayerPrefs.GetString("UnlockedPhotos", "");

        if(!string.IsNullOrEmpty(unlockedPhotosString))
        {
            unlockedPhotos.AddRange(unlockedPhotosString.Split(","));
        }
    }
}
