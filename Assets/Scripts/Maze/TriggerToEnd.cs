
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToEnd : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end position!");
            SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
