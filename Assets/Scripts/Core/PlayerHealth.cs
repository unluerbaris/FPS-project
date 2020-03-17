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
            if (playerHealthPoints <= 0) return; //return if player is dead

            playerHealthPoints -= damage; //player takes damage
            

            //debug message shows player's health points
            Debug.Log("Player has " + playerHealthPoints + " health points.");

            //if player dies show Game Over Canvas
            if (playerHealthPoints <= 0)
            {
                GetComponent<GameOverStateManager>().ActivateGameOverCanvas();
            }
        }
    }
}
