using UnityEngine;

public class Gate : MonoBehaviour, IInteractable
{
    public bool isOpened { get; private set; } = false;
    public string gateID { get; private set; }
    public Sprite openedGateSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gateID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenGate();
    }

    public bool CanInteract()
    {
        return !isOpened;
    }

    public void OpenGate()
    {
        SetOpened(true);
    }

    public void SetOpened(bool opened)
    {
        isOpened = opened;
        if (isOpened)
        {
            GetComponent<SpriteRenderer>().sprite = openedGateSprite;
        }
    }
}
