using System.Collections;
using UnityEngine;
using ES.Core;
using TMPro;
using ES.Audio;

namespace ES.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Camera FPSCamera;
        [SerializeField] float range = 100f;
        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float timeBetweenShots = 0.8f;
        [SerializeField] float newWeaponActivationTime = 1f;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] GameObject hitEffect;
        [SerializeField] Ammo ammoSlot;
        [SerializeField] AmmoType ammoType;
        [SerializeField] TextMeshProUGUI ammoValueText;

        // Audio Collections
        [SerializeField] private AudioCollections shotSFX = null;
        [SerializeField] private AudioCollections emptySFX = null;

        bool canShoot = false;

        private void Start()
        {
            UpdateAmmoDisplay();
        }

        private void OnEnable()
        {
            // Make the weapon ready to shoot after script is enabled
            StartCoroutine(ActivateWeapon(newWeaponActivationTime));
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && canShoot == true)
            {
                StartCoroutine(Shoot());
            }
        }

        IEnumerator ActivateWeapon(float activationDelay)
        {
            yield return new WaitForSeconds(activationDelay);
            canShoot = true;
        }
       
        IEnumerator Shoot()
        {
            if (ammoSlot.GetCurrentAmmo(ammoType) <= 0)
            {
                PlaySFX(emptySFX);
                yield break;
            }
            canShoot = false;

            PlaySFX(shotSFX);

            UseAmmo();
            PlayMuzzleFlash();
            ProcessRaycast();

            yield return new WaitForSeconds(timeBetweenShots);
            canShoot = true;
        }

        private void UseAmmo()
        {
            ammoSlot.ReduceCurrentAmmo(ammoType);
            UpdateAmmoDisplay();
        }

        public void UpdateAmmoDisplay()
        {
            ammoValueText.text = ammoSlot.GetCurrentAmmo(ammoType).ToString();
        }

        private void PlayMuzzleFlash()
        {
            muzzleFlash.Play();
        }

        private void ProcessRaycast()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit, range);
            if (hasHit)
            {
                CreateHitImpact(hit);
                EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                if (target == null) return;

                target.TakeDamage(weaponDamage);
            }
            else // if raycast can't hit anything, just return
            {
                return;
            }
        }

        private void CreateHitImpact(RaycastHit hit)
        {
            GameObject hitEffectInstance = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffectInstance, 0.1f);
        }

        public void PlaySFX(AudioCollections soundFX)
        {
            if (AudioManager.instance != null && soundFX != null)
            {
                AudioClip soundToPlay;
                soundToPlay = soundFX[0];
                AudioManager.instance.PlayOneShotSound("Player", soundToPlay, transform.position,
                                                      soundFX.volume, soundFX.spatialBlend, soundFX.priority);
            }
        }
    }
}
