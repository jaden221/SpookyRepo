/*
    Hi Isla :)
    Hi James :)
    Hi Frankie :)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Campfire : MonoBehaviour
{
    #region Variables

    [Header("Settings")]
    [SerializeField] private float healWaitTime = 1.3f;
    [SerializeField] private float smoothing = 5f;
    [SerializeField] private bool playerWantsHealth = false;

    [Header("GameObjects")]
    [SerializeField] private GameObject flame;
    [SerializeField] private GameObject flameLight;
    [SerializeField] private GameObject audioSource;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject player;

    [Header("Input")]
    [SerializeField] private InputActionReference absorbRef;
    private InputAction absorb;

    [Header("Scripts")]
    [SerializeField] private PlayerController playerScript;

    #endregion

    #region Processes

    void Awake()
    {
        absorb = absorbRef;
        flame = transform.GetChild(0).gameObject;
        flameLight = transform.GetChild(1).gameObject;
        shadow = transform.GetChild(2).gameObject;
        audioSource = transform.GetChild(3).gameObject;
    }

    void Update()
    {
        ForWhenPlayerWantsHealth();
    }

    void OnEnable()
    {
        absorb.Enable();
    }

    void OnDisable()
    {
        absorb.Disable();
    }

    #region Triggers

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

    #endregion

    #region Function Which Triggers When Upon The Player Wanting To Heal
    private bool flameShouldScale = false;

    private void ForWhenPlayerWantsHealth() 
    {
        if (!playerWantsHealth) return;
        if (player == null) return;

        playerScript = player.GetComponent<PlayerController>();

        if (playerScript.lives < 3 && absorb.WasPressedThisFrame()) 
        {
            StartCoroutine(GivePlayerHealth());
        }

        if (flameShouldScale) flame.transform.localScale = Vector2.Lerp(flame.transform.localScale, Vector2.zero, smoothing * Time.deltaTime);
    }

    #endregion

    #region Function Which Enables Campfires To Give Players Health

    private IEnumerator GivePlayerHealth()
    {
        var playerSprite = player.transform.GetChild(0).GetComponent<Animator>(); //local reference to anim - don't want to store it perm

        playerScript.canMove = false;
        if (!playerSprite.GetBool("heal"))
        {
            playerSprite.SetBool("heal", true);
            playerSprite.SetBool("moving", false);
        }
        flameShouldScale = true;
        
        yield return new WaitForSeconds(healWaitTime);

        GameManager.Instance.EditPlayerHealth(-1); // positive
        playerScript.ScalePlayerFlame();
        playerSprite.SetBool("heal", false);
        playerSprite.SetBool("moving", true);
        flame.SetActive(false);
        flameLight.SetActive(false);
        shadow.SetActive(false);
        audioSource.SetActive(false);
        
        playerScript.canMove = true;
    }

    #endregion
}
