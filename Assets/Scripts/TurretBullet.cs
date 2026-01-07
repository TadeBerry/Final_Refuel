using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private float speed = 15f;
    private float lifeTimer = 7f;

    private void Start()
    {

    }

    private void Update()
    {
        if (Lander.Instance != null && Lander.Instance.GetState() != Lander.State.Normal) 
        {
            return; 
        }

        transform.position += -transform.right * speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Lander.Instance != null && collision.gameObject == Lander.Instance.gameObject)
        {
            if (Lander.Instance.GetState() == Lander.State.Normal)
            {
                Lander.Instance.TriggerBulletCrash();
                Destroy(gameObject);
            }
            else 
            {
                Destroy(gameObject);
            }
        }
    }
}