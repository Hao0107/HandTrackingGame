using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NLua;
using OpenRSR.Animation;

public class BaseObject : MonoBehaviour
{
    public List<FrameAnim> animators = new List<FrameAnim>();
    private GameManager gameManager = GameManager.instance;
    public Lua objState = new Lua();
    public LuaFunction resetAnimationFunction;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        name = gameObject.name;
    }

    public void ResetAnimation(Vector3 position, bool seektoZero = true) {
        if (resetAnimationFunction != null) {
            resetAnimationFunction.Call(position);
            LuaTable animTable = objState["animators"] as LuaTable;
            List<FrameAnim> new_animators = new List<FrameAnim>();
            foreach (var animator in animTable.Values) {
                LuaTable animatorTable = animator as LuaTable;
                LuaTable framesTable = animatorTable["frames"] as LuaTable;
                List<Frame> frames = new List<Frame>();
                foreach (var frame in framesTable.Values) {
                    LuaTable frameTable = frame as LuaTable;
                    Vector3 new_position = (Vector3)frameTable["position"];
                    Quaternion rotation = (Quaternion)frameTable["rotation"];
                    Vector3 scale = (Vector3)frameTable["scale"];
                    GameObject name = frameTable["name"] as GameObject;
                    frames.Add(new Frame(new_position, rotation, scale, name));
                }
                FrameAnim frameAnim = new FrameAnim(frames);
                new_animators.Add(frameAnim);
            }
            foreach (FrameAnim animator in animators) {
                gameManager.anims[animator.name.name] = animator;
            }
            gameManager.minAnimationCount = new_animators.Count;
            gameManager.animGroups[this.name] = new_animators;
            this.animators = new_animators;
        }
        if (seektoZero) {
            foreach (FrameAnim animator in animators) {
                animator.SetFrame(1, 0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
