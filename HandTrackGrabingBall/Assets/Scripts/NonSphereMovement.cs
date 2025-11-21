using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NonSphereMovement : MonoBehaviour
{
    public float speed = 5f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.forward * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S)) {
            transform.position -= transform.forward * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.A)) {
            transform.position -= transform.right * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D)) {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("MoverArrowCollision")) {
            List<GameObject> movers = GameObject.FindGameObjectsWithTag("MoverCollisionGroup1").ToList();
            movers.AddRange(GameObject.FindGameObjectsWithTag("MoverCollisionGroup2"));
            movers.AddRange(GameObject.FindGameObjectsWithTag("MoverCollisionGroup3"));
            foreach (GameObject mover in movers) {
                if (mover.transform.position.x == other.transform.position.x && mover.transform.position.z == other.transform.position.z) {
                    Domino domino = mover.GetComponent<Domino>();
                    if (domino != null) {
                        domino.TriggerManualDomino();
                    }
                }
            }
        }
    }
}