using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullOnCloseBy : MonoBehaviour
{
    #region Variables

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer thisSpriteRenderer;
    [SerializeField] private Color transparency;

    [Header("States")]
    [SerializeField] private bool canFade = false;

    [Header("Customisable")]
    [SerializeField] private float smoothing = 10f;

    #endregion

    #region Processes

    private void Awake()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        transparency = thisSpriteRenderer.color;
    }

    private void LateUpdate()
    {
        FadeSpriteUponPlayerEntry();
    }

    #region Triggers

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canFade = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canFade)
        {
            canFade = false;
        }
    }

    #endregion

    #endregion

    #region Function Which Fades SpriteRenderer Using Parent GameObjects Alpha Values

    private void FadeSpriteUponPlayerEntry()
    {
        thisSpriteRenderer.color = transparency;

        switch (canFade)
        {
            case true:
                transparency.a = Mathf.Lerp(transparency.a, 0, smoothing * Time.deltaTime);
                break;
            case false:
                transparency.a = Mathf.Lerp(transparency.a, 1, smoothing / 2 * Time.deltaTime);
                break;
        }
    }

    #endregion
}
