
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CutSceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public FadeScreen fadeScreen;
    public bool endScene;
    public Camera cutSceneCamera;

    private void OnEnable()
    {
        if (fadeScreen != null)
            fadeScreen.OnFadeaOutComplete += StartCutScene;
    }

    public void StartCutScene()
    {
        // Disable head tracking
        XRDevice.DisableAutoXRCameraTracking(cutSceneCamera, true);
        // Play the cutscene automatically when the scene starts
        if (playableDirector)
        {
            playableDirector.stopped += OnCutSceneEnded;
            playableDirector.Play();
        }
        else
        {
            Debug.LogError("PlayableDirector is not assigned.");
        }
    }

    private void OnCutSceneEnded(PlayableDirector director)
    {
        XRDevice.DisableAutoXRCameraTracking(cutSceneCamera, false);
        if (!endScene)
            // Go to GameScene 1
            SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            // Back to main menu
            SceneTransitionManager.singleton.GoToScene(0);
    }
}
