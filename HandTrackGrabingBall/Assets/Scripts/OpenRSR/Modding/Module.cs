using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenRSR {
    namespace Modding {
        public abstract class Module : MonoBehaviour
        {
            public GameManager gameManager;
            void Start() {
                gameManager = GameManager.instance;
                Initialize();
            }

            public abstract void Initialize();

            void Update() {
                if (gameManager == null) {
                    gameManager = GameManager.instance;
                }

                if (gameManager.hasFallen) {
                    OnFallen();
                    gameManager.hasFallen = false;
                }
                if (gameManager.hasHitObstacle) {
                    OnObstacleHit();
                    gameManager.hasHitObstacle = false;
                }

                OnUpdate();
            }

            public abstract void OnUpdate();

            public virtual void OnFallen() {

            }

            public virtual void OnObstacleHit() {

            }
        }
    }
}