using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damage;

    public float staggerDamage;

    public bool canHitstop;

    [HideInInspector]
    public float damageDealt;

    public float TotalDamage() 
    { 
        return damage;
    }
}