using UnityEngine;

[ExecuteInEditMode]
public class ScreenMaterial : MonoBehaviour
{
	public Material screenMat;

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, screenMat);
	}
}

