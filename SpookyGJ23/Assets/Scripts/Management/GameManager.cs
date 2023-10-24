/*
    sso 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Instancing")]
    [SerializeField] public static GameManager Instance;

    [Space(5)]

    [Header("Scripts")]
    [SerializeField] private PlayerController plrMovementScript;

    [Space(5)]

    [Header("GameObjects")]
    [SerializeField] private GameObject gameLoseScreen;
    [SerializeField] private GameObject gameWinScreen;

    #endregion

    #region Processes

    private void Awake()
    {
        Instance = this;
        plrMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        gameWinScreen = GameObject.FindGameObjectWithTag("WinScreen");
        gameLoseScreen = GameObject.FindGameObjectWithTag("LoseScreen");
        gameWinScreen.SetActive(false);
        gameLoseScreen.SetActive(false);
    }

    void Update()
    {
        //DebugInput();
    }

    #endregion

    // REMOVE THIS BEFORE RELEASE - DONE
    #region Function Which Contains Inputs That Cause InGame Events Used For Testing - REMOVE THIS BEFORE RELEASE
    // REMOVE THIS BEFORE RELEASE
    /*(private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Minus)) { plrMovementScript.PlayerLoseLife(1);  }
        else if (Input.GetKeyDown(KeyCode.Equals)) { plrMovementScript.PlayerLoseLife(-1);  }
    }*/

    #endregion

    #region Public Function For Easy Reach To Players Health

    public void EditPlayerHealth(int health) 
    {
        plrMovementScript.lives -= health;
    }

    #endregion

    #region Public Function Which Ends The Game Depending On Parameter

    public void EndGame(bool win) 
    {
        plrMovementScript.canMove = false;
        Time.timeScale = 0;

        switch (win) 
        {
            case true:
                gameWinScreen.SetActive(true);
                break;
            case false: 
                gameLoseScreen.SetActive(true);
                break;
        }

        //release - Debug.Log("Game Over");
    }

    #endregion

    #region Public Function Which Closes The Game

    public void EndGame() 
    {
        Application.Quit();
    }

    #endregion

    #region Public Function Which Goes Back To The Menu

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
