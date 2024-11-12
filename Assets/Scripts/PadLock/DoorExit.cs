
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorExit : MonoBehaviour
{
    public OpenDoor openDoor;
    public void OnTriggerEnter(Collider other)
    {
        if (!openDoor.isOpen) return;
        if (other.CompareTag("Player"))
        {
            SceneTransitionManager.singleton.GoToScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
