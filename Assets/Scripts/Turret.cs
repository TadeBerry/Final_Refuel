using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public event EventHandler OnShoot;

    [SerializeField] private Transform headTransform;
    [SerializeField] public TurretBullet turretBulletPrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private float shootTimerMax = 2f;
    [SerializeField] private float range = 20f;
    private float shootTimer;

    
    //turret rotation speed
    private float rotationSpeed = 90f;

    private void Start()
    {
        shootTimer = shootTimerMax;
    }

    private void Update()
{
    if (Lander.Instance == null) return;

    //run only if flying
    //stops turret if land or crash
    if (Lander.Instance.GetState() != Lander.State.Normal) return;

    Transform target = Lander.Instance.transform;
    float distanceToTarget = Vector3.Distance(transform.position, target.position);

    if (distanceToTarget <= range)
    {
        bool isPointingAtTarget = LookAtTarget(target);

        if (isPointingAtTarget)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                shootTimer = shootTimerMax;
                Shoot();
            }
        }
    }
}

    private bool LookAtTarget(Transform target)
    {
        Vector3 vecToTarget = target.position - headTransform.position;
        float targetAngle = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
        
        //180 sprite offset
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle + 180f);

        //gradually rotate
        headTransform.rotation = Quaternion.RotateTowards(
            headTransform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );

        //return true if...
        return Quaternion.Angle(headTransform.rotation, targetRotation) < 5f;
    }

    private void Shoot()
    {
        if (turretBulletPrefab == null) return;
        Instantiate(turretBulletPrefab, shootPointTransform.position, shootPointTransform.rotation);
        OnShoot?.Invoke(this, EventArgs.Empty);
    }

    public Transform GetShootPointTransform()
    {
        return shootPointTransform;
    }
}