
using UnityEngine;

public class CheatBall : MonoBehaviour
{
    public BallManager BallManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            BallManager.ActivateCheatMode();
            Debug.Log("Cheat mode activated (Ball placed correctly)");
        }
    }
}
