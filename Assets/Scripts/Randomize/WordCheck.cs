
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordCheck : MonoBehaviour
{
    public int word;
    public RulerSlash rulerSlash;

    void Update()
    {
        if(rulerSlash.slashTime == word)
        {
            StartCoroutine(GoScene());
        }
    }

    IEnumerator GoScene()
    {
        yield return new WaitForSeconds(5);
        SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
