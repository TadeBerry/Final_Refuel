using UnityEngine;

public class FlyingMeteoriteMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float smoothness = 2f; // Higher = faster turnaround, Lower = driftier
    
    [Header("Local Patrol Settings")]
    [SerializeField] private float horizontalRange = 10f;
    [SerializeField] private float verticalRange = 10f;
    
    private Vector2 currentVelocity;
    private Vector2 targetDirection;
    private Vector3 spawnPosition;
    private bool hasStarted = false;

    void Start()
    {
        spawnPosition = transform.position;
        // Start with a random target direction
        float angle = Random.Range(0f, 360f);
        targetDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        currentVelocity = targetDirection * speed;
    }

    void Update()
    {
        if (!hasStarted)
        {
            // Wait for player movement
            if (Lander.Instance != null && Lander.Instance.GetComponent<Rigidbody2D>().linearVelocity.magnitude > 0.1f)
            {
                hasStarted = true;
            }
            return;
        }

        HandleSmoothPatrol();

        // Apply movement
        transform.position += (Vector3)currentVelocity * Time.deltaTime;
        transform.Rotate(0, 0, 100 * Time.deltaTime);
    }

    void HandleSmoothPatrol()
    {
        float deltaX = transform.position.x - spawnPosition.x;
        float deltaY = transform.position.y - spawnPosition.y;

        // If outside the boundary, update the target direction to point back to center
        if (Mathf.Abs(deltaX) > horizontalRange || Mathf.Abs(deltaY) > verticalRange)
        {
            Vector2 returnDir = (spawnPosition - transform.position).normalized;
            targetDirection = returnDir;
        }

        // Gradually shift current velocity toward the target direction * speed
        // This creates that "slowing down and turning back" effect
        currentVelocity = Vector2.Lerp(currentVelocity, targetDirection * speed, Time.deltaTime * smoothness);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ? spawnPosition : transform.position;
        Gizmos.DrawWireCube(center, new Vector3(horizontalRange * 2, verticalRange * 2, 0));
    }
}