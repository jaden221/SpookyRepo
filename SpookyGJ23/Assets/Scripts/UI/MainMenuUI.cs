using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void LoadLevel() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }
}