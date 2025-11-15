using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class RightHammerLargeAnim : BaseAnim
{
    public float xOffset = 0f;
    private float RotationFunction(float x) {
        return Mathf.Exp(-x * x);
    }

    public override void ResetAnimation(Vector3 newPos) {
        Transform hammerTransform = gameObject.transform.Find("DeceBalus_Large_Hammer_Hammer");
        GameObject hammerPart = hammerTransform.gameObject;
        hammerPart.transform.position = new Vector3(newPos.x + xOffset, 0.2f, newPos.z);
        List<Frame> frames1 = new List<Frame>();
        Frame initialHammerFrame = new Frame(new Vector3(newPos.x + xOffset, 0f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 75f)), new Vector3(1f, 1f, 1f), hammerTransform.gameObject);
        frames1.Add(initialHammerFrame);
        frames1.Add(initialHammerFrame);
        float y = initialHammerFrame.position.y;
        float rotChange = 0.1f;
        float rotZ = initialHammerFrame.rotation.eulerAngles.z;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 15; j++) {
                rotChange = RotationFunction(((float)j - 5f) / 16f) * 10.87975f * Mathf.Pow(-1f, (float)i+1f);
                rotZ += rotChange;
                frames1.Add(new Frame(new Vector3(newPos.x + xOffset, y, newPos.z), Quaternion.Euler(0f, 0f, rotZ), new Vector3(1f, 1f, 1f), hammerTransform.gameObject));
            }
            for (int j = 0; j < 15; j++) {
                frames1.Add(new Frame(new Vector3(newPos.x + xOffset, y, newPos.z), Quaternion.Euler(0f, 0f, rotZ), new Vector3(1f, 1f, 1f), hammerTransform.gameObject));
            }
        }
        FrameAnim animator = new FrameAnim(frames1);
        animators.Add(animator);
    }
}
