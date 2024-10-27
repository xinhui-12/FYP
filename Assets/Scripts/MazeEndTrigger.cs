
using UnityEngine;

public class MazeEndTrigger : MonoBehaviour
{
    private UIObjectInteraction uiObjectInteraction;

    public void Initialize(UIObjectInteraction interaction)
    {
        uiObjectInteraction = interaction;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == uiObjectInteraction.player)
        {
            Debug.Log("Player reached the end position!");
            uiObjectInteraction.resetPlayerPosition();
        }
    }
}
