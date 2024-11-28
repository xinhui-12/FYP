
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowMap : MonoBehaviour
{
    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            DisplayMap();
    }

    public void DisplayMap()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
