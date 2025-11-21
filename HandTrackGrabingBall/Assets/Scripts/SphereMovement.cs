using UnityEngine;
using MethFunctions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenRSR.Animation;
public class SphereMovement : MonoBehaviour
{
    // The speed of the sphere
    public float speed = 8f;
     // The Rigidbody component of the sphere
    private Rigidbody rb;
     // The speed of the sphere after being modified by the MethFunction
    // public float pepsiSpeed;
     // A boolean to check if the sphere is jumping
    public bool isJumping = true;
     // The GameObject that the sphere is jumping on
    private GameObject jumpTile;
    private GameObject normalTile;
    private GameObject glassTile;
    public List<GameObject> glassTiles = new List<GameObject>();
    public List<GameObject> fallingObstacles = new List<GameObject>();
    public List<GameObject> glassGroup1 = new List<GameObject>();
    public List<GameObject> fallingObstaclesGroup1 = new List<GameObject>();
    private bool hitGroup1 = false;
    public List<GameObject> glassGroup2 = new List<GameObject>();
    public List<GameObject> fallingObstaclesGroup2 = new List<GameObject>();
    private bool hitGroup2 = false;
    public List<GameObject> glassGroup3 = new List<GameObject>();
    public List<GameObject> fallingObstaclesGroup3 = new List<GameObject>();
    private bool hitGroup3 = false;
    public bool isNotFalling = true;
    private GameManager manager;
     // The z-position of the sphere when it collides with the jumpTile
    public float collisionZ = 0f;
    public List<GameObject> moverGroup1 = new List<GameObject>();
    public List<GameObject> movingObstaclesGroup1 = new List<GameObject>();
    public List<GameObject> moverGroup2 = new List<GameObject>();
    public List<GameObject> movingObstaclesGroup2 = new List<GameObject>();
    public List<GameObject> moverGroup3 = new List<GameObject>();
    public List<GameObject> movingObstaclesGroup3 = new List<GameObject>();
    private void Start()
    {
        // Get the Rigidbody component of the sphere
        rb = GetComponent<Rigidbody>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
         // Create a new instance of the MethFunction
        MethFunction meth = new MethFunction();
         // Modify the speed of the sphere using the MethFunction
        // pepsiSpeed = meth.piEpsilon(speed);
        //rb.MovePosition(rb.position + Vector3.back * speed * 0.025f);
        rb.useGravity = false;
         // Freeze the rotation of the sphere
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator waitBeforeLoading()
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(1f);
        Debug.Log("Done!");
        enabled = true;
    }

    IEnumerator WaitFor(float time)
    {
        yield return new WaitForSeconds(time);
        collisionZ = transform.position.z;
        isNotFalling = false;
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    private void FixedUpdate()
    {
        //Debug.Log(manager.isGamePaused);
        //Debug.Log(manager.isGameOver);
        if (!manager.isGamePaused && !manager.isGameOver) {
            //Debug.Log(CheckIfObjectIsNotAboveAnyOtherObject());
            //Debug.Log("Running");
            // Create a Vector3 for the movement of the sphere
            Vector3 direction = Vector3.forward;
            Vector3 movement = direction.normalized * speed;
            // Move the sphere according to the movement Vector3
            transform.Translate(movement * Time.fixedDeltaTime);
            if (!isNotFalling && !manager.isDeathDisabled && !manager.isGameOver) {
                rb.useGravity = true;
            }
            // If the sphere is jumping, call the Jump() method
            if (jumpTile != null && isJumping)
            {
                Jump(4f, jumpTile);
                rb.useGravity = false;
                FallingGlass();
            }
            else if (CheckIfObjectIsNotAboveAnyOtherObject() && !isJumping && !manager.isDeathDisabled && !manager.isGameOver)
            {
                exponential_falus();
            } else if (glassTiles.Count > 0) {
                FallingGlass();
            } else if (manager.levelConfig.startPortal && isJumping && rb.position.z < manager.levelConfig.startPos + 3.8f) {
                rb.useGravity = false;
                isNotFalling = true;
                Jump(4f, new Vector3(0f, 0f, manager.levelConfig.startPos));
                FallingGlass();
            } else if (rb.position.z >= manager.levelConfig.startPos + 4f) {
                isJumping = false;
            }
        } else {
            rb.velocity = Vector3.zero;
            //rb.position = new Vector3(0f, 0.5f, 0f);
        }
    }

    private void ActivateGlassTilesG1(GameObject currentGlassTile) {
        if (hitGroup1) return;
        GameObject currentGlassTileParent = currentGlassTile.transform.parent.gameObject;
        GameObject currentGlassTileNormal = currentGlassTileParent.transform.GetChild(1).gameObject;
        GameObject currentGlassTileActive = currentGlassTileParent.transform.GetChild(2).gameObject;
        currentGlassTileActive.SetActive(true);
        currentGlassTileNormal.SetActive(false);
        glassGroup1.Add(currentGlassTile);
        normalTile = null;
        glassTile = null;
        collisionZ = 0f;
        isJumping = false;
        jumpTile = null;
        List<GameObject> c_risers = GameObject.FindGameObjectsWithTag("Riser").ToList();
        if (c_risers.Any(c_riser => c_riser.transform.position.z == currentGlassTileParent.transform.position.z && c_riser.transform.position.x == currentGlassTileParent.transform.position.x)) {
        foreach (GameObject c_riser in c_risers) {
            if (c_riser.transform.position.z == currentGlassTileParent.transform.position.z && c_riser.transform.position.x == currentGlassTileParent.transform.position.x) {
                fallingObstaclesGroup1.Add(c_riser);
                break;
            }
        }
        }
        List<GameObject> c_glassTiles = GameObject.FindGameObjectsWithTag("GlassCollisionGroup1").ToList();
        foreach (GameObject c_glassTile in c_glassTiles) {
            if (!glassGroup1.Contains(c_glassTile)) {
                if (Mathf.Abs(c_glassTile.transform.position.z - currentGlassTileParent.transform.position.z) == 1f && Mathf.Abs(c_glassTile.transform.position.x - currentGlassTileParent.transform.position.x) == 1f) {
                    continue;
                }
                if (Mathf.Abs(c_glassTile.transform.position.z - currentGlassTileParent.transform.position.z) <= 1f && Mathf.Abs(c_glassTile.transform.position.x - currentGlassTileParent.transform.position.x) <= 1f) {
                    ActivateGlassTilesG1(c_glassTile);
                }
            }
        }
    }

    private void ActivateGlassTilesG2(GameObject currentGlassTile) {
        if (hitGroup2) return;
        GameObject currentGlassTileParent = currentGlassTile.transform.parent.gameObject;
        GameObject currentGlassTileNormal = currentGlassTileParent.transform.GetChild(1).gameObject;
        GameObject currentGlassTileActive = currentGlassTileParent.transform.GetChild(2).gameObject;
        currentGlassTileActive.SetActive(true);
        currentGlassTileNormal.SetActive(false);
        glassGroup2.Add(currentGlassTile);
        normalTile = null;
        glassTile = null;
        collisionZ = 0f;
        isJumping = false;
        jumpTile = null;
        List<GameObject> c_risers = GameObject.FindGameObjectsWithTag("Riser").ToList();
        if (c_risers.Any(c_riser => c_riser.transform.position.z == currentGlassTileParent.transform.position.z && c_riser.transform.position.x == currentGlassTileParent.transform.position.x)) {
        foreach (GameObject c_riser in c_risers) {
            if (c_riser.transform.position.z == currentGlassTileParent.transform.position.z && c_riser.transform.position.x == currentGlassTileParent.transform.position.x) {
                fallingObstaclesGroup2.Add(c_riser);
                break;
            }
        }
        }
        List<GameObject> c_glassTiles = GameObject.FindGameObjectsWithTag("GlassCollisionGroup2").ToList();
        foreach (GameObject c_glassTile in c_glassTiles) {
            if (!glassGroup2.Contains(c_glassTile)) {
                if (Mathf.Abs(c_glassTile.transform.position.z - currentGlassTileParent.transform.position.z) == 1f && Mathf.Abs(c_glassTile.transform.position.x - currentGlassTileParent.transform.position.x) == 1f) {
                    continue;
                }
                if (Mathf.Abs(c_glassTile.transform.position.z - currentGlassTileParent.transform.position.z) <= 1f && Mathf.Abs(c_glassTile.transform.position.x - currentGlassTileParent.transform.position.x) <= 1f) {
                    ActivateGlassTilesG2(c_glassTile);
                }
            }
        }
    }

    private void ActivateGlassTilesG3(GameObject currentGlassTile) {
        if (hitGroup3) return;
        GameObject currentGlassTileParent = currentGlassTile.transform.parent.gameObject;
        GameObject currentGlassTileNormal = currentGlassTileParent.transform.GetChild(1).gameObject;
        GameObject currentGlassTileActive = currentGlassTileParent.transform.GetChild(2).gameObject;
        currentGlassTileActive.SetActive(true);
        currentGlassTileNormal.SetActive(false);
        glassGroup3.Add(currentGlassTile);
        normalTile = null;
        glassTile = null;
        collisionZ = 0f;
        isJumping = false;
        jumpTile = null;
        List<GameObject> c_risers = GameObject.FindGameObjectsWithTag("Riser").ToList();
        if (c_risers.Any(c_riser => c_riser.transform.position.z == currentGlassTileParent.transform.position.z && c_riser.transform.position.x == currentGlassTileParent.transform.position.x)) {
        foreach (GameObject c_riser in c_risers) {
            if (c_riser.transform.position.z == currentGlassTileParent.transform.position.z && c_riser.transform.position.x == currentGlassTileParent.transform.position.x) {
                fallingObstaclesGroup3.Add(c_riser);
                break;
            }
        }
        }
        List<GameObject> c_glassTiles = GameObject.FindGameObjectsWithTag("GlassCollisionGroup3").ToList();
        foreach (GameObject c_glassTile in c_glassTiles) {
            if (!glassGroup3.Contains(c_glassTile)) {
                if (Mathf.Abs(c_glassTile.transform.position.z - currentGlassTileParent.transform.position.z) == 1f && Mathf.Abs(c_glassTile.transform.position.x - currentGlassTileParent.transform.position.x) == 1f) {
                    continue;
                }
                if (Mathf.Abs(c_glassTile.transform.position.z - currentGlassTileParent.transform.position.z) <= 1f && Mathf.Abs(c_glassTile.transform.position.x - currentGlassTileParent.transform.position.x) <= 1f) {
                    ActivateGlassTilesG3(c_glassTile);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && gameObject != null && manager != null) {
            isNotFalling = true;
            if (manager.levelConfig.startPortal && transform.position.z < manager.levelConfig.startPos + 4f) return;
            //isJumping = false;
            // Check if the sphere has collided with a jumpTile
            if (collision.gameObject.tag == "JumpCollision")
            {
                // Set isJumping to true to indicate that the sphere is jumping 
                isJumping = true;
                isNotFalling = true;
                // Disable gravity for the sphere 
                rb.useGravity = false; 
                // Set the jumpTile to the GameObject the sphere has collided with 
                jumpTile = collision.gameObject; 
                // Set the collisionZ to the z-position of the sphere 
                collisionZ = transform.position.z;
                hitGroup1 = false;
                hitGroup2 = false;
                hitGroup3 = false;
            }
            else if (collision.gameObject.tag == "NormalCollision" || collision.gameObject.tag == "NormalEndCollision" || collision.gameObject.tag == "MoverAutoCollisionGroup1" || collision.gameObject.tag == "MoverAutoCollisionGroup2" || collision.gameObject.tag == "MoverAutoCollisionGroup3")
            {
                normalTile = null;
                glassTile = null;
                collisionZ = 0f;
                isJumping = false;
                jumpTile = null;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(0f, 0f, 0f);
                }
                hitGroup1 = false;
                hitGroup2 = false;
                hitGroup3 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "GlassCollision") {
                GameObject glassTileParent = collision.gameObject.transform.parent.gameObject;
                GameObject glassTileNormal = glassTileParent.transform.GetChild(1).gameObject;
                GameObject glassTileActive = glassTileParent.transform.GetChild(2).gameObject;
                glassTileActive.SetActive(true);
                glassTileNormal.SetActive(false);
                normalTile = null;
                glassTile = null;
                collisionZ = 0f;
                isJumping = false;
                jumpTile = null;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(0f, 0f, 0f);
                }
                hitGroup1 = false;
                hitGroup2 = false;
                hitGroup3 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "GlassCollisionGroup1" && !hitGroup1) {
                ActivateGlassTilesG1(collision.gameObject);
                hitGroup1 = true;
                isJumping = false;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }
                hitGroup2 = false;
                hitGroup3 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "GlassCollisionGroup2" && !hitGroup2) {
                ActivateGlassTilesG2(collision.gameObject);
                hitGroup2 = true;
                isJumping = false;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }
                hitGroup1 = false;
                hitGroup3 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "GlassCollisionGroup3" && !hitGroup3) {
                ActivateGlassTilesG3(collision.gameObject);
                hitGroup3 = true;
                isJumping = false;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }
                hitGroup1 = false;
                hitGroup2 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "MoverCollisionGroup1") {
                normalTile = null;
                glassTile = null;
                collisionZ = 0f;
                jumpTile = null;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(0f, 0f, 0f);
                }
                hitGroup1 = true;
                hitGroup2 = false;
                hitGroup3 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "MoverCollisionGroup2") {
                normalTile = null;
                glassTile = null;
                collisionZ = 0f;
                jumpTile = null;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(0f, 0f, 0f);
                }
                hitGroup1 = false;
                hitGroup2 = true;
                hitGroup3 = false;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            } else if (collision.gameObject.tag == "MoverCollisionGroup3") {
                normalTile = null;
                glassTile = null;
                collisionZ = 0f;
                jumpTile = null;
                if (!isJumping && rb != null) {
                    //rb.velocity = new Vector3(0f, 0f, 0f);
                }
                hitGroup1 = false;
                hitGroup2 = false;
                hitGroup3 = true;
                transform.Translate(new Vector3(0f, -1f * (transform.position.y - 0.5f), 0f));
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "NormalCollision")
        {
            normalTile = null;
            glassTile = null;
            collisionZ = transform.position.z;
        } else if (collision.gameObject.tag == "GlassCollision") {
            normalTile = null;
            glassTile = null;
            collisionZ = transform.position.z;
        } else if (collision.gameObject.tag == "GlassCollisionGroup1") {
            if (transform.position.z - collision.gameObject.transform.position.z >= 0.25f) {
                foreach (GameObject m_GlassTile in glassGroup1) {
                    if (m_GlassTile == null) continue;
                    if (!glassTiles.Contains(m_GlassTile)) {
                        glassTiles.Add(m_GlassTile);
                    }
                }
                foreach (GameObject m_Riser in fallingObstaclesGroup1) {
                    if (m_Riser == null) continue;
                    fallingObstacles.Add(m_Riser);
                }
            }
        } else if (collision.gameObject.tag == "GlassCollisionGroup2") {
            if (transform.position.z - collision.gameObject.transform.position.z >= 0.25f) {
                foreach (GameObject m_GlassTile in glassGroup2) {
                    if (m_GlassTile == null) continue;
                    if (!glassTiles.Contains(m_GlassTile)) {
                        glassTiles.Add(m_GlassTile);
                    }
                }
                foreach (GameObject m_Riser in fallingObstaclesGroup2) {
                    if (m_Riser == null) continue;
                    fallingObstacles.Add(m_Riser);
                }
            }
        } else if (collision.gameObject.tag == "GlassCollisionGroup3") {
            if (transform.position.z - collision.gameObject.transform.position.z >= 0.25f) {
                foreach (GameObject m_GlassTile in glassGroup3) {
                    if (m_GlassTile == null) continue;
                    if (!glassTiles.Contains(m_GlassTile)) {
                        glassTiles.Add(m_GlassTile);
                    }
                }
                foreach (GameObject m_Riser in fallingObstaclesGroup3) {
                    if (m_Riser == null) continue;
                    fallingObstacles.Add(m_Riser);
                }
            }
        } else {
            normalTile = null;
            glassTile = null;
            collisionZ = transform.position.z;
        }
        //if (collision.gameObject.tag == "DiamondCollision") {
        //    rb.MovePosition(rb.position + Vector3.down * 5f * Time.fixedDeltaTime);
        //}
        isNotFalling = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (manager == null) return;
        if (manager.currentScene.name == "SampleScene") {
        if (manager.levelConfig.startPortal && rb.position.z < manager.levelConfig.startPos + 3.99f) {
            return;
        }
        }
        if (collision.gameObject != null && gameObject != null && manager != null) {
        if (collision.gameObject.tag == "NormalCollision" || collision.gameObject.tag == "MoverAutoCollisionGroup1" || collision.gameObject.tag == "MoverAutoCollisionGroup2" || collision.gameObject.tag == "MoverAutoCollisionGroup3")
        {
            normalTile = collision.gameObject;
            glassTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z;
            isNotFalling = false;
        } else if (collision.gameObject.tag == "JumpCollision") {
            isJumping = true;
            normalTile = null;
            glassTile = null;
        } else if (collision.gameObject.tag == "GlassCollision") {
            glassTile = collision.gameObject;
            glassTiles.Add(collision.gameObject);
            GameObject[] risers = GameObject.FindGameObjectsWithTag("Riser");
            foreach (GameObject riser in risers) {
                if (riser.transform.position.z == collision.gameObject.transform.position.z && riser.transform.position.x == collision.gameObject.transform.position.x) {
                    fallingObstacles.Add(riser);
                }
            }
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z;
            isNotFalling = false;
        } else if (collision.gameObject.tag == "MoverArrowCollision") {
            glassTile = collision.gameObject;
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z;
            isNotFalling = true;
        } else if (collision.gameObject.tag == "GlassCollisionGroup1") {
            //hitGroup1 = true;
            glassTile = collision.gameObject;
            /*if (transform.position.z - collision.gameObject.transform.position.z > 0.01f) {
                foreach (GameObject m_GlassTile in glassGroup1) {
                    if (m_GlassTile == null) continue;
                    if (!glassTiles.Contains(m_GlassTile)) {
                        glassTiles.Add(m_GlassTile);
                    }
                }
                foreach (GameObject m_Riser in fallingObstaclesGroup1) {
                    if (m_Riser == null) continue;
                    fallingObstacles.Add(m_Riser);
                }
            }*/
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z + 0.2f;
            hitGroup1 = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isNotFalling = false;
        } else if (collision.gameObject.tag == "GlassCollisionGroup2") {
            //hitGroup2 = true;
            glassTile = collision.gameObject;
            /*if (transform.position.z - collision.gameObject.transform.position.z > 0.01f) {
                foreach (GameObject m_GlassTile in glassGroup2) {
                    if (m_GlassTile == null) continue;
                    if (!glassTiles.Contains(m_GlassTile)) {
                        glassTiles.Add(m_GlassTile);
                    }
                }
                foreach (GameObject m_Riser in fallingObstaclesGroup2) {
                    if (m_Riser == null) continue;
                    fallingObstacles.Add(m_Riser);
                }
            }*/
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z + 0.2f;
            hitGroup2 = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isNotFalling = false;
        } else if (collision.gameObject.tag == "GlassCollisionGroup3") {
            //hitGroup3 = true;
            glassTile = collision.gameObject;
            /*if (transform.position.z - collision.gameObject.transform.position.z > 0.01f) {
                foreach (GameObject m_GlassTile in glassGroup3) {
                    if (m_GlassTile == null) continue;
                    if (!glassTiles.Contains(m_GlassTile)) {
                        glassTiles.Add(m_GlassTile);
                    }
                }
                foreach (GameObject m_Riser in fallingObstaclesGroup3) {
                    if (m_Riser == null) continue;
                    fallingObstacles.Add(m_Riser);
                }
            }*/
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z + 0.2f;
            hitGroup3 = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isNotFalling = false;
        } else if (collision.gameObject.tag == "MoverCollisionGroup1") {
            glassTile = collision.gameObject;
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z + 0.2f;
            //hitGroup1 = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isNotFalling = false;
        } else if (collision.gameObject.tag == "MoverCollisionGroup2") {
            glassTile = collision.gameObject;
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z + 0.2f;
            //hitGroup2 = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isNotFalling = false;
        } else if (collision.gameObject.tag == "MoverCollisionGroup3") {
            glassTile = collision.gameObject;
            normalTile = collision.gameObject;
            rb.useGravity = false;
            collisionZ = collision.gameObject.transform.position.z + 0.2f;
            //hitGroup3 = false;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            isNotFalling = false;
        } else {
            isNotFalling = true;
            /*if (!isJumping && !(manager.levelConfig.startPortal && rb.position.z < manager.levelConfig.startPos + 4f)) {
                rb.MovePosition(rb.position + Vector3.down * 0.25f);
            } */
        }
        }
    }

    public bool CheckIfObjectIsNotAboveAnyOtherObject()
    {
        // Cast a ray downward from the object's position
        Ray ray = new Ray(transform.position, Vector3.down);
        //Debug.DrawRay(transform.position, Vector3.down, Color.red);
        //Debug.DrawRay(transform.position, Vector3.down - Vector3.forward / 6f, Color.red);

        // Perform the raycast
        RaycastHit hit1;
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.25f, 2f);
        bool rayHit1 = Physics.Raycast(ray, out hit1, 2f);
        //Debug.Log(hits.Length);
        //bool rayHit3 = Physics.Raycast(ray3, out hit);
        if (hits.Length > 0/*rayHit1 || Physics.Raycast(ray3, out hit)*/)
        {
            foreach (RaycastHit hit in hits) {
                //Debug.Log(hit.collider.gameObject.name);
                if ((hit.collider.gameObject.tag == "GlassCollisionGroup1" 
                || hit.collider.gameObject.tag == "GlassCollisionGroup2" 
                || hit.collider.gameObject.tag == "GlassCollisionGroup3") 
                && hit.collider.gameObject.transform.position.y < -0.15f) {
                    return true;
                }
                if (hit.collider.gameObject.name == "GameManager") {
                    return true;
                }
                if (hit.collider.gameObject.name == "Balus") {
                    return true;
                }
                return false;
            }
            // If the raycast hits any collider, the object is above another object
            //if (hit1.collider != null) {
            //if ((hit1.collider.gameObject.tag == "GlassCollisionGroup1" 
            //|| hit1.collider.gameObject.tag == "GlassCollisionGroup2" 
            //|| hit1.collider.gameObject.tag == "GlassCollisionGroup3") 
            //&& hit1.collider.gameObject.transform.position.y < -0.15f) {
            //    return true;
            //}
            //}
            return false;
        }
        else
        {
            // If the raycast does not hit any collider, the object is not above any other object
            return true;
        }
    }

    public void FallingGlass() {
        for (int i = 0; i < glassTiles.Count; i++) {
            GameObject m_glassTile = glassTiles[i];
            if (m_glassTile == null || !m_glassTile.transform.parent.gameObject.activeSelf) {
                glassTiles.RemoveAt(i);
                continue;
            }
            GameObject glassTileParent = m_glassTile.transform.parent.gameObject;
            GameObject glassTileNormal = glassTileParent.transform.GetChild(1).gameObject;
            GameObject glassTileActive = glassTileParent.transform.GetChild(2).gameObject;
            GlassObject glassObject = glassTileParent.GetComponent<GlassObject>();
            if (glassObject != null) {
                glassTileParent.transform.position = new Vector3(glassTileParent.transform.position.x, glassTileParent.transform.position.y - glassObject.fallCoefficient, glassTileParent.transform.position.z);
                glassObject.fallCoefficient += 0.025f;
            }
        }
        foreach(GameObject riser in fallingObstacles) {
            if (riser == null) continue;
            GlassObject obstacleGlassObject = riser.GetComponent<GlassObject>();
            if (obstacleGlassObject != null) {
                if (riser.TryGetComponent<BaseAnim>(out BaseAnim baseAnim)) {
                    foreach (FrameAnim animator in baseAnim.animators) {
                        foreach (Frame frame in animator.frames) {
                            frame.position = new Vector3(riser.transform.position.x, frame.position.y - obstacleGlassObject.fallCoefficient, frame.position.z);
                        }
                    }
                }
                riser.transform.position = new Vector3(riser.transform.position.x, riser.transform.position.y - obstacleGlassObject.fallCoefficient, riser.transform.position.z);
                obstacleGlassObject.fallCoefficient += 0.025f;
            }
        }
    }

    private float jumpFunction(float x, float div_coeff) {
        return speed * -x / div_coeff;
    }

    public void Jump(float distance, GameObject jumpTile) 
    { 
        if (jumpTile == null) return;
        Vector3 newPosition = new Vector3(jumpTile.transform.position.x, jumpTile.transform.position.y + 0.1f, jumpTile.transform.position.z);
        GameObject jumpTileParent = jumpTile.transform.parent.parent.gameObject;
        GameObject jumpTileNormal = jumpTileParent.transform.GetChild(0).gameObject;
        GameObject jumpTileActive = jumpTileParent.transform.GetChild(1).gameObject;
        jumpTile = jumpTileParent;
        /* float p = distance / 3.025f * (-1f); */
        float z = transform.position.z - jumpTile.transform.position.z;
        /*float h = 0f;
        float k = 6.12f;
        float jumpCalc = p * ((float)Math.Pow(z - h, 2)) + k; */
        float jumpCalc = jumpFunction(z - (distance / (2f)), distance / 5f);
        if (z < 0f) {
            jumpCalc = 0f;
        }
        isJumping = true;
        if (!jumpTileActive.activeSelf) {
            jumpTileActive.SetActive(true);
            jumpTileNormal.SetActive(false);
        }
        // Create a Vector3 for the upward movement of the sphere 
        Vector3 movement2 = Vector3.up * (jumpCalc);
        try 
        { 
            if (z > 0.25f && z < 1.25f && isJumping) {
                newPosition = new Vector3(jumpTile.transform.position.x, jumpTileActive.transform.position.y + 0.1f, jumpTile.transform.position.z);
                jumpTileActive.transform.position = newPosition;
            }
            if (z > 2.25f && z < 3.25f && isJumping) {
                newPosition = new Vector3(jumpTile.transform.position.x, jumpTileActive.transform.position.y - 0.1f, jumpTile.transform.position.z);
                jumpTileActive.transform.position = newPosition;
            }
            if (isJumping)
            {
                transform.Translate(movement2 * Time.fixedDeltaTime, Space.World);
                //collisionZ += 0.075f;
            }
            // If the zDiff is greater than the distance, set the jumpTile to null and enable gravity for the sphere 
            if (z >= distance) 
            {
                //Debug.Log("JUMP");
                newPosition = new Vector3(jumpTile.transform.position.x, 0f, jumpTile.transform.position.z);
                jumpTileActive.transform.position = newPosition;
                jumpTile = null;
                jumpCalc = 0f;
                isJumping = false;
                collisionZ = transform.position.z;
            } 
        } 
        catch (Exception e) 
        { 
            Debug.LogError("Error in Jump: " + e.Message); 
        } 
    }

    public void Jump(float distance, Vector3 startPosition) 
    { 
        /* float p = distance / 3.025f * (-1f); */
        float z = transform.position.z - startPosition.z;
        /*float h = 0f;
        float k = 6.12f;
        float jumpCalc = p * ((float)Math.Pow(z - h, 2)) + k; */
        float jumpCalc = jumpFunction(z - (distance / (2f)), distance / 5f);
        if (z < 0f) {
            jumpCalc = 0f;
        }
        isJumping = true;
        // Create a Vector3 for the upward movement of the sphere 
        Vector3 movement2 = Vector3.up * jumpCalc;
        try 
        {
            if (isJumping)
            {
                transform.Translate(movement2 * Time.fixedDeltaTime);
                //collisionZ += 0.075f;
            }
            // If the zDiff is greater than the distance, set the jumpTile to null and enable gravity for the sphere 
            if (z >= distance) 
            {
                jumpCalc = 0f;
                isJumping = false;
                collisionZ = transform.position.z;
            } 
        } 
        catch (Exception e) 
        { 
            Debug.LogError("Error in Jump: " + e.Message); 
        } 
    }
    public void exponential_falus() 
    {
        if (!manager.isGameOver) {
            float z = transform.position.z - collisionZ;
            float downY = jumpFunction(z + 2f, 4f / 5f) * -1f;
            Vector3 downVector = Vector3.down * downY;
            transform.Translate(downVector * Time.fixedDeltaTime);
            hitGroup1 = false;
            hitGroup2 = false;
            hitGroup3 = false;
        }
    }

    public void ClearFallingObstacles() {
        glassTiles.Clear();
        fallingObstacles.Clear();
        glassGroup1.Clear();
        fallingObstaclesGroup1.Clear();
        glassGroup2.Clear();
        fallingObstaclesGroup2.Clear();
        glassGroup3.Clear();
        fallingObstaclesGroup3.Clear();
        moverGroup1.Clear();
        movingObstaclesGroup1.Clear();
        moverGroup2.Clear();
        movingObstaclesGroup2.Clear();
        moverGroup3.Clear();
        movingObstaclesGroup3.Clear();
    }
}