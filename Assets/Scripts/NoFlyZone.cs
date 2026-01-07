using UnityEngine;

public class NoFlyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Search for the Lander script on the object or its parent
        Lander lander = other.GetComponentInParent<Lander>();

        if (lander != null)
        {
            // Direct call to your existing crash logic
            lander.TriggerBulletCrash();
        }
    }
}