using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Instancing")]
    [SerializeField] public static GameManager Instance;

    [Space(5)]

    [Header("Scripts")]
    [SerializeField] private PlayerController plrMovementScript;

    [Header("States")]
    [SerializeField] public bool playerIsBeingChased = false;

    #endregion

    #region Processes

    private void Awake()
    {
        Instance = this;
        plrMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Start()
    {

    }

    void Update()
    {
        DebugInput();
    }

    #endregion

    // REMOVE THIS BEFORE RELEASE
    #region Function Which Contains Inputs That Cause InGame Events Used For Testing - REMOVE THIS BEFORE RELEASE
    // REMOVE THIS BEFORE RELEASE
    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Minus)) { plrMovementScript.PlayerLoseLife(1);  }
        else if (Input.GetKeyDown(KeyCode.Equals)) { plrMovementScript.PlayerLoseLife(-1);  }
    }

    #endregion

    #region Public Function For Easy Reach To Players Health

    public void EditPlayerHealth(int health) 
    {
        plrMovementScript.lives -= health;
    }

    #endregion

    #region Public Function Which Ends The Game

    public void EndGame() 
    {
        Debug.Log("Game Over");
    }
    
    #endregion
}
