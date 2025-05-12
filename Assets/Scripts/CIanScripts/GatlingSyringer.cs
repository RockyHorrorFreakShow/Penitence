using UnityEngine;

public class GatlingSyringer : BaseWeapon
{
    public float syringeRange = 50f;
    public float syringeDamage = 10f;
    public GameObject impactEffect;
    [Header("line renderer junk 3: Return of the Jedi")]
    public GameObject bulletTrailPrefab;

    private bool isCooldownActive = false;

    public override void Fire()
    {
        if (Time.time >= nextFireTime && HasAmmo()) // checks 4 cooldown
        {
            FireShot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FireShot()
    {
        Vector3 rayStart = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;
        Vector3 rayEnd;

        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, syringeRange))
        {
            rayEnd = hit.point;

            Debug.Log($"Shot hit {hit.collider.gameObject.name} at {hit.point}");

            iDamageable damageable = hit.collider.GetComponentInParent<iDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(syringeDamage);
            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        else
        {
            rayEnd = rayStart + rayDirection * syringeRange;
        }

        if (bulletTrailPrefab != null)
        {
            GameObject trailGO = Instantiate(bulletTrailPrefab);
            trailGO.transform.parent = null; //kills the bullet trail even after the weapons unselected to prevent a bug that stopped them from despawning
            BulletTrail trail = trailGO.GetComponent<BulletTrail>();
            if (trail != null)
            {
                trail.Init(rayStart, rayEnd);
            }
        }

        DecreaseAmmo();
        PlayShootSound();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && HasAmmo())
        {
            Fire();
        }

        //backup to ensure cooldown is reset
        if (Time.time >= nextFireTime)
        {
            isCooldownActive = false;
        }
    }
}
