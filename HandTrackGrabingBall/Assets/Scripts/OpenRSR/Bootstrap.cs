using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OpenRSR
{
    public class Bootstrap : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}