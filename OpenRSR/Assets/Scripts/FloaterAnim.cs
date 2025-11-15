using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class FloaterAnim : BaseAnim
{
    public GameObject part1Object;
    public GameObject part2Object;
    public GameObject part3Object;

    public override void ResetAnimation(Vector3 newPos) {
        Transform part1Transform = gameObject.transform.Find("DeceBalus_Floater_Part1");
        Transform part2Transform = gameObject.transform.Find("DeceBalus_Floater_Part2");
        Transform part3Transform = gameObject.transform.Find("DeceBalus_Floater_Part3");
        part1Object = part1Transform.gameObject;
        part2Object = part2Transform.gameObject;
        part3Object = part3Transform.gameObject;
        Frame part1InitialFrame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), part1Object);
        Frame part2InitialFrame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), part2Object);
        List<Frame> frames1 = new List<Frame>();
        frames1.Add(part1InitialFrame);
        frames1.Add(part1InitialFrame);
        List<Frame> frames2 = new List<Frame>();
        frames2.Add(part2InitialFrame);
        frames2.Add(part2InitialFrame);
        float rotx = 0f;
        float posY = 0f;
        for (int i = 0; i < 80; i++) {
            rotx += 180f / 80f;
            posY += 2.7f / 80f;
            frames1.Add(new Frame(new Vector3(newPos.x, posY, newPos.z), Quaternion.Euler(rotx, 0f, 0f), new Vector3(1f, 1f, 1f), part1Object));
            frames2.Add(new Frame(new Vector3(newPos.x, posY, newPos.z), Quaternion.Euler(rotx, 0f, 0f), new Vector3(1f, 1f, 1f), part2Object));
        }
        for (int i = 0; i < 100; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, posY, newPos.z), Quaternion.Euler(rotx, 0f, 0f), new Vector3(1f, 1f, 1f), part1Object));
            frames2.Add(new Frame(new Vector3(newPos.x, posY, newPos.z), Quaternion.Euler(rotx, 0f, 0f), new Vector3(1f, 1f, 1f), part2Object));
        }
        frames.Add(frames1);
        frames.Add(frames2);
        FrameAnim animator = new FrameAnim(frames1);
        FrameAnim animator2 = new FrameAnim(frames2);
        animators.Add(animator);
        animators.Add(animator2);
    }

    // Update is called once per frame
    new public void Update()
    {
        currentFrame = (int)Math.Floor((balus.transform.position.z - gameObject.transform.position.z + animationOffset) * 15f);
        float curFrame2 = (balus.transform.position.z - gameObject.transform.position.z + animationOffset) * 15f;
        float t = curFrame2 - (int)curFrame2;
        if (currentFrame < 0)
        {
            currentFrame = 0;
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
            part1Object.SetActive(true);
            part2Object.SetActive(false);
            part3Object.SetActive(false);
        }
        else if (currentFrame >= 90 && currentFrame < 180) {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(currentFrame + 1, t);
            }
            part1Object.SetActive(false);
            part2Object.SetActive(true);
            part3Object.SetActive(true);
        }
        else if (currentFrame >= 180)
        {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
            part1Object.SetActive(true);
            part2Object.SetActive(false);
            part3Object.SetActive(false);
        }
        else
        {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(currentFrame + 1, t);
            }
        }
    }
}
