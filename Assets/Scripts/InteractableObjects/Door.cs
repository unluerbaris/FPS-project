using UnityEngine;
using ES.Audio;

namespace ES.InteractableObjects
{
    public class Door : MonoBehaviour
    {
        [SerializeField] AudioCollections doorOpenSFX;
        public bool isAnimationPlayable = true;
        Animation doorAnimation;

        private void Start()
        {
            doorAnimation = gameObject.GetComponent<Animation>();
        }

        private void Update()
        {
            if (doorAnimation.isPlaying && isAnimationPlayable)
            {
                isAnimationPlayable = false;
                PlaySFX(doorOpenSFX);
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