using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class RiserAnim : BaseAnim
{
    public override void ResetAnimation(Vector3 newPos) {
        Transform baseTransform = gameObject.transform.Find("Base_2");
        Transform coloredTransform = gameObject.transform.Find("ColoredPart");
        GameObject baseObject = baseTransform.gameObject;
        GameObject coloredPart = coloredTransform.gameObject;
        baseObject.transform.position = new Vector3(newPos.x, -0.2f, newPos.z);
        baseObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        coloredPart.transform.position = new Vector3(newPos.x, -1f, newPos.z);
        Frame initialFrame = new Frame(new Vector3(newPos.x, -0.2f, newPos.z), Quaternion.identity, new Vector3(0.4f, 0.4f, 0.4f), baseTransform.gameObject);
        List<Frame> frames1 = new List<Frame>();
        frames1.Add(initialFrame);
        frames1.Add(initialFrame);
        float y = initialFrame.position.y;
        float scaleY = initialFrame.scale.y;
        for (int i = 0; i < 16; i++)
        {
            y += 0.049f;
            //scaleY += 0.05f;
            frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.identity, new Vector3(0.4f, scaleY, 0.4f), baseTransform.gameObject));
        }
        for (int i = 0; i < 10; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, 0.5f, newPos.z), Quaternion.identity, new Vector3(0.4f, 0.4f, 0.4f), baseTransform.gameObject));
        }
        frames.Add(frames1);
        FrameAnim animator = new FrameAnim(frames1);
        animators.Add(animator);
        List<Frame> frames2 = new List<Frame>();
        Frame coloredFrame = new Frame(new Vector3(newPos.x, -1f, newPos.z), Quaternion.identity, new Vector3(0.4f, 0.4f, 0.4f), coloredTransform.gameObject);
        frames2.Add(coloredFrame);
        frames2.Add(coloredFrame);
        float coloredY = coloredFrame.position.y;
        for (int i = 0; i < 18; i++)
        {
            //coloredY += 0.2f;
            frames2.Add(new Frame(new Vector3(newPos.x, coloredY, newPos.z), Quaternion.identity, new Vector3(0.4f, 0.4f, 0.4f), coloredTransform.gameObject));
        }
        for (int i = 0; i < 8; i++)
        {
            coloredY += 0.025f;
            frames2.Add(new Frame(new Vector3(newPos.x, 0.5f, coloredPart.transform.position.z), Quaternion.identity, new Vector3(0.4f, 0.4f, 0.4f), baseTransform.gameObject));
        }
        frames.Add(frames2);
        FrameAnim animator2 = new FrameAnim(frames2);
        animators.Add(animator2);
    }
}
