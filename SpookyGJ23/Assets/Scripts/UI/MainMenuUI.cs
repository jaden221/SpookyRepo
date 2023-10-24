using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void LoadLevel() 
    { 
        SceneManager.LoadScene("Level1");
    }
}