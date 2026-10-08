using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuCanvas;
    public Key toggleKey = Key.Tab;

    void Update()
    {
        // toggle menu when key is pressed 
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

            // Pause the game when the menu is active, and resume when it's closed
            if (menuCanvas.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
              Time.timeScale = 1f;  
            }
             
        }
    }
}
