using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES.Control;
using ES.Audio;

namespace ES.Animations
{
    public class DoorAnimation : MonoBehaviour
    {
        [SerializeField] AudioCollections doorOpenSFX;
        float distanceFromPlayer;
        bool isOpen = false;
        GameObject target;

        private void Update()
        {
            distanceFromPlayer = FPSController.distanceFromTarget;
            target = FPSController.targetObject;

            if (this.gameObject == target && Input.GetKeyDown(KeyCode.E) && distanceFromPlayer < 2f && !isOpen)
            {
                isOpen = true;
                Animation doorAnimation = target.GetComponent<Animation>();
                PlaySFX(doorOpenSFX);
                doorAnimation.Play();
            }
        }

        public void PlaySFX(AudioCollections soundFX)
        {
            if (AudioManager.instance != null && soundFX != null)
            {
                AudioClip soundToPlay;
                soundToPlay = soundFX[0];
                AudioManager.instance.PlayOneShotSound("Scene", soundToPlay, transform.position,
                                                      soundFX.volume, soundFX.spatialBlend, soundFX.priority);
            }
        }
    }
}