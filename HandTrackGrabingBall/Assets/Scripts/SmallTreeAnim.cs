using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OpenRSR.Animation;

public class SmallTreeAnim : BaseAnim
{
    public override void ResetAnimation(Vector3 newPos) {
        Transform stemTransform = gameObject.transform.Find("DeceBalus_Tree_Stem");
        Transform part8Transform = gameObject.transform.Find("DeceBalus_Tree_Part8");
        Transform part9Transform = gameObject.transform.Find("DeceBalus_Tree_Part9");
        Transform part10Transform = gameObject.transform.Find("DeceBalus_Tree_Part10");
        Transform part11Transform = gameObject.transform.Find("DeceBalus_Tree_Part11");
        Transform part12Transform = gameObject.transform.Find("DeceBalus_Tree_Part12");
        Transform part13Transform = gameObject.transform.Find("DeceBalus_Tree_Part13");
        GameObject stemObject = stemTransform.gameObject;
        GameObject part8Object = part8Transform.gameObject;
        GameObject part9Object = part9Transform.gameObject;
        GameObject part10Object = part10Transform.gameObject;
        GameObject part11Object = part11Transform.gameObject;
        GameObject part12Object = part12Transform.gameObject;
        GameObject part13Object = part13Transform.gameObject;
        Frame stemInitialFrame = new Frame(new Vector3(newPos.x, -0.9f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject);
        List<Frame> frames1 = new List<Frame>();
        frames1.Add(stemInitialFrame);
        frames1.Add(stemInitialFrame);
        Frame part8InitialFrame = new Frame(new Vector3(newPos.x, -0.9f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part8Object);
        List<Frame> frames2 = new List<Frame>();
        frames2.Add(part8InitialFrame);
        frames2.Add(part8InitialFrame);
        Frame part9InitialFrame = new Frame(new Vector3(newPos.x + 0.05f, -0.9f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object);
        List<Frame> frames3 = new List<Frame>();
        frames3.Add(part9InitialFrame);
        frames3.Add(part9InitialFrame);
        Frame part10InitialFrame = new Frame(new Vector3(newPos.x, -0.95f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object);
        List<Frame> frames4 = new List<Frame>();
        frames4.Add(part10InitialFrame);
        frames4.Add(part10InitialFrame);
        Frame part11InitialFrame = new Frame(new Vector3(newPos.x - 0.05f, -0.9f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object);
        List<Frame> frames5 = new List<Frame>();
        frames5.Add(part11InitialFrame);
        frames5.Add(part11InitialFrame);
        Frame part12InitialFrame = new Frame(new Vector3(newPos.x, -0.9f, newPos.z + 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object);
        List<Frame> frames6 = new List<Frame>();
        frames6.Add(part12InitialFrame);
        frames6.Add(part12InitialFrame);
        Frame part13InitialFrame = new Frame(new Vector3(newPos.x, -0.9f, newPos.z - 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part13Object);
        List<Frame> frames7 = new List<Frame>();
        frames7.Add(part13InitialFrame);
        frames7.Add(part13InitialFrame);
        float YPos = stemInitialFrame.position.y;
        for (int i = 0; i < 10; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
            frames2.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part8Object));
            frames3.Add(new Frame(new Vector3(newPos.x + 0.05f, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
            frames4.Add(new Frame(new Vector3(newPos.x, YPos - 0.05f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
            frames5.Add(new Frame(new Vector3(newPos.x - 0.05f, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
            frames6.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z + 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
            frames7.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z - 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part13Object));
        }
        for (int i = 0; i < 6; i++) {
            YPos += 0.9f / 6f;
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
            frames2.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part8Object));
            frames3.Add(new Frame(new Vector3(newPos.x + 0.05f, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
            frames4.Add(new Frame(new Vector3(newPos.x, YPos - 0.05f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
            frames5.Add(new Frame(new Vector3(newPos.x - 0.05f, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
            frames6.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z + 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
            frames7.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z - 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part13Object));
        }
        for (int i = 0; i < 2; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
            frames2.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part8Object));
            frames3.Add(new Frame(new Vector3(newPos.x + 0.05f, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
            frames4.Add(new Frame(new Vector3(newPos.x, YPos - 0.05f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
            frames5.Add(new Frame(new Vector3(newPos.x - 0.05f, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
            frames6.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z + 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
            frames7.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z - 0.05f), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part13Object));
        }
        float xzOffset = 0.05f;
        for (int i = 0; i < 2; i++) {
            xzOffset -= 0.05f / 2f;
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
            frames2.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part8Object));
            frames3.Add(new Frame(new Vector3(newPos.x + xzOffset, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
            frames4.Add(new Frame(new Vector3(newPos.x, YPos - xzOffset, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
            frames5.Add(new Frame(new Vector3(newPos.x - xzOffset, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
            frames6.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z + xzOffset), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
            frames7.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z - xzOffset), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part13Object));
        }
        for (int i = 0; i < 40; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
            frames2.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part8Object));
            frames3.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
            frames4.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
            frames5.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
            frames6.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
            frames7.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part13Object));
        }
        frames.Add(frames1);
        frames.Add(frames2);
        frames.Add(frames3);
        frames.Add(frames4);
        frames.Add(frames5);
        frames.Add(frames6);
        frames.Add(frames7);
        FrameAnim animator = new FrameAnim(frames1);
        FrameAnim animator2 = new FrameAnim(frames2);
        FrameAnim animator3 = new FrameAnim(frames3);
        FrameAnim animator4 = new FrameAnim(frames4);
        FrameAnim animator5 = new FrameAnim(frames5);
        FrameAnim animator6 = new FrameAnim(frames6);
        FrameAnim animator7 = new FrameAnim(frames7);
        animators.Add(animator);
        animators.Add(animator2);
        animators.Add(animator3);
        animators.Add(animator4);
        animators.Add(animator5);
        animators.Add(animator6);
        animators.Add(animator7);
    }
}
