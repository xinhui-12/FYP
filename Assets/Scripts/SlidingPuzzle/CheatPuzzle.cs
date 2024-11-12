
using UnityEngine;

public class CheatPuzzle : MonoBehaviour
{
    public SlidingPuzzle slidingPuzzle;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            slidingPuzzle.CompletePuzzleDirectly();
        }
    }
}
