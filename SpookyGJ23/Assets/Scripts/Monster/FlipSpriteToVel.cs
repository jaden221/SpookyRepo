using UnityEngine;

public class FlipSpriteToVel : MonoBehaviour
{
    Rigidbody2D rigidbody; 
    
    void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rigidbody.velocity.x > 0) 
        {
            transform.eulerAngles = Vector3.zero;
        }
        else 
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
