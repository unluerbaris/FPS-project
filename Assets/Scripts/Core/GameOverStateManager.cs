using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Core
{
    public class GameOverStateManager : MonoBehaviour
    {
        [SerializeField] Canvas gameOverCanvas;

        private void Start()
        {
            gameOverCanvas.enabled = false;
        }

        public void ActivateGameOverCanvas() // activate go canvas when player dies
        {
            gameOverCanvas.enabled = true;

            Time.timeScale = 0; // this stops the game

            // unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }
}
