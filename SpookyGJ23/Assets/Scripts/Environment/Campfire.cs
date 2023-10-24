using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float healWaitTime = 1.3f;
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private bool playerWantsHealth = false;

    [Header("GameObjects")]
    [SerializeField] private GameObject flame;
    [SerializeField] private GameObject player;

    [Header("Scripts")]
    [SerializeField] private P_Movement playerScript;

    #endregion

    #region Processes

    void Awake()
    {
        flame = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        ForWhenPlayerWantsHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            player = collision.gameObject;
            playerWantsHealth = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            playerWantsHealth = false;
        }
    }

    #endregion

    #region Function Player Wants Health
    private bool flameShouldScale = false;

    private void ForWhenPlayerWantsHealth() 
    {
        if (!playerWantsHealth) return;
        if (player == null) return;

        playerScript = player.GetComponent<P_Movement>();

        if (playerScript.lives < 3 && Input.GetKeyDown(KeyCode.R)) 
        {
            StartCoroutine(GivePlayerHealth());
        }

        if (flameShouldScale) flame.transform.localScale = Vector2.Lerp(flame.transform.localScale, Vector2.zero, smoothing * Time.deltaTime);
    }

    #endregion

    private IEnumerator GivePlayerHealth() 
    {
        playerScript.canMove = false;
        if (!player.transform.GetChild(0).GetComponent<Animator>().GetBool("heal")) 
        {
            player.transform.GetChild(0).GetComponent<Animator>().SetBool("heal", true);
            player.transform.GetChild(0).GetComponent<Animator>().SetBool("moving", false);
        }
        flameShouldScale = true;

        yield return new WaitForSeconds(healWaitTime);

        GameManager.Instance.EditPlayerHealth(-1);
        playerScript.ScalePlayerFlame();
        player.transform.GetChild(0).GetComponent<Animator>().SetBool("heal", false);
        player.transform.GetChild(0).GetComponent<Animator>().SetBool("moving", true);
        flame.SetActive(false);
        playerScript.canMove = true;
    }
}
