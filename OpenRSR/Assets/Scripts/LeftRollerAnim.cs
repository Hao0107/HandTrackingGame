using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRollerAnim : MonoBehaviour
{
    public GameObject balus;
    public float xOffset = 0f;
    private GameObject baseObject;
    
    // Start is called before the first frame update
    void Start()
    {
        baseObject = gameObject.transform.Find("DeceBalus_Roller_Base").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float curFrame2 = (balus.transform.position.z - gameObject.transform.position.z) / 2f;
        if (xOffset == 0f) {
            baseObject.transform.position = new Vector3(-Mathf.Sin(curFrame2) * 2f + xOffset, 0.55f, baseObject.transform.position.z);
            baseObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Asin(baseObject.transform.position.x / 2f) * 180f / Mathf.PI);
        } else if (xOffset == 1f) {
            baseObject.transform.position = new Vector3(-Mathf.Sin(curFrame2 - Mathf.PI / 6f) * 2f, 0.55f, baseObject.transform.position.z);
            baseObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Asin(baseObject.transform.position.x / 2f) * 180f / Mathf.PI);
        } else if (xOffset == 2f) {
            baseObject.transform.position = new Vector3(-Mathf.Sin(curFrame2 - Mathf.PI / 2f) * 2f, 0.55f, baseObject.transform.position.z);
            baseObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Asin(baseObject.transform.position.x / 2f) * 180f / Mathf.PI);
        } else if (xOffset == -2f) {
            baseObject.transform.position = new Vector3(-Mathf.Sin(curFrame2 + Mathf.PI / 2f) * 2f, 0.55f, baseObject.transform.position.z);
            baseObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Asin(baseObject.transform.position.x / 2f) * 180f / Mathf.PI);
        } else if (xOffset == -1f) {
            baseObject.transform.position = new Vector3(-Mathf.Sin(curFrame2 + Mathf.PI / 6f) * 2f, 0.55f, baseObject.transform.position.z);
            baseObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Asin(baseObject.transform.position.x / 2f) * 180f / Mathf.PI);
        } else {
            baseObject.transform.position = new Vector3(-Mathf.Sin(curFrame2) * 2f + xOffset, 0.55f, baseObject.transform.position.z);
            //baseObject.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Asin(baseObject.transform.position.x / 2f) * 180f / Mathf.PI);
        }
    }
}
