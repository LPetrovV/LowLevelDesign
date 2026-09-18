

using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    public string message;
    public Text messageText;
    public GameObject messageBox;

    void Start()
    {
        if (messageText != null) messageText.text = message;
        if (messageBox != null) messageBox.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (messageText != null) messageText.text = message;
            if (messageBox != null) messageBox.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (messageBox != null) messageBox.SetActive(false);
            if (messageText != null) messageText.text = "";
        }
    }
}