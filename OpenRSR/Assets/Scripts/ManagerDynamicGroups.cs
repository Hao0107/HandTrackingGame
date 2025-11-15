using System.Collections;
using System.Collections.Generic;
using OpenRSR.Animation;
using UnityEngine;

public class ManagerDynamicGroups : MonoBehaviour
{
    public enum GroupType
	{
		fragile = 0,
		mover = 1,
		moverAuto = 2,
		none = 3
	}

	public GroupType groupType = GroupType.none;
    public LevelRenderer levelRenderer;
    public MoverVisual moverActivator;
    private Vector2Int[] poses;
    private Transform[] transforms;
    public int autoX;
    public int autoY;
    private Coroutine running;
    private int pivot;
    private int point;

    private void Start() {
        Initialize();
    }

    private void Update() {
        if (transform.childCount == 0) {
            Destroy(gameObject);
        }
    }

    public void Initialize() {
        poses = new Vector2Int[transform.childCount];
        transforms = new Transform[transform.childCount];
        pivot = (int)transform.position.z;
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            transforms[i] = child;
            poses[i] = new Vector2Int((int)child.position.x + 2, (int)child.position.z);
        }
    }

    public void TriggerGroup()
	{
		if (groupType == GroupType.moverAuto && !(moverActivator == null) && FreeToUse(autoX, autoY) && running == null)
		{
			moverActivator.Active();
			UpdateCollision(autoX, autoY, -1);
			running = StartCoroutine(Move(autoX, autoY));
		}
	}

	public void TriggerGroup(int x, int y, int activated, int activatedX)
	{
		point = activated;
		//Debug.Log(FreeToUse(x, y));
		if (running == null /*&& FreeToUse(x, y)*/)
		{
			if (levelRenderer.levelVisuals[activated][activatedX].Enemy.GetComponentsInChildren<MoverVisual>().Length != 0)
			{
				levelRenderer.levelVisuals[activated][activatedX].Enemy.GetComponentInChildren<MoverVisual>().Active();
			}
			UpdateCollision(x, y, point);
			running = StartCoroutine(Move(x, y));
		}
	}

	private IEnumerator CheckFall(Transform player)
	{
		float start = player.position.z;
		while (player.position.z < start + 1f)
		{
			SphereMovement sphm = player.GetComponent<SphereMovement>();
			if (sphm != null) {
				yield return !sphm.CheckIfObjectIsNotAboveAnyOtherObject();
			}
			yield return null;
		}
		//UpdateCollision(0, 0, point, fall: true);
	}

	public float Distance(float x1, float x2, float y1, float y2) {
		return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
	}

    public void UpdateCollision(int x, int y, int point) {
        foreach (Vector2Int vector2Int in poses) {
            GameObject tileCollision = levelRenderer.levelVisuals[vector2Int.y][vector2Int.x + 1].Tile?.transform.Find("Collision").gameObject;
			//Debug.Log((vector2Int.x - 2) + ", " + vector2Int.y);
			//Debug.Log(GameManager.instance.balus.transform.position.x + ", " + GameManager.instance.balus.transform.position.z);
			//Debug.Log(Mathf.Abs(GameManager.instance.balus.transform.position.x - (vector2Int.x - 2)));
			//Debug.Log(Mathf.Abs(GameManager.instance.balus.transform.position.z - vector2Int.y) + ", " + Mathf.Abs(GameManager.instance.balus.transform.position.x - (vector2Int.x - 2)));
            if (Distance(GameManager.instance.balus.transform.position.z, vector2Int.y, GameManager.instance.balus.transform.position.x, (vector2Int.x - 2)) < 1f) {
                GameObject newCollision = Instantiate(tileCollision, new Vector3(vector2Int.x - 2, 0f, vector2Int.y - y), Quaternion.identity);
                newCollision.transform.parent = transform;
				DestroyObj CollisionDestroyer = newCollision.AddComponent<DestroyObj>();
            	CollisionDestroyer.progressPos = GameManager.instance.balus.transform;
            	CollisionDestroyer.deletePos = -12f;
            }
        }
    }

    private IEnumerator Move(int x, int z)
	{
		Vector3 startPos = base.transform.position;
		Vector3 mod = new Vector3(x, 0f, z);
		float progress = 0f;
		bool activated = false;
		while (progress < 1f)
		{
			progress = Mathf.Clamp(progress + Time.deltaTime * 8f, 0f, 1f);
			base.transform.position = startPos + mod * GameManager.instance.curves[2].Evaluate(progress);
			BaseAnim[] baseAnims = GetComponentsInChildren<BaseAnim>();
			for (int i = 0; i < baseAnims.Length; i++) {
				BaseAnim current = baseAnims[i];
				foreach (FrameAnim animator in current.animators) {
					foreach (Frame frame in animator.frames) {
						Vector3 frameStartPos = current.transform.position;
						frame.position = new Vector3(current.transform.position.x, frame.position.y, current.transform.position.z);
					}
				}
			}
			if ((double)progress > 0.5 && !activated)
			{
				TriggerAuto(x, z);
			}
			yield return null;
		}
	}

    private void TriggerAuto(int x, int z)
	{
		Vector2Int[] array = poses;
		for (int i = 0; i < array.Length; i++)
		{
			Vector2Int vector2Int = array[i];
			for (int j = 0; j < 4; j++)
			{
				int num = 0;
				int num2 = 0;
				switch (j)
				{
				case 0:
					num = 0;
					num2 = 1;
					break;
				case 1:
					num = -1;
					num2 = 0;
					break;
				case 2:
					num = 1;
					num2 = 0;
					break;
				case 3:
					num = 0;
					num2 = -1;
					break;
				}
				//Debug.Log(vector2Int.x + x + num);
				if (vector2Int.x + x + num + 1 >= 0 && vector2Int.x + x + num + 1 <= 6 && vector2Int.y + z + num2 >= 0 && vector2Int.y + z + num2 < levelRenderer.positionsCount && levelRenderer.levelVisuals[vector2Int.y + z + num2][vector2Int.x + x + num + 1].Tile != null)
				{
					levelRenderer.levelVisuals[vector2Int.y + z + num2][vector2Int.x + x + num + 1].Tile?.GetComponentInParent<ManagerDynamicGroups>()?.TriggerGroup();
				}
			}
		}
	}

    private bool FreeToUse(int x, int z)
	{
		Vector2Int[] array = poses;
		for (int i = 0; i < array.Length; i++)
		{
			Vector2Int vector2Int = array[i];
			if (vector2Int.y + z >= levelRenderer.positionsCount)
			{
				return false;
			}
			Vector2Int[] array2 = poses;
			int num = 0;
			while (true)
			{
				if (num < array2.Length)
				{
					Vector2Int vector2Int2 = array2[num];
					if (vector2Int2.x == vector2Int.x + x && vector2Int2.y == vector2Int.y + z)
					{
						break;
					}
					num++;
					continue;
				}
				//Debug.Log(levelRenderer.levelVisuals[vector2Int.y + z][vector2Int.x + x].Tile);
				if ((levelRenderer.levelVisuals[vector2Int.y + z][vector2Int.x + x + 1].Tile == null))
				{
					break;
				}
				return false;
			}
		}
		return true;
	}
}