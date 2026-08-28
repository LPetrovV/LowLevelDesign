using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.CanInteract())
            {
                interactableInRange = interactable;

                Debug.Log("Interactable entered: " + collision.gameObject.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable == interactableInRange)
            {
                interactableInRange = null;

                Debug.Log("Interactable left");
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log("Interacting!");
        if (context.performed)
        {
            if (interactableInRange != null &&
                interactableInRange.CanInteract())
            {
                Debug.Log("Interacting!");

                interactableInRange.Interact();
            }
        }
    }
}