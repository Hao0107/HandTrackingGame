using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class LeftHammerAnim : BaseAnim
{
    private float RotationFunction(float x) {
        return Mathf.Exp(-x * x);
    }

    public override void ResetAnimation(Vector3 newPos) {
        Transform baseTransform = gameObject.transform.Find("DeceBalus_Small_Hammer_Base");
        Transform hammerTransform = gameObject.transform.Find("DeceBalus_Small_Hammer_Hammer");
        GameObject baseObject = baseTransform.gameObject;
        GameObject hammerPart = hammerTransform.gameObject;
        baseObject.transform.position = new Vector3(newPos.x, 0f, newPos.z);
        hammerPart.transform.position = new Vector3(newPos.x, 0f, newPos.z);
        Frame initialFrame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.Euler(new Vector3(0f, 180f, 0f)), new Vector3(1f, 1f, 1f), baseTransform.gameObject);
        List<Frame> frames1 = new List<Frame>();
        List<Frame> frames2 = new List<Frame>();
        frames1.Add(initialFrame);
        frames1.Add(initialFrame);
        Frame initialHammerFrame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.Euler(new Vector3(0f, 180f, 0f)), new Vector3(1f, 1f, 1f), hammerTransform.gameObject);
        frames2.Add(initialHammerFrame);
        frames2.Add(initialHammerFrame);
        float y = initialFrame.position.y;
        float rotChange = 0.1f;
        float rotY = initialFrame.rotation.eulerAngles.y;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 15; j++) {
                rotChange = RotationFunction(((float)j - 7.5f) / 16f) * 12.898f;
                rotY -= rotChange;
                frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, rotY, 0f), new Vector3(1f, 1f, 1f), baseTransform.gameObject));
                frames2.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, rotY, 0f), new Vector3(1f, 1f, 1f), hammerTransform.gameObject));
            }
            for (int j = 0; j < 15; j++) {
                frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, rotY, 0f), new Vector3(1f, 1f, 1f), baseTransform.gameObject));
                frames2.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, rotY, 0f), new Vector3(1f, 1f, 1f), hammerTransform.gameObject));
            }
        }
        frames.Add(frames1);
        frames.Add(frames2);
        FrameAnim animator = new FrameAnim(frames1);
        FrameAnim animator2 = new FrameAnim(frames2);
        animators.Add(animator);
        animators.Add(animator2);
    }
}
