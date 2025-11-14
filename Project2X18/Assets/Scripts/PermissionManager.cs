using UnityEngine;
using System.Collections;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class PermissionManager : MonoBehaviour
{
    public GameObject solutionObject;

    IEnumerator Start()
    {
        if (solutionObject != null)
        {
            solutionObject.SetActive(false);
        }

        yield return RequestCameraPermission();

        if (solutionObject != null)
        {
            solutionObject.SetActive(true);
        }
    }

    IEnumerator RequestCameraPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            
            // Đợi cho đến khi người dùng trả lời (tối đa 10 giây)
            float startTime = Time.time;
            while (!Permission.HasUserAuthorizedPermission(Permission.Camera) && Time.time - startTime < 10f)
            {
                yield return null; // Đợi 1 frame
            }
        }
#endif
        yield return null;
    }
}