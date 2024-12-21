
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhotoGallery : MonoBehaviour
{
    public GameObject imagePrefab;
    public Transform parent;
    public GameObject FullScreenPanel;
    public AudioSource soundEffectsSource;

    // List of unlocked photos
    public List<string> unlockedPhotos = new List<string>();

    // Dictionary to track instantiated images
    private Dictionary<string, GameObject> instantiatedImages = new();

    // Define the all the image in gallery
    public List<Sprite> photoSprites = new();

    void Start()
    {
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
        // Ensure the sprites list is not empty
        if (photoSprites == null || photoSprites.Count == 0)
        {
            Debug.LogError("No photo sprites are assigned to the gallery!");
            return;
        }

        for (int i = 0; i < photoSprites.Count; i++)
        {
            Sprite sprite = photoSprites[i];
            string photoName = sprite.name; // Use sprite's name as an identifier

            // Skip already instantiated images
            if (instantiatedImages.ContainsKey(photoName))
                continue;

            // Instantiate the image prefab
            GameObject images = Instantiate(imagePrefab, parent);
            images.name = photoName;

            // Set the sprite directly
            Image imageComponent = images.GetComponent<Image>();
            imageComponent.sprite = sprite;

            // Add the GetPhoto component and set it up
            GetPhoto getPhoto = images.GetComponent<GetPhoto>();
            getPhoto.photoGallery = this;
            getPhoto.sprite = sprite;

            // Lock or unlock the photo based on its status
            bool isUnlocked = unlockedPhotos.Contains(photoName);
            LockOrUnlockImage(imageComponent, getPhoto, isUnlocked);

            // Add to instantiated images dictionary
            instantiatedImages[photoName] = images;
        }
    }


    void LockOrUnlockImage(Image img, GetPhoto getPhoto, bool isUnlocked)
    {
        GameObject lockIcon = img.transform.Find("LockIcon").gameObject;

        if (isUnlocked)
        {
            img.color = Color.white;
            getPhoto.gameObject.GetComponent<Button>().enabled = true;
            lockIcon?.SetActive(false);
        }
        else
        {
            img.color = new Color(0.2f, 0.2f, 0.2f);
            getPhoto.gameObject.GetComponent<Button>().enabled = false;
            lockIcon?.SetActive(true);
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
