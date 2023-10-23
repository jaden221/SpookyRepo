using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] AttackData attackData;

    void OnEnable()
    {
        GetComponent<Rigidbody2D>().AddForce(speed * transform.right, ForceMode2D.Impulse);
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