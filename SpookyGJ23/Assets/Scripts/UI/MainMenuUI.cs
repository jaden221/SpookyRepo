using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public void LoadLevel(int buildIndex) 
    { 
        SceneManager.LoadScene(buildIndex);
    }
}