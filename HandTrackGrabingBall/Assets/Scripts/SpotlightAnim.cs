using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class SpotlightAnim : BaseAnim
{
    private float RotationFunction(float x) {
        return Mathf.Cos(Mathf.PI * x) * 30f;
    }

    public override void ResetAnimation(Vector3 newPos) {
        Transform laserTransform = gameObject.transform.Find("DeceBalus_Spotlight_Laser");
        GameObject laserPart = laserTransform.gameObject;
        laserPart.transform.position = new Vector3(newPos.x, 0f, newPos.z);
        List<Frame> frames1 = new List<Frame>();
        Frame initialLaserFrame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 30f)), new Vector3(1f, 1f, 1f), laserTransform.gameObject);
        frames1.Add(initialLaserFrame);
        frames1.Add(initialLaserFrame);
        float y = initialLaserFrame.position.y;
        float rotZ = initialLaserFrame.rotation.eulerAngles.z;
        for (int i = 0; i < 120; i++) {
            rotZ = RotationFunction((float)i / 60f);
            frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, 0f, rotZ), new Vector3(1f, 1f, 1f), laserTransform.gameObject));
        }
        frames.Add(frames1);
        FrameAnim animator = new FrameAnim(frames1);
        animators.Add(animator);
    }
}
