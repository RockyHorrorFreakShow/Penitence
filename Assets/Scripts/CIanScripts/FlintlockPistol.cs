using UnityEngine;

public class FlintlockPistol : BaseWeapon
{

    //stop running my script through chat gpt, its entirely different from when I last opened it - Cian
    // All comments are ai too, its so blanant. Stop touching my scripts

    public float shotRange = 50f;
    public float shotDamage = 10f;
    public GameObject impactEffect;
    public LineRenderer lineRenderer; 
    private iDamageable enemy;
    private HealthManager healthManager;

    private bool canFireSecondShot = false;
    private float secondShotTime = 0.1f;
    private bool isCooldownActive = false;

    public override void Fire()
    {
        if (Time.time >= nextFireTime && HasAmmo()) //check for ammo b4 firing
        {
            // Fire the first shot only if enough time has passed since the last shot
            Debug.Log("Firing first shot...");
            FireShot();

            // Fire the second shot after a short delay
            canFireSecondShot = true;
            nextFireTime = Time.time + fireRate;
        }
        else if (canFireSecondShot && Time.time >= nextFireTime)
        {
            Debug.Log("Firing second shot...");
            FireShot();

            canFireSecondShot = false;
            nextFireTime = Time.time + secondShotTime; // cooldown for 2nd shot
        }
        else if (!isCooldownActive)
        {
            Debug.Log("Can't fire yet. Cooldown in progress.");
            isCooldownActive = true; 
        }
    }

    private void FireShot()
    {
        Vector3 rayStart = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, rayStart);

        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, shotRange))
        {
            lineRenderer.SetPosition(1, hit.point);

            Debug.Log($"Shot hit {hit.collider.gameObject.name} at {hit.point}");

            
            iDamageable damageable = hit.collider.GetComponentInParent<iDamageable>();


            if (damageable != null)
            {
                Debug.Log("Enemy detected! Applying damage...");
                damageable.TakeDamage(shotDamage); 
            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        else
        {
            lineRenderer.SetPosition(1, rayStart + rayDirection * shotRange);
            Debug.Log("Shot missed.");
        }

        DecreaseAmmo();
        PlayShootSound();
        StartCoroutine(DisableLineRenderer());
    }


    private System.Collections.IEnumerator DisableLineRenderer()
    {
        yield return new WaitForSeconds(2f);
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // resets cooldown
        if (Time.time >= nextFireTime)
        {
            isCooldownActive = false;
        }
    }
}
