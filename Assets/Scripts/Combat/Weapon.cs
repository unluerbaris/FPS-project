using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES.Core;
using System;

namespace ES.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Camera FPSCamera;
        [SerializeField] float range = 100f;
        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float timeBetweenShots = 0.8f;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] GameObject hitEffect;
        [SerializeField] Ammo ammoSlot;
        [SerializeField] AmmoType ammoType;

        bool canShoot = true;

        private void OnEnable()
        {
            canShoot = true;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && canShoot == true)
            {
                StartCoroutine(Shoot());
            }
        }
       
        IEnumerator Shoot()
        {
            if (ammoSlot.GetCurrentAmmo(ammoType) <= 0) yield break;

            canShoot = false;

            UseAmmo();
            PlayMuzzleFlash();
            ProcessRaycast();

            yield return new WaitForSeconds(timeBetweenShots);
            canShoot = true;
        }

        private void UseAmmo()
        {
            ammoSlot.ReduceCurrentAmmo(ammoType);
            Debug.Log(ammoSlot.GetCurrentAmmo(ammoType));
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
    }
}
