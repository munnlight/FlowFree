using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject nextButton;
    private int currentLevel = 1;

    public void ShowNextButton()
    {
        nextButton.SetActive(true);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void WinLevel()
    {
        currentLevel++;
    }
}
