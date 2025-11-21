using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class CrusherAnim : BaseAnim
{
    private float RotationFunction(float x) {
        return Mathf.Exp(-x * x);
    }

    public override void ResetAnimation(Vector3 newPos) {
        //balus = GameObject.Find("Balus");
        //animationOffset = 11f;
        Transform baseTransform = gameObject.transform.Find("Base");
        Transform propellerTransform = gameObject.transform.Find("Propeller");
        GameObject baseObject = baseTransform.gameObject;
        GameObject propellerPart = propellerTransform.gameObject;
        baseObject.transform.position = new Vector3(newPos.x, 0.5f, newPos.z);
        propellerPart.transform.position = new Vector3(newPos.x, 0.5f, newPos.z);
        List<Frame> frames1 = new List<Frame>();
        Frame initialFrame = new Frame(new Vector3(newPos.x, 0.5f, newPos.z), Quaternion.identity, new Vector3(0.8f, 0.8f, 0.8f), baseTransform.gameObject);
        frames1.Add(initialFrame);
        frames1.Add(initialFrame);
        float y = initialFrame.position.y;
        float scaleY = initialFrame.scale.y;
        float yChange = 0.1f;
        for (int i = 0; i < 60; i++)
        {
            yChange = 0.25f * RotationFunction(((float)i - 30f) / 16f) / 3f;
            y += yChange;
            //scaleY += 0.05f;
            frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.identity, new Vector3(0.8f, scaleY, 0.8f), baseTransform.gameObject));
        }
        for (int i = 0; i < 11; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.identity, new Vector3(0.8f, scaleY, 0.8f), baseTransform.gameObject));
        }
        for (int i = 0; i < 3; i++) {
            y -= 0.7f;
            frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.identity, new Vector3(0.8f, scaleY, 0.8f), baseTransform.gameObject));
        }
        frames1.Add(new Frame(new Vector3(newPos.x, 0.49f, newPos.z), Quaternion.identity, new Vector3(0.8f, 0.8f, 0.8f), baseTransform.gameObject));
        frames1.Add(new Frame(new Vector3(newPos.x, 0.49f, newPos.z), Quaternion.identity, new Vector3(0.8f, 0.8f, 0.8f), baseTransform.gameObject));
        frames1.Add(new Frame(new Vector3(newPos.x, 0.49f, newPos.z), Quaternion.identity, new Vector3(0.8f, 0.8f, 0.8f), baseTransform.gameObject));
        frames.Add(frames1);
        FrameAnim animator = new FrameAnim(frames1);
        animators.Add(animator);
        List<Frame> frames2 = new List<Frame>();
        Frame propellerFrame = new Frame(new Vector3(newPos.x, 0.5f, newPos.z), Quaternion.identity, new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject);
        frames2.Add(propellerFrame);
        frames2.Add(propellerFrame);
        float propellerY = propellerFrame.position.y;
        float propellerYChange = 0.1f;
        float propellerRotationYChange = 0f;
        float propellerRotationY = 0f;
        for (int i = 0; i < 60; i++)
        {
            propellerYChange = 0.25f * RotationFunction(((float)i - 30f) / 16f) / 3f;
            propellerY += propellerYChange;
            propellerRotationYChange = RotationFunction(((float)i - 30f) / 16f) * 60f;
            propellerRotationY += propellerRotationYChange;
            frames2.Add(new Frame(new Vector3(newPos.x, propellerY, newPos.z), Quaternion.Euler(0f, propellerRotationY, 0f), new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject));
        }
        for (int i = 0; i < 11; i++) {
            propellerRotationY += propellerRotationYChange;
            frames2.Add(new Frame(new Vector3(newPos.x, propellerY, newPos.z), Quaternion.Euler(0f, propellerRotationY, 0f), new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject));
        }
        for (int i = 0; i < 3; i++)
        {
            propellerY -= 0.7f;
            propellerRotationY += propellerRotationYChange;
            frames2.Add(new Frame(new Vector3(newPos.x, propellerY, newPos.z), Quaternion.Euler(0f, propellerRotationY, 0f), new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject));
        }
        frames2.Add(new Frame(new Vector3(newPos.x, 0.49f, newPos.z), Quaternion.Euler(0f, propellerRotationY, 0f), new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject));
        frames2.Add(new Frame(new Vector3(newPos.x, 0.49f, newPos.z), Quaternion.Euler(0f, propellerRotationY, 0f), new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject));
        frames2.Add(new Frame(new Vector3(newPos.x, 0.49f, newPos.z), Quaternion.Euler(0f, propellerRotationY, 0f), new Vector3(0.8f, 0.8f, 0.8f), propellerTransform.gameObject));
        frames.Add(frames2);
        FrameAnim animator2 = new FrameAnim(frames2);
        animators.Add(animator2);
    }
}
