using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

namespace ProjektSumperk
{
    public class Screenshots : MonoBehaviour
    {
        public KeyCode captureKey = KeyCode.Space;
        public float captureInterval = 5.0f;
        private float captureTimer = 0.0f;
        public int captureWidth;
        public int captureHeight;
        public GameObject hideGameObject;
        public bool optimizeForManyScreenshots = true;
        public enum Format { JPG, PNG };
        public Format format = Format.JPG;
        private Rect rect;
        private RenderTexture renderTexture;
        private Texture2D screenShot;
        public Image screenShotPanel;
        private string folder;
        public bool capture = false;

        public Button captureButton; // Assign this in the Inspector
        public Button timedCaptureButton; // Assign this in the Inspector
        public TMP_Text timerText; // Assign this in the Inspector
        private bool isTimedCaptureActive = false;

        private void Start()
        {
            folder = GetPlatformDirectoryPath();

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Add click event listeners to buttons
            captureButton.onClick.AddListener(CaptureScreenshot);
            timedCaptureButton.onClick.AddListener(ToggleTimedCapture);
        }

        private void ToggleTimedCapture()
        {
            isTimedCaptureActive = !isTimedCaptureActive;
            captureTimer = 0.0f; // Reset timer

            if (isTimedCaptureActive)
            {
                timedCaptureButton.GetComponentInChildren<TMP_Text>().text = "Stop Timed Capture";
            }
            else
            {
                timedCaptureButton.GetComponentInChildren<TMP_Text>().text = "Start Timed Capture";
            }
        }

        private string GetPlatformDirectoryPath()
        {
            string platformPath;

#if UNITY_STANDALONE_WIN
            platformPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "MyGameScreenshots");
#elif UNITY_STANDALONE_OSX
        platformPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyGameScreenshots");
#elif UNITY_ANDROID
        platformPath = Path.Combine(Application.persistentDataPath, "MyGameScreenshots");
#elif UNITY_IOS
        platformPath = Path.Combine(Application.persistentDataPath, "MyGameScreenshots");
#else
        platformPath = Application.persistentDataPath;
#endif

            if (!Directory.Exists(platformPath))
            {
                Directory.CreateDirectory(platformPath);
            }

            Debug.Log(platformPath);

            return platformPath;
        }

        public void LoadImages(byte[] imageByte, Image img)
        {
            Texture2D texture = new Texture2D(1920, 1080);
            texture.LoadImage(imageByte);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), pivot, 100.0f);
            img.sprite = sprite;
        }

        private void Update()
        {
            try
            {
                // Check for key input
                if (Input.GetKeyDown(captureKey))
                {
                    CaptureScreenshot();
                }

                // Timed capture
                if (isTimedCaptureActive)
                {
                    captureTimer += Time.deltaTime;
                    if (captureTimer >= captureInterval)
                    {
                        CaptureScreenshot();
                        captureTimer = 0.0f; // Reset timer
                    }
                    // Update timer text
                    timerText.text = string.Format("Timer: {0:F1}s", captureInterval - captureTimer);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Screenshot capture error: {e.Message}");
            }

            // Rest of the script remains unchanged
            if (capture)
            {
                capture = false;

                if (hideGameObject != null) hideGameObject.SetActive(false);

                if (renderTexture == null)
                {
                    rect = new Rect(0, 0, captureWidth, captureHeight);
                    renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                    screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
                }

                Camera camera = this.GetComponent<Camera>();
                camera.targetTexture = renderTexture;
                camera.Render();

                RenderTexture.active = renderTexture;
                screenShot.ReadPixels(rect, 0, 0);

                camera.targetTexture = null;
                RenderTexture.active = null;

                string filename = uniqueFilename((int)rect.width, (int)rect.height);
                byte[] fileHeader = null;
                byte[] fileData = null;

                if (format == Format.PNG)
                {
                    fileData = screenShot.EncodeToPNG();
                    LoadImages(fileData, screenShotPanel);
                }
                else if (format == Format.JPG)
                {
                    fileData = screenShot.EncodeToJPG();
                    LoadImages(fileData, screenShotPanel);
                }

                new System.Threading.Thread(() =>
                {
                    var f = System.IO.File.Create(filename);
                    if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                    f.Write(fileData, 0, fileData.Length);
                    f.Close();
                }).Start();

                if (hideGameObject != null) hideGameObject.SetActive(true);

                if (!optimizeForManyScreenshots)
                {
                    Destroy(renderTexture);
                    renderTexture = null;
                    screenShot = null;
                }
            }
        }

        // Rest of the script remains unchanged

        private void CaptureScreenshot()
        {
            capture = true;
        }

        private string uniqueFilename(int width, int height)
        {
            folder = GetPlatformDirectoryPath();

            int rand = UnityEngine.Random.Range(10000, 999999);
            var filename = string.Format("{0}/IMG_{1}.{2}", folder, rand, format.ToString().ToLower());
            return filename;
        }
    }
}