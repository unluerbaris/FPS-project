using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Core
{
    public class DisplayDamage : MonoBehaviour
    {
        [SerializeField] Canvas damageEffectCanvas;
        [SerializeField] float impactTime = 0.3f;

        void Start()
        {
            damageEffectCanvas.enabled = false;
        }

        public void DisplayDamageImpact()
        {
            StartCoroutine(DisplaySplatter());
        }

        IEnumerator DisplaySplatter()
        {
            damageEffectCanvas.enabled = true;
            yield return new WaitForSeconds(impactTime);
            damageEffectCanvas.enabled = false;
        }
    }
}
