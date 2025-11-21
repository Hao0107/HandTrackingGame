using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OpenRSR {
    namespace Gameplay {
        public class Ball : MonoBehaviour {
            public float speed = 6.0f;
            public Rigidbody rb;
            public SphereMovement sphm;
            public SphereDragger controls;

            void Start() {
                rb = GetComponent<Rigidbody>();
                sphm = GetComponent<SphereMovement>();
                controls = GetComponent<SphereDragger>();
                if (sphm != null) {
                speed = sphm.speed;
                }
            }

            public void Jump(float distance, Vector3 startPosition) {
                sphm.Jump(distance, startPosition);
            }

            public void DisableControls() {
                controls.enabled = false;
            }

            public void EnableControls() {
                controls.enabled = true;
            }

            public void SetMaxOffset(float maxOffset) {
                controls.maxOffset = maxOffset;
            }
        }
    }
}