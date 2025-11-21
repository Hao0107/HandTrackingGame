using System.Collections;
using System.Globalization;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class FilterArgsJson {
    public string name;
    public float start;
    public float end;
    public float startValue;
    public float endValue;
}

[System.Serializable]
public class GrayScaleArgsJson {
    public string name;
    public float start;
    public float end;
    public MyColor startValue;
    public MyColor endValue;
}

[System.Serializable]
public class Vector2Json {
	public float x;
	public float y;

	public Vector2 ToUnityVector2() {
		return new Vector2(x, y);
	}
}

[System.Serializable]
public class ChromaticArgsJson {
    public string name;
    public float start;
    public float end;
    public Vector2Json startValue;
    public Vector2Json endValue;
}

// Borrowed from RSR
public class FilterManager : MonoBehaviour
{
	public static FilterManager filterManager;

	public ScreenMaterial[] screenMaterial;

	private Transform ballTransform;

	private CultureInfo ci;

	private Coroutine Chromatic;

	private Coroutine Gray;

	private Coroutine Negative;

	private Coroutine Glitch;

	private Coroutine Scan;

	private Coroutine Hue;

	private void Start()
	{
		if (filterManager != null)
		{
			UnityEngine.Debug.LogError("Filter Manager already exists!");
		}
		filterManager = this;
		ballTransform = GameObject.Find("Balus").transform;
		ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
		ci.NumberFormat.NumberDecimalSeparator = ".";
	}

	public void DisableAll()
	{
		if (Chromatic != null)
		{
			StopCoroutine(Chromatic);
		}
		if (Gray != null)
		{
			StopCoroutine(Gray);
		}
		if (Negative != null)
		{
			StopCoroutine(Negative);
		}
		if (Glitch != null)
		{
			StopCoroutine(Glitch);
		}
		if (Scan != null)
		{
			StopCoroutine(Scan);
		}
		if (Hue != null)
		{
			StopCoroutine(Hue);
		}
		for (int i = 0; i < screenMaterial.Length; i++)
		{
			screenMaterial[i].enabled = false;
		}
	}

	public void SetEffect(float pos, string[] args)
	{
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split('/');
			string[] array2 = array[0].Split(',');
			string[] array3 = array[1].Split(',');
			string[] array4 = array[2].Split(',');
			if (array2[1].Equals("0"))
			{
				if (Gray != null)
				{
					StopCoroutine(Gray);
				}
				//Debug.Log(float.Parse(array3[0], ci) + " " + float.Parse(array3[1], ci) + " " + float.Parse(array3[2], ci) + " " + float.Parse(array3[3], ci));
				Gray = StartCoroutine(SetGrayScale(pos, float.Parse(array2[0], ci), new Color(float.Parse(array3[0], ci), float.Parse(array3[1], ci), float.Parse(array3[2], ci), float.Parse(array3[3], ci)), new Color(float.Parse(array4[0], ci), float.Parse(array4[1], ci), float.Parse(array4[2], ci), float.Parse(array4[3], ci))));
			}
			else if (array2[1].Equals("1"))
			{
				if (Chromatic != null)
				{
					StopCoroutine(Chromatic);
				}
				Chromatic = StartCoroutine(SetChromaticAberration(pos, float.Parse(array2[0], ci), new Vector2(float.Parse(array3[0], ci), float.Parse(array3[1], ci)), new Vector2(float.Parse(array4[0], ci), float.Parse(array4[1], ci))));
			}
			else if (array2[1].Equals("2"))
			{
				if (Negative != null)
				{
					StopCoroutine(Negative);
				}
				Negative = StartCoroutine(SetNegative(pos, float.Parse(array2[0], ci), float.Parse(array3[0], ci), float.Parse(array4[0], ci)));
			}
			else if (array2[1].Equals("3"))
			{
				if (Glitch != null)
				{
					StopCoroutine(Glitch);
				}
				Glitch = StartCoroutine(SetGlitch(pos, float.Parse(array2[0], ci), float.Parse(array3[0], ci), float.Parse(array4[0], ci)));
			}
			else if (array2[1].Equals("4"))
			{
				if (Scan != null)
				{
					StopCoroutine(Scan);
				}
				Scan = StartCoroutine(SetScanLines(pos, float.Parse(array2[0], ci), float.Parse(array3[0], ci), float.Parse(array4[0], ci)));
			}
			else if (array2[1].Equals("5"))
			{
				if (Hue != null)
				{
					StopCoroutine(Hue);
				}
				Hue = StartCoroutine(SetHueShifting(pos, float.Parse(array2[0], ci), float.Parse(array3[0], ci), float.Parse(array4[0], ci)));
			}
		}
	}

	public void SetEffect(float pos, string jsonString) {
		// try to deserialize json
		FilterArgsJson filterArgsJson = JsonConvert.DeserializeObject<FilterArgsJson>(jsonString);
		if (filterArgsJson != null) {
			if (filterArgsJson.name == "negative") {
				if (Negative != null)
				{
					StopCoroutine(Negative);
				}
				Negative = StartCoroutine(SetNegative(pos, filterArgsJson.end, filterArgsJson.startValue, filterArgsJson.endValue));
			} else if (filterArgsJson.name == "glitch") {
				if (Glitch != null)
				{
					StopCoroutine(Glitch);
				}
				Glitch = StartCoroutine(SetGlitch(pos, filterArgsJson.end, filterArgsJson.startValue, filterArgsJson.endValue));
			} else if (filterArgsJson.name == "scan_lines") {
				if (Scan != null)
				{
					StopCoroutine(Scan);
				}
				Scan = StartCoroutine(SetScanLines(pos, filterArgsJson.end, filterArgsJson.startValue, filterArgsJson.endValue));
			} else if (filterArgsJson.name == "hue_shifting") {
				if (Hue != null)
				{
					StopCoroutine(Hue);
				}
				Hue = StartCoroutine(SetHueShifting(pos, filterArgsJson.end, filterArgsJson.startValue, filterArgsJson.endValue));
			}
		} else {
			GrayScaleArgsJson grayScaleArgsJson = JsonConvert.DeserializeObject<GrayScaleArgsJson>(jsonString);
			if (grayScaleArgsJson != null) {
				if (Gray != null)
				{
					StopCoroutine(Gray);
				}
				Gray = StartCoroutine(SetGrayScale(pos, grayScaleArgsJson.end, grayScaleArgsJson.startValue.ToUnityColor(), grayScaleArgsJson.endValue.ToUnityColor()));
			} else {
				ChromaticArgsJson chromaticArgsJson = JsonConvert.DeserializeObject<ChromaticArgsJson>(jsonString);
				if (chromaticArgsJson != null) {
					if (Chromatic != null) {
						StopCoroutine(Chromatic);
					}
					Chromatic = StartCoroutine(SetChromaticAberration(pos, chromaticArgsJson.end, chromaticArgsJson.startValue.ToUnityVector2(), chromaticArgsJson.endValue.ToUnityVector2()));
				} else {
					throw new System.Exception("Invalid JSON string");
				}
			}
		}
	}

	private IEnumerator SetChromaticAberration(float start, float end, Vector2 startValue, Vector2 endValue)
	{
		float minimalLimit = 0f;
		screenMaterial[0].enabled = true;
		Material chrome = screenMaterial[0].screenMat;
		bool set = end > start + 1f;
		float safeValueX = Mathf.Abs(endValue.x);
		float safeValueY = Mathf.Abs(endValue.y);
		if (!set)
		{
			chrome.SetFloat("_X", startValue.x);
			chrome.SetFloat("_Y", startValue.y);
			safeValueX = Mathf.Abs(startValue.x);
			safeValueY = Mathf.Abs(startValue.y);
		}
		while (set && end >= ballTransform.position.z)
		{
			Vector2 vector = Vector2.Lerp(startValue, endValue, Mathf.InverseLerp(start, end, ballTransform.position.z));
			chrome.SetFloat("_X", vector.x);
			chrome.SetFloat("_Y", vector.y);
			yield return null;
		}
		if (safeValueX <= minimalLimit && safeValueY <= minimalLimit)
		{
			screenMaterial[0].enabled = false;
		}
	}

	private IEnumerator SetGrayScale(float start, float end, Color startValue, Color endValue)
	{
		float minimalLimit = 0f;
		screenMaterial[1].enabled = true;
		Material gray = screenMaterial[1].screenMat;
		bool set = end > start + 1f;
		float safeValue = endValue.a;
		if (!set)
		{
			gray.SetColor("_Color", startValue);
			safeValue = startValue.a;
		}
		while (set && end >= ballTransform.position.z)
		{
			Color value = Color.Lerp(startValue, endValue, Mathf.InverseLerp(start, end, ballTransform.position.z));
			gray.SetColor("_Color", value);
			yield return null;
		}
		if (safeValue <= minimalLimit)
		{
			screenMaterial[1].enabled = false;
		}
	}

	private IEnumerator SetNegative(float start, float end, float startValue, float endValue)
	{
		float minimalLimit = 0f;
		screenMaterial[2].enabled = true;
		Material negative = screenMaterial[2].screenMat;
		bool set = end > start + 1f;
		float safeValue = endValue;
		if (!set)
		{
			negative.SetFloat("_ColorIntensity", startValue);
			safeValue = startValue;
		}
		while (set && end >= ballTransform.position.z)
		{
			float value = Mathf.Lerp(startValue, endValue, Mathf.InverseLerp(start, end, ballTransform.position.z));
			negative.SetFloat("_ColorIntensity", value);
			yield return null;
		}
		if (safeValue <= minimalLimit)
		{
			screenMaterial[2].enabled = false;
		}
	}

	private IEnumerator SetGlitch(float start, float end, float startValue, float endValue)
	{
		screenMaterial[3].enabled = true;
		Material negative = screenMaterial[3].screenMat;
		bool set = end > start + 1f;
		if (!set)
		{
			negative.SetFloat("_BlockSize", startValue);
		}
		while (set && end >= ballTransform.position.z)
		{
			float value = Mathf.Lerp(startValue, endValue, Mathf.InverseLerp(start, end, ballTransform.position.z));
			negative.SetFloat("_BlockSize", value);
			yield return null;
		}
		if (endValue >= 3.1f)
		{
			screenMaterial[3].enabled = false;
		}
	}

	private IEnumerator SetScanLines(float start, float end, float startValue, float endValue)
	{
		float minimalLimit = 0f;
		screenMaterial[4].enabled = true;
		Material hue = screenMaterial[4].screenMat;
		bool set = end > start + 1f;
		float safeValue = endValue;
		if (!set)
		{
			hue.SetFloat("_OpacityScanline", startValue);
			safeValue = startValue;
		}
		while (set && end >= ballTransform.position.z)
		{
			float value = Mathf.Lerp(startValue, endValue, Mathf.InverseLerp(start, end, ballTransform.position.z));
			hue.SetFloat("_OpacityScanline", value);
			yield return null;
		}
		if (safeValue <= minimalLimit)
		{
			screenMaterial[4].enabled = false;
		}
	}

	private IEnumerator SetHueShifting(float start, float end, float startValue, float endValue)
	{
		float minimalLimit = 0f;
		screenMaterial[5].enabled = true;
		Material scan = screenMaterial[5].screenMat;
		bool set = end > start + 1f;
		float safeValue = endValue;
		if (!set)
		{
			scan.SetFloat("_Shift", startValue);
			safeValue = startValue;
		}
		while (set && end >= ballTransform.position.z)
		{
			float value = Mathf.Lerp(startValue, endValue, Mathf.InverseLerp(start, end, ballTransform.position.z));
			scan.SetFloat("_Shift", value);
			yield return null;
		}
		if (safeValue <= minimalLimit || safeValue >= 1f - minimalLimit)
		{
			screenMaterial[5].enabled = false;
		}
	}
}
