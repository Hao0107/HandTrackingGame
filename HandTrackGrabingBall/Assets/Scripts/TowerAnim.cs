using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OpenRSR.Animation;

public class TowerAnim : BaseAnim
{
    private GameObject eyeObject;
    public override void ResetAnimation(Vector3 newPos) {
        Transform stemTransform = gameObject.transform.Find("DeceBalus_Tower_Section_1");
        Transform part1Transform = gameObject.transform.Find("DeceBalus_Tower_Section_2");
        Transform part2Transform = gameObject.transform.Find("DeceBalus_Tower_Section_3");
        Transform part3Transform = gameObject.transform.Find("DeceBalus_Tower_Section_4");
        Transform part4Transform = gameObject.transform.Find("DeceBalus_Tower_MiddleSpike_1");
        Transform part5Transform = gameObject.transform.Find("DeceBalus_Tower_MiddleSpike_2");
        Transform part6Transform = gameObject.transform.Find("DeceBalus_Tower_MiddleSpike_3");
        Transform part7Transform = gameObject.transform.Find("DeceBalus_Tower_MiddleSpike_4");
        Transform eyeTransform = gameObject.transform.Find("DeceBalus_Tower_Eye");
        Transform part9Transform = gameObject.transform.Find("DeceBalus_Tower_TopSpike_1");
        Transform part10Transform = gameObject.transform.Find("DeceBalus_Tower_TopSpike_2");
        Transform part11Transform = gameObject.transform.Find("DeceBalus_Tower_TopSpike_3");
        Transform part12Transform = gameObject.transform.Find("DeceBalus_Tower_TopSpike_4");
        GameObject stemObject = stemTransform.gameObject;
        GameObject part1Object = part1Transform.gameObject;
        GameObject part2Object = part2Transform.gameObject;
        GameObject part3Object = part3Transform.gameObject;
        GameObject part4Object = part4Transform.gameObject;
        GameObject part5Object = part5Transform.gameObject;
        GameObject part6Object = part6Transform.gameObject;
        GameObject part7Object = part7Transform.gameObject;
        eyeObject = eyeTransform.gameObject;
        GameObject part9Object = part9Transform.gameObject;
        GameObject part10Object = part10Transform.gameObject;
        GameObject part11Object = part11Transform.gameObject;
        GameObject part12Object = part12Transform.gameObject;
        Frame stemInitialFrame = new Frame(new Vector3(newPos.x, -0.6f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject);
        List<Frame> frames1 = new List<Frame>();
        frames1.Add(stemInitialFrame);
        frames1.Add(stemInitialFrame);
        float YPos = stemInitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            YPos += 0.6f / 20f;
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
        }
        for (int i = 0; i < 160; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), stemObject));
        }
        Frame part1InitialFrame = new Frame(new Vector3(newPos.x, -0.99f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part1Object);
        List<Frame> frames2 = new List<Frame>();
        frames2.Add(part1InitialFrame);
        frames2.Add(part1InitialFrame);
        float part1YPos = part1InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part1YPos += 0.6f / 20f;
            frames2.Add(new Frame(new Vector3(newPos.x, part1YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part1Object));
        }
        for (int i = 0; i < 20; i++) {
            part1YPos += 0.39f / 20f;
            frames2.Add(new Frame(new Vector3(newPos.x, part1YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part1Object));
        }
        for (int i = 0; i < 140; i++) {
            frames2.Add(new Frame(new Vector3(newPos.x, part1YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part1Object));
        }
        Frame part2InitialFrame = new Frame(new Vector3(newPos.x, -1.38f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part2Object);
        List<Frame> frames3 = new List<Frame>();
        frames3.Add(part2InitialFrame);
        frames3.Add(part2InitialFrame);
        float part2YPos = part2InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part2YPos += 0.6f / 20f;
            frames3.Add(new Frame(new Vector3(newPos.x, part2YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part2Object));
        }
        for (int i = 0; i < 40; i++) {
            part2YPos += 0.39f / 20f;
            frames3.Add(new Frame(new Vector3(newPos.x, part2YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part2Object));
        }
        for (int i = 0; i < 120; i++) {
            frames3.Add(new Frame(new Vector3(newPos.x, part2YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part2Object));
        }
        Frame part3InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part3Object);
        List<Frame> frames4 = new List<Frame>();
        frames4.Add(part3InitialFrame);
        frames4.Add(part3InitialFrame);
        float part3YPos = part3InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part3YPos += 0.6f / 20f;
            frames4.Add(new Frame(new Vector3(newPos.x, part3YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part3Object));
        }
        for (int i = 0; i < 80; i++) {
            part3YPos += 0.39f / 20f;
            frames4.Add(new Frame(new Vector3(newPos.x, part3YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part3Object));
        }
        for (int i = 0; i < 80; i++) {
            frames4.Add(new Frame(new Vector3(newPos.x, part3YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part3Object));
        }
        Frame part4InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part4Object);
        List<Frame> frames5 = new List<Frame>();
        frames5.Add(part4InitialFrame);
        frames5.Add(part4InitialFrame);
        float part4YPos = part4InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part4YPos += 0.6f / 20f;
            frames5.Add(new Frame(new Vector3(newPos.x, part4YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part4Object));
        }
        for (int i = 0; i < 80; i++) {
            part4YPos += 0.39f / 20f;
            frames5.Add(new Frame(new Vector3(newPos.x, part4YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part4Object));
        }
        for (int i = 0; i < 80; i++) {
            frames5.Add(new Frame(new Vector3(newPos.x, part4YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part4Object));
        }
        Frame part5InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part5Object);
        List<Frame> frames6 = new List<Frame>();
        frames6.Add(part5InitialFrame);
        frames6.Add(part5InitialFrame);
        float part5YPos = part5InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part5YPos += 0.6f / 20f;
            frames6.Add(new Frame(new Vector3(newPos.x, part5YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part5Object));
        }
        for (int i = 0; i < 80; i++) {
            part5YPos += 0.39f / 20f;
            frames6.Add(new Frame(new Vector3(newPos.x, part5YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part5Object));
        }
        for (int i = 0; i < 80; i++) {
            frames6.Add(new Frame(new Vector3(newPos.x, part5YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part5Object));
        }
        Frame part6InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part6Object);
        List<Frame> frames7 = new List<Frame>();
        frames7.Add(part6InitialFrame);
        frames7.Add(part6InitialFrame);
        float part6YPos = part6InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part6YPos += 0.6f / 20f;
            frames7.Add(new Frame(new Vector3(newPos.x, part6YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part6Object));
        }
        for (int i = 0; i < 80; i++) {
            part6YPos += 0.39f / 20f;
            frames7.Add(new Frame(new Vector3(newPos.x, part6YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part6Object));
        }
        for (int i = 0; i < 80; i++) {
            frames7.Add(new Frame(new Vector3(newPos.x, part6YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part6Object));
        }
        Frame part7InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part7Object);
        List<Frame> frames8 = new List<Frame>();
        frames8.Add(part7InitialFrame);
        frames8.Add(part7InitialFrame);
        float part7YPos = part7InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part7YPos += 0.6f / 20f;
            frames8.Add(new Frame(new Vector3(newPos.x, part7YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part7Object));
        }
        for (int i = 0; i < 80; i++) {
            part7YPos += 0.39f / 20f;
            frames8.Add(new Frame(new Vector3(newPos.x, part7YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part7Object));
        }
        for (int i = 0; i < 80; i++) {
            frames8.Add(new Frame(new Vector3(newPos.x, part7YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part7Object));
        }
        Frame eyeInitialFrame = new Frame(new Vector3(newPos.x, -0.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), eyeObject);
        List<Frame> frames9 = new List<Frame>();
        frames9.Add(eyeInitialFrame);
        frames9.Add(eyeInitialFrame);
        float eyeYPos = eyeInitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            eyeYPos += 0.6f / 20f;
            frames9.Add(new Frame(new Vector3(newPos.x, eyeYPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), eyeObject));
        }
        for (int i = 0; i < 80; i++) {
            eyeYPos += 0.39f / 20f;
            frames9.Add(new Frame(new Vector3(newPos.x, eyeYPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), eyeObject));
        }
        for (int i = 0; i < 80; i++) {
            frames9.Add(new Frame(new Vector3(newPos.x, eyeYPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), eyeObject));
        }
        Frame part9InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object);
        List<Frame> frames10 = new List<Frame>();
        frames10.Add(part9InitialFrame);
        frames10.Add(part9InitialFrame);
        float part9YPos = part9InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part9YPos += 0.6f / 20f;
            frames10.Add(new Frame(new Vector3(newPos.x, part9YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
        }
        for (int i = 0; i < 80; i++) {
            part9YPos += 0.39f / 20f;
            frames10.Add(new Frame(new Vector3(newPos.x, part9YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
        }
        for (int i = 0; i < 80; i++) {
            frames10.Add(new Frame(new Vector3(newPos.x, part9YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part9Object));
        }
        Frame part10InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object);
        List<Frame> frames11 = new List<Frame>();
        frames11.Add(part10InitialFrame);
        frames11.Add(part10InitialFrame);
        float part10YPos = part10InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part10YPos += 0.6f / 20f;
            frames11.Add(new Frame(new Vector3(newPos.x, part10YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
        }
        for (int i = 0; i < 80; i++) {
            part10YPos += 0.39f / 20f;
            frames11.Add(new Frame(new Vector3(newPos.x, part10YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
        }
        for (int i = 0; i < 80; i++) {
            frames11.Add(new Frame(new Vector3(newPos.x, part10YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part10Object));
        }
        Frame part11InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object);
        List<Frame> frames12 = new List<Frame>();
        frames12.Add(part11InitialFrame);
        frames12.Add(part11InitialFrame);
        float part11YPos = part11InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part11YPos += 0.6f / 20f;
            frames12.Add(new Frame(new Vector3(newPos.x, part11YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
        }
        for (int i = 0; i < 80; i++) {
            part11YPos += 0.39f / 20f;
            frames12.Add(new Frame(new Vector3(newPos.x, part11YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
        }
        for (int i = 0; i < 80; i++) {
            frames12.Add(new Frame(new Vector3(newPos.x, part11YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part11Object));
        }
        Frame part12InitialFrame = new Frame(new Vector3(newPos.x, -2.16f, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object);
        List<Frame> frames13 = new List<Frame>();
        frames13.Add(part12InitialFrame);
        frames13.Add(part12InitialFrame);
        float part12YPos = part12InitialFrame.position.y;
        for (int i = 0; i < 20; i++) {
            part12YPos += 0.6f / 20f;
            frames13.Add(new Frame(new Vector3(newPos.x, part12YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
        }
        for (int i = 0; i < 80; i++) {
            part12YPos += 0.39f / 20f;
            frames13.Add(new Frame(new Vector3(newPos.x, part12YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
        }
        for (int i = 0; i < 80; i++) {
            frames13.Add(new Frame(new Vector3(newPos.x, part12YPos, newPos.z), Quaternion.Euler(new Vector3(0f, 0f, 0f)), new Vector3(1f, 1f, 1f), part12Object));
        }
        frames.Add(frames1);
        frames.Add(frames2);
        frames.Add(frames3);
        frames.Add(frames4);
        frames.Add(frames5);
        frames.Add(frames6);
        frames.Add(frames7);
        frames.Add(frames8);
        frames.Add(frames9);
        frames.Add(frames10);
        frames.Add(frames11);
        frames.Add(frames12);
        frames.Add(frames13);
        FrameAnim animator = new FrameAnim(frames1);
        FrameAnim animator2 = new FrameAnim(frames2);
        FrameAnim animator3 = new FrameAnim(frames3);
        FrameAnim animator4 = new FrameAnim(frames4);
        FrameAnim animator5 = new FrameAnim(frames5);
        FrameAnim animator6 = new FrameAnim(frames6);
        FrameAnim animator7 = new FrameAnim(frames7);
        FrameAnim animator8 = new FrameAnim(frames8);
        FrameAnim animator9 = new FrameAnim(frames9);
        FrameAnim animator10 = new FrameAnim(frames10);
        FrameAnim animator11 = new FrameAnim(frames11);
        FrameAnim animator12 = new FrameAnim(frames12);
        FrameAnim animator13 = new FrameAnim(frames13);

        animators.Add(animator);
        animators.Add(animator2);
        animators.Add(animator3);
        animators.Add(animator4);
        animators.Add(animator5);
        animators.Add(animator6);
        animators.Add(animator7);
        animators.Add(animator8);
        animators.Add(animator9);
        animators.Add(animator10);
        animators.Add(animator11);
        animators.Add(animator12);
        animators.Add(animator13);
    }

    // Update is called once per frame
    new public void Update()
    {
        currentFrame = (int)Math.Floor((balus.transform.position.z - gameObject.transform.position.z + animationOffset) * 13f);
        float curFrame2 = (balus.transform.position.z - gameObject.transform.position.z + animationOffset) * 13f;
        float t = curFrame2 - (int)curFrame2;
        if (currentFrame < 0)
        {
            currentFrame = 0;
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
        }
        else if (currentFrame >= 180)
        {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(180, 0.99f);
            }
            Vector3 direction = balus.transform.position - eyeObject.transform.position;
            eyeObject.transform.rotation = Quaternion.LookRotation(-direction);
        }
        else
        {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(currentFrame + 1, t);
            }
        }
    }
}
