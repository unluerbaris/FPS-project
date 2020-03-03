using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Core
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] float playerHealthPoints = 200f;

        public void TakeDamage(float damage)
        {
            if (playerHealthPoints <= 0)
            {
                Debug.Log("You are dead");
                return;
            }
            playerHealthPoints -= damage;
            Debug.Log("Player has " + playerHealthPoints + " health points.");
        }
    }
}
