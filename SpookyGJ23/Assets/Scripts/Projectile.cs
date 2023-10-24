using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AttackData attackData;

    void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.forward * speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out DamageReceiver damageReceiver))
        {
            damageReceiver.ReceiveDamage(attackData);
        }

        Destroy(gameObject);
    }
}