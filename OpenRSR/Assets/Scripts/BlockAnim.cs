using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class BlockAnim : BaseAnim {
    public float yOffset = 0.0f;
    public float dyOffset = 1.0f;

    public override void ResetAnimation(Vector3 newPos) {
        Transform blockTransform = gameObject.transform;
        blockTransform.position = new Vector3(newPos.x, newPos.y + yOffset - 0.5f, newPos.z);
        Frame initialFrame = new Frame(new Vector3(newPos.x, newPos.y + yOffset - 0.5f, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), blockTransform.gameObject);
        List<Frame> frames1 = new List<Frame>();
        frames1.Add(initialFrame);
        frames1.Add(initialFrame);
        float yPos = initialFrame.position.y;
        for (int i = 0; i < 50; i++) {
            yPos += dyOffset / 50f;
            frames1.Add(new Frame(new Vector3(newPos.x, yPos, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), blockTransform.gameObject));
        }
        for (int i = 0; i < 50; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, yPos, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), blockTransform.gameObject));
        }
        for (int i = 0; i < 50; i++) {
            yPos -= dyOffset / 50f;
            frames1.Add(new Frame(new Vector3(newPos.x, yPos, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), blockTransform.gameObject));
        }
        frames1.Add(initialFrame);
        frames1.Add(initialFrame);
        FrameAnim animator = new FrameAnim(frames1);
        animators.Add(animator);
    }
}