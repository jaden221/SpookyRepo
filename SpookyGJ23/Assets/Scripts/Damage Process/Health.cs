using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float curHealth;
    [SerializeField] bool resetOnEnable = true;

    bool isDead = false;

    public event Action OnBeforeDeath;
    public event Action OnDeath;
    /// <summary>
    /// float 1 = curHealth, float 2 = HealthPercent, float 3 = amount changed
    /// </summary>
    public event Action<float, float, float> OnHealthChanged;
    //Mostly for UI purposes
    public event Action<float> OnMaxHealthChanged;

    public float GetMaxHealth
    {
        get { return maxHealth; }
    }

    public float GetHealthPercent
    {
        get { return curHealth / maxHealth; }
    }

    public float GetCurHealth
    {
        get { return curHealth; }
    }

    void Awake()
    {
        if (maxHealth <= 0) Debug.Log($"maxHealth is too low on {transform.name}");

        curHealth = maxHealth;
    }

    void OnEnable()
    {
        if (resetOnEnable) curHealth = maxHealth;
    }

    public void HandleDamagedReceived(DamageDataReceived data)
    {
        // if the damage is more than the health then the damage dealt is however much health is left... otherwise it's the total damage
        if (data.dmgdata.TotalDamage() >= curHealth) data.dmgdata.damageDealt = curHealth;
        else data.dmgdata.damageDealt = data.dmgdata.TotalDamage();

        AddHealth(-data.dmgdata.TotalDamage());
    }

    public void AddHealth(float value)
    {
        curHealth += value;
        curHealth = Mathf.Clamp(curHealth, 0f, maxHealth);
        OnHealthChanged?.Invoke(curHealth, GetHealthPercent, value);

        CheckForDeath();
    }

    void CheckForDeath()
    {
        if (isDead) return;

        if (curHealth == 0)
        {
            OnBeforeDeath?.Invoke();
        }

        // if one of those functions hasn't added some health back preventing the death... then actually die
        if (curHealth == 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// If refill is true will set the curHealth to max automatically
    /// </summary>
    /// <param name="newMaxHealth"></param>
    /// <param name="refill"></param>
    public void SetNewMaxHealth(float newMaxHealth, bool refill)
    {
        maxHealth = newMaxHealth;

        if (refill)
        {
            float amountHealed = newMaxHealth - curHealth;

            curHealth = maxHealth;

            OnHealthChanged?.Invoke(curHealth, GetHealthPercent, amountHealed);
        }
    }
}