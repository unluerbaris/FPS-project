using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Core
{
    public class TargetFPS : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 0;
        }
    }
}
