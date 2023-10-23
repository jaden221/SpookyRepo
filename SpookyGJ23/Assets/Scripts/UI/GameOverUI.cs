using System.Collections;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;

    void Awake()
    {
        StartCoroutine(LateAwake());   
    }

    IEnumerator LateAwake() 
    { 
        yield return new WaitForSeconds(1);

        GameObject.FindGameObjectWithTag("Monster").GetComponent<Health>().OnDeath += HandleMonsterDeath;
    }

    void HandleMonsterDeath()
    {
        Time.timeScale = 0;
        winUI.SetActive(true);
    }

    void HandlePlayerDeath() 
    {
        Time.timeScale = 0;
        loseUI.SetActive(true);
    }
}
