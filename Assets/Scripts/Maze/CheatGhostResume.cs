
using UnityEngine;

public class CheatGhostResume : MonoBehaviour
{
    public Ghost[] ghost;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            foreach (Ghost g in ghost)
            {
                g.isCheatModeActive = false;
                g.agent.isStopped = false;
            }
            Debug.Log("Cheat mode activated (Ghost resume patrolling)");
        }
    }

}
