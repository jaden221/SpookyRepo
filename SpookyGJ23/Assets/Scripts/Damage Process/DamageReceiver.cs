using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    DamageDataReceived dmgData = new DamageDataReceived();

    [Tooltip("Add Dynamic HandleDamageReceived functions here")]
    public UnityEvent<DamageDataReceived> OnDamageReceived;

    public void ReceiveDamage(AttackData data)
    {
        dmgData.dmgdata = data.dmgStruct;
    }
}

public class DamageDataReceived
{
    public DamageData dmgdata;
}