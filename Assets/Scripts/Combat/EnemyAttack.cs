using UnityEngine;
using ES.Core;
using ES.Audio;

namespace ES.Combat
{
    public class EnemyAttack : MonoBehaviour
    {
        PlayerHealth target;
        [SerializeField] float damage = 10f;

        // Audio Collections
        [SerializeField] private AudioCollections attackSFX = null;

        void Start()
        {
            target = FindObjectOfType<PlayerHealth>();
        }

        public void AttackHitEvent() // Animation event
        {
            if (target == null) return;
            PlaySFX(attackSFX);
            target.TakeDamage(damage);
            target.GetComponent<DisplayDamage>().DisplayDamageImpact();
        }

        public void PlaySFX(AudioCollections soundFX)
        {
            if (AudioManager.instance != null && soundFX != null)
            {
                AudioClip soundToPlay;
                soundToPlay = soundFX[0];
                AudioManager.instance.PlayOneShotSound("Zombies", soundToPlay, transform.position,
                                                      soundFX.volume, soundFX.spatialBlend, soundFX.priority);
            }
        }
    }
}
