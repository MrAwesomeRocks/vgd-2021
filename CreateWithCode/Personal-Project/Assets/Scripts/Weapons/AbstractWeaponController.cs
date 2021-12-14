using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An <see langword="abstract"/> class for managing weapons.
/// Most weapons can simply subclass this without any overrides.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class AbstractWeaponController : MonoBehaviour {
    // Ammo information
    [SerializeField] protected int startingAmmo;
    [SerializeField] protected int ammoClipSize;
    /// <summary>
    /// The amount of ammo in the gun's clip.
    /// </summary>
    public int AmmoInGun { get; protected set; }
    /// <summary>
    /// The amount of ammo not in the gun.
    /// </summary>
    public int AmmoRemaining { get; protected set; }
    /// <summary>
    /// The total amount of ammunition in the gun.
    /// This is a computed property from the ammo in the clip and the remaining ammo.
    /// Assigning to Ammo puts as much ammo into the remaining as possible and the minimum into the clip.
    /// </summary>
    public int Ammo
    {
        get { return AmmoInGun + AmmoRemaining; }
        protected set {
            AmmoInGun = value % ammoClipSize;
            AmmoRemaining = value - AmmoInGun;
        }
    }
    /// <summary>
    /// A computed property that simply checks if the ammo in the gun is greater than 0.
    /// </summary>
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
    /// <summary>
    /// A bool that is set when the gun is reloading to make sure that the gun isn't fired.
    /// </summary>
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
    System.Action lastReloadCallback;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        Ammo = startingAmmo;

        gunAudio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable() {
        if (IsReloading) {
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
    public virtual bool Shoot(Vector3 fireDirection, Vector3 fireStartPoint) {
        // Extend the fire direction for drawing the debug ray.
        fireDirection *= range;
        Debug.Log("SHOOT!");

        if (Time.time < nextTimeToFire || !GunReloaded || IsReloading || Ammo <= 0) {
            // Still on fire cooldown or not reloaded (yet) or out of ammo
            return false;
        }

        // Decrease the ammo in the gun and reset the fire cooldown
        AmmoInGun--;
        nextTimeToFire = Time.time + 1f / fireRate;

        // Play effects
        muzzleFlash.Play();
        gunAudio.PlayOneShot(fireSound);

        // Check if the gun hit anything
        if (Physics.Raycast(fireStartPoint, fireDirection, out RaycastHit hit, range)) {
            // Hit something
            Debug.Log("HIT");
            ShotTarget target = hit.transform.GetComponent<ShotTarget>();

            // Take damage and spawn impact effects
            if (target != null) {
                // Hit an enemy or powerup
                target.TakeDamage(damage);
                Instantiate(nonMazeImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                return true;
            } else {
                if (
                  hit.transform.CompareTag("Ground") ||
                  hit.transform.CompareTag("MazeWall") ||
                  hit.transform.CompareTag("Platform")
                ) {
                    // Hit the maze
                    Instantiate(mazeImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                }
                // Didn't hit anything that needs an effect, ignore
            }

            // Add force to hit rigidbody
            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            // Draw the fire ray
            Debug.DrawRay(fireStartPoint, fireDirection, Color.red);
        }
        // Didn't hit anything
        Debug.Log("MISS");
        return false;
    }

    /// <summary>
    /// Add ammo to this weapon.
    /// </summary>
    /// <param name="amount">How much ammo to add.</param>
    public virtual void AddAmmo(int amount) {
        AmmoRemaining += amount;
    }

    /// <summary>
    /// Reload the weapon.
    /// </summary>
    public void Reload() {
        Reload(() => { });
    }

    /// <summary>
    /// Reload the weapon with a <paramref name="callback"/> to run when finished.
    /// </summary>
    /// <param name="callback">A callback to run after the reload is finished. Useful for updating UI.</param>
    public virtual void Reload(System.Action callback) {
        if (Time.time < nextTimeToFire || AmmoRemaining <= 0 || IsReloading) {
            // Still on fire cooldown or out of ammo to reload or already reloading
            return;
        }
        // Restart cooldown
        nextTimeToFire = Time.time + 1f / fireRate;

        // Save callback in case weapons are switched while reloading, then start reload
        lastReloadCallback = callback;
        StartCoroutine(ReloadWithDelay(callback));
    }

    /// <summary>
    /// Reload the weapon after a certain delay.
    /// Used internally by Reload.
    /// </summary>
    /// <param name="callback">The callback to run after the reload is finished.</param>
    /// <returns>A coroutine.</returns>
    protected virtual IEnumerator ReloadWithDelay(System.Action callback) {
        // Set the reloading flag to prevent firing
        IsReloading = true;

        // Play effects
        gunAudio.PlayOneShot(reloadSound);

        // Wait for the reload time
        yield return new WaitForSeconds(reloadTime);

        // Reload the gun
        int ammoToMove = AmmoClipSize - AmmoInGun;
        if (ammoToMove < AmmoRemaining) {
            // Enough ammo for a full reload, fill clip and deduct from the remaining ammo
            AmmoInGun = AmmoClipSize;
            AmmoRemaining -= ammoToMove;
        } else {
            // Not enough ammo, fill clip as much as possible and empty the remaining ammo
            AmmoInGun += AmmoRemaining;
            AmmoRemaining = 0;
        }

        // Reset reloading flag
        IsReloading = false;

        // Call the callback
        callback();
    }
}
