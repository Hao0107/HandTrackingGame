using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverVisual : MonoBehaviour
{
    public GameObject normalPart;
    public GameObject activePart;
    public AudioSource sound;
    public void Start() {
        activePart = transform.GetChild(0).gameObject;
        normalPart = transform.GetChild(1).gameObject;
    }
    public void Active() {
        normalPart.SetActive(false);
        activePart.SetActive(true);
        if (sound) {
            sound.Play();
        }
    }
}
