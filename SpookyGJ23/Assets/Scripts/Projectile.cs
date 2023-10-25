using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] AttackData attackData;

    [SerializeField] float speed;
    [SerializeField] float lifetime = 2;

    void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * speed, ForceMode2D.Impulse);

        Destroy(gameObject, lifetime);
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