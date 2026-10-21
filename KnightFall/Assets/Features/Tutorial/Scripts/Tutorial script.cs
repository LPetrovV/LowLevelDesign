using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialFinal : MonoBehaviour
{
    public GameObject TutorialPanel;
    public TextMeshProUGUI InstructionText;
    public string Message1 = "Welcome to Knightfall";
    public string Message2 = "Press Space to jump";
    public string Message3 = "Press Q to use ladders";
    public string Message4 = "Press E to fight, Q to open doors";
    public float ShowTime = 5f;
    public KeyCode SkipKey = KeyCode.Escape;

    private bool tutorialSkipped = false;

    void Start()
    {
        if (TutorialPanel != null)
            TutorialPanel.SetActive(true);

        if (InstructionText != null)
        {
            InstructionText.text = Message1;
        }

        StartCoroutine(ShowMessages());
    }

    void Update()
    {
        if (Input.GetKeyDown(SkipKey) && !tutorialSkipped)
        {
            SkipTutorial();
        }
    }

    IEnumerator ShowMessages()
    {
        if (tutorialSkipped) yield break;

        yield return new WaitForSeconds(ShowTime);
        if (tutorialSkipped) yield break;
        if (InstructionText != null)
            InstructionText.text = Message2;

        yield return new WaitForSeconds(ShowTime);
        if (tutorialSkipped) yield break;
        if (InstructionText != null)
            InstructionText.text = Message3;

        yield return new WaitForSeconds(ShowTime);
        if (tutorialSkipped) yield break;
        if (InstructionText != null)
            InstructionText.text = Message4;

        yield return new WaitForSeconds(ShowTime);
        if (tutorialSkipped) yield break;

        HideTutorial();
    }

    void SkipTutorial()
    {
        tutorialSkipped = true;
        HideTutorial();
    }

    void HideTutorial()
    {
        if (TutorialPanel != null)
            TutorialPanel.SetActive(false);
    }
}