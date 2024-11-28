
using UnityEngine;

public class CheatGhostStop : MonoBehaviour
{
    public Ghost[] ghost;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            foreach (Ghost g in ghost)
            {
                g.isCheatModeActive = true;
            }
            Debug.Log("Cheat mode activated (Ghost stop at start point)");
        }
    }

}
