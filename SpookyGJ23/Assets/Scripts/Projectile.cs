using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    #region Variables

    [Header("Struct")]
    [SerializeField] AttackData attackData;

    [Header("Input")]
    [SerializeField] private InputActionReference moveRef;
    [SerializeField] private InputAction move;

    [Header("Customisable")]
    [SerializeField] float speed;
    [SerializeField] float lifetime = 2;

    #endregion

    #region Processes

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

    #region Triggers

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out DamageReceiver damageReceiver))
        {
            damageReceiver.ReceiveDamage(attackData);
        }

        if (other.gameObject.CompareTag("Monster")) 
        {
            other.gameObject.GetComponent<Health>().AddHealth(-50);
        }

        Destroy(gameObject);
    }

    #endregion

    #endregion
}