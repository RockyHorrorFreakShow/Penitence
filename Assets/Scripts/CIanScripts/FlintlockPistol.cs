using UnityEngine;

public class FlintlockPistol : BaseWeapon
{

    //stop running my script through chat gpt, its entirely different from when I last opened it - Cian
    // All comments are ai too, its so blanant. Stop touching my scripts

    public float shotRange = 50f;
    public float shotDamage = 10f;
    public GameObject impactEffect;
 

    private iDamageable enemy;
    private HealthManager healthManager;

    private bool canFireSecondShot = false;
    private float secondShotTime = 0.1f;
    private bool isCooldownActive = false;

    [Header("Line renderer junk")]
    public GameObject bulletTrailPrefab;

    public override void Fire()
    {
        if (!Input.GetButton("Fire1")) return;

        if (Time.time >= nextFireTime && HasAmmo())
        {
            if (!canFireSecondShot)
            {
                // First shot (only one that works atm)
                Debug.Log("Firing first shot...");
                FireShot();
                canFireSecondShot = true;
                nextFireTime = Time.time + secondShotTime; 
            }
            else
            {
                // Second shot (not workin atm)
                Debug.Log("Firing second shot...");
                FireShot();
                canFireSecondShot = false;
                nextFireTime = Time.time + fireRate; 
            }
        }
    }

    private void FireShot()
    {
        Vector3 rayStart = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;
        Vector3 hitPoint;

        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, shotRange))
        {
            hitPoint = hit.point;

            Debug.Log($"Shot hit {hit.collider.gameObject.name} at {hit.point}");

            iDamageable damageable = hit.collider.GetComponentInParent<iDamageable>();
            if (damageable != null)
            {
                Debug.Log("enemy took damage");
                damageable.TakeDamage(shotDamage);
            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        else
        {
            hitPoint = rayStart + rayDirection * shotRange;
            Debug.Log("Shot missed.");
        }

        // spawn bullet trail
        if (bulletTrailPrefab != null)
        {
            GameObject trailGO = Instantiate(bulletTrailPrefab);
            BulletTrail trail = trailGO.GetComponent<BulletTrail>();
            if (trail != null)
            {
                trail.Init(rayStart, hitPoint);
            }
        }

        WeaponEvents.OnFlintlockFired?.Invoke();
        DecreaseAmmo();
        PlayShootSound();
    }

    private void Update()
    {
        // resets cooldown bwteen shots, adjustable in the inspector DO NOT CHANGE VALUES HERE
        if (Time.time >= nextFireTime)
        {
            isCooldownActive = false;
        }
    }
}
