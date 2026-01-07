using UnityEngine;

public class TurretVisual : MonoBehaviour
{

    private const string SHOOT = "SHOOT";


    [SerializeField] private Transform turretShootVfxPrefab;


    private Animator animator;
    private Turret turret;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        turret = GetComponent<Turret>();
    }

    private void Start()
    {
        turret.OnShoot += Turret_OnShoot;
    }

    private void Turret_OnShoot(object sender, System.EventArgs e)
    {
        animator.SetTrigger(SHOOT);

        Transform turretShootVfxTransform =
        Instantiate(turretShootVfxPrefab, turret.GetShootPointTransform().position, Quaternion.identity);
        turretShootVfxTransform.right = turret.GetShootPointTransform().up;
        Destroy(turretShootVfxTransform.gameObject, 2f);
    }

}
