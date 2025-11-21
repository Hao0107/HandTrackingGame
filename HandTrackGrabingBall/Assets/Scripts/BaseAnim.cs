using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OpenRSR.Animation;

public abstract class BaseAnim : MonoBehaviour
{
    public GameObject balus;
    public float animationOffset = 5f;
    public float animationSpeed = 1f;
    public List<FrameAnim> animators = new List<FrameAnim>();
    public List<List<Frame>> frames = new List<List<Frame>>();
    public int currentFrame = 0;
    // Start is called before the first frame update
    public void Start() {
        ResetAnimation(transform.position);
        currentFrame = 0;
    }

    abstract public void ResetAnimation(Vector3 newPos);

    // Update is called once per frame
    public void Update() {
        currentFrame = (int)Math.Floor((balus.transform.position.z - gameObject.transform.position.z + animationOffset) * animationSpeed);
        float curFrame2 = (balus.transform.position.z - gameObject.transform.position.z + animationOffset) * animationSpeed;
        float t = curFrame2 - (int)curFrame2;
        if (animators.Count == 0) return;
        if (currentFrame < 0)
        {
            currentFrame = 0;
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
        }
        else if (currentFrame >= animators[0].frames.Count - 1)
        {
            foreach (FrameAnim animator in animators)
            {
                animator.SetFrame(animators[0].frames.Count - 1, 0.99f);
            }
        }
        else
        {
            foreach (FrameAnim animator in animators)
            {
                animator.SetFrame(currentFrame + 1, t);
            }
        }
    }
}
