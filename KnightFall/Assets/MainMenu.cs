using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "SampleScene";
    public GameObject mainPanel;
    public GameObject loadPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenLoadGame()
    {
        mainPanel.SetActive(false);
        loadPanel.SetActive(true);
    }

    public void CloseLoadGame()
    {
        loadPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}