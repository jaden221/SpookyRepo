using System;
using UnityEngine.Events;
using UnityEngine;
using UnityEditor.Playables;

public class Attacks : MonoBehaviour, IAttack
{
    //Intended to hold all melee attack data, which the AE events call

    [SerializeField] Attack[] attackData;

    [Space(5)]
    [SerializeField] UnityEvent<AttackData> OnApplyToDamageData;

    public event Action<AttackData> OnStartAttack;
    public event Action OnEndAttack;

    Transform[] hitEnemies = new Transform[20];

    [Serializable]
    public class Attack
    {
        //name for animator
        [SerializeField] string attackName;
        public string GetAtkName
        {
            get
            {
                return attackName;
            }
        }

        //DamageData
        [SerializeField] AttackData damageData;
        public AttackData GetAttackData
        {
            get
            {
                return damageData;
            }
        }
    }

    public void AE_StartAttack(string atkName)
    {
        Utility.ArrayClear(ref hitEnemies);

        int index = -1;

        //find attackData based on string
        for (int i = 0; i < attackData.Length; i++)
        {
            if (attackData[i].GetAtkName == atkName)
            {
                index = i;
                break;
            }
        }

        //if nothing matches the string, log error
        if (index == -1)
        {
            Debug.LogError("AE_StartAttack string passed is either null or incorrect.");
            return;
        }

        //Apply stats to damage data
        OnApplyToDamageData?.Invoke(attackData[index].GetAttackData);

        //Call Start attack event
        OnStartAttack?.Invoke(attackData[index].GetAttackData);
    }

    public void AE_EndAttack()
    {
        OnEndAttack?.Invoke();
    }

    public AttackData GetAttackData(string atkName)
    {
        int index = -1;

        //find attackData based on string
        for (int i = 0; i < attackData.Length; i++)
        {
            if (attackData[i].GetAtkName == atkName)
            {
                index = i;
                break;
            }
        }

        //if nothing matches the string, log error
        if (index == -1) Debug.LogError("AE_StartAttack string passed is either null or incorrect.");

        //Apply stats to damage data
        OnApplyToDamageData?.Invoke(attackData[index].GetAttackData);

        return attackData[index].GetAttackData;
    }

    public bool Contains(Transform hitTrans)
    {
        return Utility.ArrayContains(hitTrans, ref hitEnemies);
    }

    public void Add(Transform hitTrans)
    {
        Utility.ArrayAdd(ref hitEnemies, hitTrans);
    }
}

public interface IAttack
{
    public event Action<AttackData> OnStartAttack;
    public event Action OnEndAttack;

    public bool Contains(Transform hitTrans);
    public void Add(Transform hitTrans);
}