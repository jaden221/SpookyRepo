using System.Collections;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] float lifetime = 9;
    float timeLeft = 0;

    SpriteRenderer renderer;
    Color normalColor;

    void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        
        normalColor = renderer.color;
        timeLeft = lifetime;

        StartCoroutine(HandleLifetime());
    }

    IEnumerator HandleLifetime()
    {
        Destroy(gameObject, lifetime);

        while (true) 
        {
            renderer.color = new Color(
                normalColor.r, 
                normalColor.g, 
                normalColor.b, 
                timeLeft / lifetime);

            timeLeft -= Time.deltaTime;

            yield return null;
        }
    }
}