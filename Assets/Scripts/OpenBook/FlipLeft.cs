
using UnityEngine;
using UnityEngine.InputSystem;

public class FlipLeft : MonoBehaviour
{
    public AutoFlip controlledBook;

    public void OnLeftHandPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
            controlledBook.FlipLeftPage();
    }

}
