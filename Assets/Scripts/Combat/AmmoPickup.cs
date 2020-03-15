using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Combat
{
    public class AmmoPickup : MonoBehaviour
    {
        [SerializeField] int ammoAmount = 5;
        [SerializeField] AmmoType ammoType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<Ammo>().IncreaseCurrentAmmo(ammoType, ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
