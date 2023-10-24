using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] private InputActionReference moveRef;
    [SerializeField] private InputAction move;
    [SerializeField] float speed;
    [SerializeField] AttackData attackData;
    [SerializeField] float lifetime = 2;

    private void Awake()
    {
        move = moveRef;
    }

    void OnEnable()
    {
        var totalVec = move.ReadValue<Vector2>();

        GetComponent<Rigidbody2D>().AddForce(totalVec * speed, ForceMode2D.Impulse);

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