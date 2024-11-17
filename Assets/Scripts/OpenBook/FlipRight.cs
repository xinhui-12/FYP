
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipRight : MonoBehaviour
{
    public AutoFlip controlledBook;

    public void OnRightHandPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            controlledBook.FlipRightPage();
    }

}
