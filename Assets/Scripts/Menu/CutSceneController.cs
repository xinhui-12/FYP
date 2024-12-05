
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public FadeScreen fadeScreen;
    public bool endScene;

    private void OnEnable()
    {
        if (fadeScreen != null)
            fadeScreen.OnFadeaOutComplete += StartCutScene;
    }

    public void StartCutScene()
    {
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
        if (!endScene)
            // Transition to the game scene when the cutscene ends
            SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneTransitionManager.singleton.GoToScene(0);
    }
}
