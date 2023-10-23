using UnityEngine;

public class DealDamageMelee : MonoBehaviour
{
    IAttack iAttack;
    AttackData attackData;

    Transform upperTransform;

    void Awake()
    {
        iAttack = GetComponentInParent<IAttack>();

        upperTransform = GetComponentInParent<Attacks>().transform;
    }

    void OnEnable()
    {
        iAttack.OnStartAttack += HandleStartAttack;
    }

    void OnDisable()
    {
        iAttack.OnStartAttack -= HandleStartAttack;
    }

    public void HandleStartAttack(AttackData data)
    {
        attackData = data;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (iAttack.Contains(other.transform.root)) return;
        if (transform.root == other.transform.root) return;
        if (!other.transform.root.TryGetComponent(out DamageReceiver damageReceiver)) return;

        iAttack.Add(damageReceiver.transform);

        damageReceiver.ReceiveDamage(attackData);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (iAttack.Contains(other.transform.root)) return;
        if (transform.root == other.transform.root) return;
        if (!other.transform.root.TryGetComponent(out DamageReceiver damageReceiver)) return;

        iAttack.Add(damageReceiver.transform);

        damageReceiver.ReceiveDamage(attackData);
    }
}