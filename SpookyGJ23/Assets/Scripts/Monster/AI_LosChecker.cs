using System;
using System.Collections;
using UnityEngine;

public class AI_LosChecker : MonoBehaviour
{
    [SerializeField] LayerMask losObstructions;
    public event Action<Transform> OnHasTarget;
    public event Action OnNoTarget;

    Transform curTarget;

    bool hasLoS = false;

    void OnEnable()
    {
        StartCoroutine(CheckIfHasLoS());
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // if didn't hit any obstructions then it must have LoS
        if (curTarget != null && other.transform != curTarget) return;

        if (!Physics2D.Raycast(
            transform.position,
            other.transform.position - transform.position,
            Vector2.Distance(other.transform.position, transform.position),
            losObstructions)) 
        {
            curTarget = other.transform;
            hasLoS = true;
        }
    }

    IEnumerator CheckIfHasLoS() 
    { 
        while (true) 
        {
            yield return new WaitForFixedUpdate();

            if (curTarget == null)
            {
                hasLoS = false;
            }
            else if (hasLoS) 
            {
                OnHasTarget?.Invoke(curTarget);
            }
            else 
            {
                curTarget = null;
                OnNoTarget?.Invoke();
            }

            hasLoS = false;
        }
    }
}