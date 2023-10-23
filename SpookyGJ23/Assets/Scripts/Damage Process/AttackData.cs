using System.Diagnostics;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    [SerializeField] public DamageData dmgStruct;
    [SerializeField] Sound soundData;

    public Sound GetSoundData
    {
        get
        {
            return soundData;
        }
    }
}