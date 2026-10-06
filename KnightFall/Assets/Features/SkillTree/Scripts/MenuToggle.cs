using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuCanvas;
    public Key toggleKey = Key.Tab;

    void Update()
    {
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

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
