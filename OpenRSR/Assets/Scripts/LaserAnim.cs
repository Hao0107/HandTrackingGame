using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public class LaserAnim : BaseAnim
{
    public GameObject laserObject;

    public override void ResetAnimation(Vector3 newPos) {
        Transform cannonTransform = gameObject.transform.Find("DeceBalus_Laser_Cannon");
        Transform lid1Transform = gameObject.transform.Find("DeceBalus_Laser_Cannon_Lid1");
        Transform lid2Transform = gameObject.transform.Find("DeceBalus_Laser_Cannon_Lid2");
        Transform laserTransform = gameObject.transform.Find("DeceBalus_Laser_Laser");
        GameObject cannonObject = cannonTransform.gameObject;
        GameObject lid1Object = lid1Transform.gameObject;
        GameObject lid2Object = lid2Transform.gameObject;
        laserObject = laserTransform.gameObject;
        //laserObject.transform.position = newPos;
        laserObject.SetActive(false);
        Frame initialFrame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), cannonTransform.gameObject);
        List<Frame> frames1 = new List<Frame>();
        frames1.Add(initialFrame);
        frames1.Add(initialFrame);
        List<Frame> frames2 = new List<Frame>();
        List<Frame> frames3 = new List<Frame>();
        Frame lid1Frame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), lid1Transform.gameObject);
        Frame lid2Frame = new Frame(new Vector3(newPos.x, 0f, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), lid2Transform.gameObject);
        frames2.Add(lid1Frame);
        frames2.Add(lid1Frame);
        frames3.Add(lid2Frame);
        frames3.Add(lid2Frame);
        float lid1RotationY = 0f;
        float lid2RotationY = 0f;
        float lid1PosZ = newPos.z;
        float lid2PosZ = newPos.z;
        float y = newPos.y;
        for (int i = 0; i < 80; i++) {
            y += Mathf.Sin((float)i / 2f) * 0.01f;
            lid1RotationY += 0.2f;
            lid2RotationY -= 0.2f;
            frames1.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.identity, new Vector3(1f, 1f, 1f), cannonTransform.gameObject));
            frames2.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, lid1RotationY, 0f), new Vector3(1f, 1f, 1f), lid1Transform.gameObject));
            frames3.Add(new Frame(new Vector3(newPos.x, y, newPos.z), Quaternion.Euler(0f, lid2RotationY, 0f), new Vector3(1f, 1f, 1f), lid2Transform.gameObject));
        }
        float z = newPos.z;
        for (int i = 0; i < 10; i++) {
            z += 0.02f;
            frames1.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.identity, new Vector3(1f, 1f, 1f), cannonTransform.gameObject));
            frames2.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.Euler(0f, 90f, 0f), new Vector3(1f, 1f, 1f), lid1Transform.gameObject));
            frames3.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.Euler(0f, -90f, 0f), new Vector3(1f, 1f, 1f), lid2Transform.gameObject));
        }
        for (int i = 0; i < 20; i++) {
            frames1.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.identity, new Vector3(1f, 1f, 1f), cannonTransform.gameObject));
            frames2.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.Euler(0f, 90f, 0f), new Vector3(1f, 1f, 1f), lid1Transform.gameObject));
            frames3.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.Euler(0f, -90f, 0f), new Vector3(1f, 1f, 1f), lid2Transform.gameObject));
        }
        lid1RotationY = 90f;
        lid2RotationY = -90f;
        for (int i = 0; i < 10; i++) {
            z -= 0.02f;
            lid1RotationY -= 9f;
            lid2RotationY += 9f;
            frames1.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.identity, new Vector3(1f, 1f, 1f), cannonTransform.gameObject));
            frames2.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.Euler(0f, lid1RotationY, 0f), new Vector3(1f, 1f, 1f), lid1Transform.gameObject));
            frames3.Add(new Frame(new Vector3(newPos.x, y, z), Quaternion.Euler(0f, lid2RotationY, 0f), new Vector3(1f, 1f, 1f), lid2Transform.gameObject));
        }
        frames.Add(frames1);
        frames.Add(frames2);
        frames.Add(frames3);
        FrameAnim animator = new FrameAnim(frames1);
        FrameAnim animator2 = new FrameAnim(frames2);
        FrameAnim animator3 = new FrameAnim(frames3);
        animators.Add(animator);
        animators.Add(animator2);
        animators.Add(animator3);
    }

    // Update is called once per frame
    new public void Update()
    {
        currentFrame = (int)Math.Floor((balus.transform.position.z - gameObject.transform.position.z + animationOffset) * 5f);
        float curFrame2 = (balus.transform.position.z - gameObject.transform.position.z + animationOffset) * 5f;
        float t = curFrame2 - (int)curFrame2;
        if (currentFrame < 0)
        {
            currentFrame = 0;
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
        }
        else if (currentFrame >= 80 && currentFrame < 120) {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(currentFrame + 1, 0.99f);
            }
            laserObject.SetActive(true);
        }
        else if (currentFrame >= 120)
        {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
            laserObject.SetActive(false);
        }
        else
        {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(currentFrame + 1, t);
            }
        }
    }
}
