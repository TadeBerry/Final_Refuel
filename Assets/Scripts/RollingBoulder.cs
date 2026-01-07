using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasStarted = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        if (hasStarted) return;

        if (Lander.Instance != null && Lander.Instance.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0.1f)
        {
            hasStarted = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}