using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public bool isInteracted { get; private set; } = false;
    public string gateID { get; private set; }

    [SerializeField]
    public InteractionTarget TargetToActivate;
    public InteractionTarget SecondaryTargetToActivate;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gateID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public void Interact()
    {
        PullLever();
        TargetToActivate?.Activate();
        if (SecondaryTargetToActivate != null)
        {
            SecondaryTargetToActivate.Activate();
        }
    }

    public void PullLever()
    {
        SetInteracted();
    }

    public void SetInteracted()
    {
        //Debug.Log("object interacted with!");
        if (isInteracted)
        {
            isInteracted = false;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
        else
        {
            isInteracted = true;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
    }

    public bool CanInteract()
    {
        return true;
    }
}
