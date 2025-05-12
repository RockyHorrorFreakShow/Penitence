using UnityEngine;
using System.Collections;

public class SpreadCrossbow : BaseWeapon
{
    public int bolts = 5;
    public float shotRange = 50f;
    public float shotDamage = 10f;
    public GameObject impactEffect;


    [Header("Line renderer junk 2: Electric Boogaloo")]
    public GameObject bulletTrailPrefab; // UNIVERSAL BETWEEN WEAPONS !!!
    public float spreadMultiplier = 0.2f;

    public override void Fire()
    {
        if (Time.time >= nextFireTime && HasAmmo())
        {
            for (int i = 0; i < bolts; i++)
            {
                Vector3 spread = playerCamera.transform.right * Random.Range(-spreadMultiplier, spreadMultiplier) +
                                 playerCamera.transform.up * Random.Range(-spreadMultiplier, spreadMultiplier);

                Vector3 rayStart = playerCamera.transform.position;
                Vector3 rayDirection = (playerCamera.transform.forward + spread).normalized;
                Vector3 hitPoint;

                if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, shotRange))
                {
                    iDamageable damageable = hit.collider.GetComponentInParent<iDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(shotDamage);
                    }

                    if (impactEffect != null)
                    {
                        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    }

                    hitPoint = hit.point;
                }
                else
                {
                    hitPoint = rayStart + rayDirection * shotRange;
                }

                // new bullet trail, its the same one as the flintlockm so modify the same matierial and itll change this too
                if (bulletTrailPrefab != null)
                {
                    GameObject trailGO = Instantiate(bulletTrailPrefab);
                    BulletTrail trail = trailGO.GetComponent<BulletTrail>();
                    if (trail != null)
                    {
                        trail.Init(rayStart, hitPoint);
                    }
                }
            }

            nextFireTime = Time.time + fireRate;
            PlayShootSound();
        }
    }
}
