
using UnityEngine;

public class BombSelectionTrigger : MonoBehaviour
{
    // Event to notify other scripts which collider was triggered
    public delegate void ColliderTriggered(Collider collider);
    public static event ColliderTriggered OnColliderTriggered;

    public void OnTriggerEnter(Collider other)
    {
        if (BombDifuseLogic.Defuse) return;
        if (other.CompareTag("Hand"))
        {
            OnColliderTriggered?.Invoke(GetComponent<Collider>());
        }
    }

}
