//using GlobalValues;
using UnityEngine;

public class DestroyObj : MonoBehaviour
{
	public Transform progressPos;

	public float deletePos;

	private void Update()
	{
		if (base.transform.position.z - progressPos.position.z <= deletePos && !GameManager.instance.isGameOver)
		{
			Object.Destroy(base.gameObject);
		}
	}
}