using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES.Core;

namespace ES.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Camera FPSCamera;
        [SerializeField] float range = 100f;
        [SerializeField] float damage = 20f;

        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit, range);
            if (hasHit)
            {
                Debug.Log("Hits " + hit.transform.name);

                EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
                if (target == null) return;

                target.TakeDamage(damage);
            }
            else // if raycast can't hit anything, just return
            {
                return;
            }
        }
    }
}
