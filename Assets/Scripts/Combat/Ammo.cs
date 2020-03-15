using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Combat
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] AmmoSlot[] ammoSlots;

        [System.Serializable]
        private class AmmoSlot
        {
            public AmmoType ammoType;
            public int ammoAmount;
        }

        public int GetCurrentAmmo(AmmoType ammoType) //call this method in Weapon.cs
        {                                            // to get ammoType and ammoAmounts
            return GetAmmoSlot(ammoType).ammoAmount;
        }

        public void ReduceCurrentAmmo(AmmoType ammoType)
        {
            GetAmmoSlot(ammoType).ammoAmount--;
        }

        public void IncreaseCurrentAmmo(AmmoType ammoType, int pickupAmmoAmount)
        {
            GetAmmoSlot(ammoType).ammoAmount += pickupAmmoAmount;
        }

        private AmmoSlot GetAmmoSlot(AmmoType ammoType) // it loops through the AmmoSlot array
        {                                               // and gets the correct ammoSlot
            foreach (AmmoSlot slot in ammoSlots)        // which has the similar ammoType with selected weapon.
            {
                if (slot.ammoType == ammoType)
                {
                    return slot;
                }
            }
            return null;
        }
    }
}
