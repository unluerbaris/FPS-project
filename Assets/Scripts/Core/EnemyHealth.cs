using UnityEngine;
using ES.Audio;

namespace ES.Core
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] float hitPoints = 100f;
        bool isDead = false;

        // Audio Collections
        [SerializeField] private AudioCollections hurtSFX = null;
        [SerializeField] private AudioCollections dieSFX = null;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            if (isDead) return;

            PlaySFX(hurtSFX);
            BroadcastMessage("OnDamageTaken"); // Call OnDamageTaken to provoke the enemy

            hitPoints -= damage;
            if(hitPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            PlaySFX(dieSFX);
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
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
