using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Camera FPSCamera;
        [SerializeField] float range = 100f;

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
            Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit, range);
            Debug.Log("Hits " + hit.transform.name);
        }
    }
}
