using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuCanvas;
    public Key toggleKey = Key.Tab;

    void Update()
    {
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
