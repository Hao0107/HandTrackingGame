using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationLayerTranform : MonoBehaviour
{
    public GameObject layerToTransform;
    public Vector3 positionOffset;

    // Start is called before the first frame update
    void Start()
    {
        layerToTransform.transform.position += positionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
