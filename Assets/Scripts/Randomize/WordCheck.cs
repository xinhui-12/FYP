
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordCheck : MonoBehaviour
{
    public int word;
    public RulerSlash rulerSlash;
    public PhotoGallery photoGallery;

    void Update()
    {
        if(rulerSlash.slashTime == word)
        {
            photoGallery.UnlockPhoto(photoGallery.customOrder[5]);
            StartCoroutine(GoScene());
        }
    }

    IEnumerator GoScene()
    {
        yield return new WaitForSeconds(5);
        SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
