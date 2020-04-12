using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES.Control;

namespace ES.Animations
{
    public class DoorAnimation : MonoBehaviour
    {
        float distanceFromPlayer;
        GameObject target;

        private void Update()
        {
            distanceFromPlayer = FPSController.distanceFromTarget;
            target = FPSController.targetObject;

            if (this.gameObject == target && Input.GetKeyDown(KeyCode.E) && distanceFromPlayer < 2f)
            {
                Animation doorAnimation = target.GetComponent<Animation>();
                doorAnimation.Play();
            }
        }
    }
}