
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockInteractable : MonoBehaviour
{
    public AudioSource clockSound;
    public void OnTriggerEnter(Collider other)
    {
        if (!clockSound.isPlaying)
            return;
        if (other.CompareTag("Hand"))
        {
            gameObject.GetComponentInChildren<Animator>().           SetTrigger("Stop");
            clockSound.Stop();
            StartCoroutine(BackToSceneOne());
        }
    }

    private IEnumerator BackToSceneOne()
    {
        yield return new WaitForSeconds(1f);
        SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
