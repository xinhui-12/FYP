
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToEnd : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end position!");
            // Temporarily back to main menu, should be go to cut scene
            SceneTransitionManager.singleton.GoToScene(0);
        }
    }
}
