using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AbstractWeaponController : MonoBehaviour
{
    // Ammo information
    [SerializeField] protected int startingAmmo;
    [SerializeField] protected int ammoClipSize;
    public int AmmoInGun { get; protected set; }
    public int AmmoRemaining { get; protected set; }
    /// <summary>
    /// The amount of ammunition in the gun.
    /// </summary>
    public int Ammo
    {
        get { return AmmoInGun + AmmoRemaining; }
        protected set
        {
            AmmoInGun = value % ammoClipSize;
            AmmoRemaining = value - AmmoInGun;
        }
    }
    public bool GunReloaded
    {
        get { return AmmoInGun > 0; }
    }
    /// <summary>
    /// The size of the gun's ammo clip.
    /// Used for reloading and score displays
    /// </summary>
    /// <value>The new size of the ammo clip (i.e. you picked up a high-capacity clip).</value>
    public int AmmoClipSize
    {
        get { return ammoClipSize; }
        protected set { ammoClipSize = value; }
    }
    public bool IsReloading { get; protected set; }

    // Control vars
    [SerializeField] protected float damage;
    [SerializeField] protected float range;
    [SerializeField] protected float impactForce;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float reloadTime;

    // Effects
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected ParticleSystem mazeImpactEffect;
    [SerializeField] protected ParticleSystem nonMazeImpactEffect;

    // Audio
    [SerializeField] protected AudioClip fireSound;
    [SerializeField] protected AudioClip reloadSound;
    AudioSource gunAudio;

    // Bookeeping
    protected float nextTimeToFire = 0f;
    System.Func<int> lastReloadCallback;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Ammo = startingAmmo;
        Debug.Log(Ammo);

        gunAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if (IsReloading)
        {
            // Switched weapons in the middle of reloading,
            // restart the reload coroutine
            StartCoroutine(ReloadWithDelay(lastReloadCallback));
        }
    }

    /// <summary>
    /// Shoot a bullet with this gun.
    /// </summary>
    /// <param name="fireDirection">The direction the bullet was fired.</param>
    /// <param name="fireStartPoint">The start point of the firing. Usually just `transform.position`.</param>
    /// <returns><see langword="true"/> if the bullet hit an enemy and <see langword="false"/> otherwise.</returns>
    public virtual bool Shoot(Vector3 fireDirection, Vector3 fireStartPoint)
    {
        Debug.Log("SHOOT!");
        fireDirection *= range;

        if (Time.time < nextTimeToFire || !GunReloaded || IsReloading || Ammo <= 0)
        {
            // Still on fire cooldown or not reloaded (yet) or out of ammo
            return false;
        }

        AmmoInGun--;
        nextTimeToFire = Time.time + 1f / fireRate;

        muzzleFlash.Play();
        gunAudio.PlayOneShot(fireSound);

        if (Physics.Raycast(fireStartPoint, fireDirection, out RaycastHit hit, range))
        {
            Debug.Log("HIT");
            // Hit something
            ShotTarget target = hit.transform.GetComponent<ShotTarget>();

            // Take damage and spawn impact effects
            if (target != null)
            {
                // Hit an enemy or powerup
                target.TakeDamage(damage);
                Instantiate(nonMazeImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                return true;
            }
            else
            {
                if (hit.transform.CompareTag("Ground")
                    || hit.transform.CompareTag("MazeWall")
                    || hit.transform.CompareTag("Platform"))
                {
                    // Hit the maze
                    Instantiate(mazeImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }

            // Add force to hit rigidbody
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            // Draw the fire ray
            Debug.DrawRay(fireStartPoint, fireDirection, Color.red);
        }
        // Didn't hit anything
        Debug.Log("MISS");
        return false;
    }

    public virtual void AddAmmo(int amount)
    {
        AmmoRemaining += amount;
    }

    public virtual void Reload(System.Func<int> callback)
    {
        if (Time.time < nextTimeToFire || AmmoRemaining <= 0 || IsReloading)
        {
            // Still on fire cooldown or out of ammo to reload or already reloading
            return;
        }
        nextTimeToFire = Time.time + 1f / fireRate;

        lastReloadCallback = callback;
        StartCoroutine(ReloadWithDelay(callback));
    }

    protected virtual IEnumerator ReloadWithDelay(System.Func<int> callback)
    {
        IsReloading = true;
        gunAudio.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);

        int ammoToMove = AmmoClipSize - AmmoInGun;
        if (ammoToMove < AmmoRemaining)
        {
            AmmoInGun = AmmoClipSize;
            AmmoRemaining -= ammoToMove;
        }
        else
        {
            AmmoInGun += AmmoRemaining;
            AmmoRemaining = 0;
        }
        IsReloading = false;
        callback();
    }
}
