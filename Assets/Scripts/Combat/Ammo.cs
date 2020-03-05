using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Combat
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] int ammoAmount = 10; 

        public int GetCurrentAmmo()
        {
            return ammoAmount;
        }

        public void ReduceCurrentAmmo()
        {
            ammoAmount--;
        }
    }
}
