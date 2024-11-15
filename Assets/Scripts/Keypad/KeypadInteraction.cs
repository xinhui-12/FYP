
using UnityEngine;

public class KeypadInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if(TryGetComponent(out KeypadButton keypadButton))
            {
                keypadButton.PressButton();
            }
        }
    }
}