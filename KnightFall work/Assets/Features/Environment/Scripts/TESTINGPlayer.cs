using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed){
            //Debug.Log(" jump pressed!");
            rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
        }
             
        
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if (context.performed){
            //Debug.Log("movement pressed!");
            Vector2 movementInput = context.ReadValue<Vector2>();
            rb.linearVelocity = new Vector2(movementInput.x * 5f, rb.linearVelocity.y);
        }
        
    }
}


