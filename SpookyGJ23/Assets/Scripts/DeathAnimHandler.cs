using UnityEngine;

public class DeathAnimHandler : MonoBehaviour
{
    Health health;
    Animator animator;
    
    void Awake()
    {
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        health.OnDeath += HandleDeath;    
    }

    void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    void HandleDeath()
    {
        animator.SetBool("Dead", true);
    }
}