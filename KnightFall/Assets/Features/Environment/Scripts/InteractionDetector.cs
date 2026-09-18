using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy entered: " + collision.gameObject.name);
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Enemy component found: " + enemy.gameObject.name);
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                enemy.OnTriggerEnter2D(player.GetComponent<Collider2D>());
            }
        }

        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.CanInteract())
            {
                interactableInRange = interactable;

                //Debug.Log("Interactable entered: " + collision.gameObject.name);
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

                //Debug.Log("Interactable left");
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        //Debug.Log("Interacting!");
        if (context.performed)
        {
            if (interactableInRange != null &&
                interactableInRange.CanInteract())
            {
                interactableInRange.Interact();
            }
        }
    }
}