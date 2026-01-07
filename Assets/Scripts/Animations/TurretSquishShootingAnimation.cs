using UnityEngine;

public class TurretSquishShootingAnimation : MonoBehaviour
{
    private Transform headTransform;
    private Turret turret;
    private Vector3 originalScale;
    
    // Hardcoded Animation Settings
    private float squishSpeed = 1.5f;      // Very slow "charging" squish
    private float restoreSpeed = 40f;     // Extremely fast "snap" release
    private float anticipationTime = 0.8f; // Start squishing earlier for a better build-up
    private Vector3 maxSquish = new Vector3(0.6f, 1.4f, 1f);

    private bool isRestoring = false;

    private void Awake()
    {
        turret = GetComponent<Turret>();
        headTransform = transform.Find("Head");

        if (headTransform != null)
        {
            originalScale = headTransform.localScale;
        }
    }

    private void Start()
    {
        if (turret != null)
        {
            turret.OnShoot += Turret_OnShoot;
        }
    }

    private void Update()
    {
        if (headTransform == null || turret == null) return;

        // NEW: Wait until the game starts (Lander gravity active) before doing any animation logic
        if (Lander.Instance == null || Lander.Instance.GetComponent<Rigidbody2D>().gravityScale == 0)
        {
            headTransform.localScale = originalScale;
            return;
        }

        if (isRestoring)
        {
            headTransform.localScale = Vector3.Lerp(headTransform.localScale, originalScale, Time.deltaTime * restoreSpeed);
            
            if (Vector3.Distance(headTransform.localScale, originalScale) < 0.001f)
            {
                headTransform.localScale = originalScale;
                isRestoring = false;
            }
        }
        else
        {
            float currentTimer = GetTurretTimer();

            if (currentTimer <= anticipationTime)
            {
                headTransform.localScale = Vector3.Lerp(headTransform.localScale, maxSquish, Time.deltaTime * squishSpeed);
            }
        }
    }

    private void Turret_OnShoot(object sender, System.EventArgs e)
    {
        isRestoring = true;
    }

    private float GetTurretTimer()
    {
        var field = typeof(Turret).GetField("shootTimer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null ? (float)field.GetValue(turret) : 100f;
    }

    private void OnDestroy()
    {
        if (turret != null) turret.OnShoot -= Turret_OnShoot;
    }
}