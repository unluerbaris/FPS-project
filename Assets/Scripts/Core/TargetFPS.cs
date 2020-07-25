using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ES.Core
{
    public class TargetFPS : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
        }
    }
}
