using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MethFunctions
{
    public class MethFunction
    {
        public float epsilon(float number=0f)
        {
            return number + 0.3f;
        }

        public float piEpsilon(float number=0f)
        {
            return number / (4f * 3.1415f * epsilon(0));
        }
    }
}
