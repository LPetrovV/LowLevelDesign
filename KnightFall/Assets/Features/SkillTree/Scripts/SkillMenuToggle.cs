using UnityEngine;
using UnityEngine.InputSystem;

public class SkillMenuToggle : MonoBehaviour
{
    public GameObject skillTreeCanvas;
    public Key toggleKey = Key.Tab;

    void Update()
    {
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            skillTreeCanvas.SetActive(!skillTreeCanvas.activeSelf);
        }
    }
}
