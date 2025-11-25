using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    [SerializeField] bool isReduceTimeInEditor;

    IEnumerator Start()
    {
        GameManager.Instance.LoadingPanel(true, 1);
        float loadingTime = 0f;

        if (isReduceTimeInEditor && Application.isEditor)
        {
            loadingTime = 11f;
        }
        else
        {
            loadingTime = 6f;
            yield return new WaitForSecondsRealtime(6f);
        }

        var async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;

        while (loadingTime < 12f)
        {
            if (AdsManager_AdmobMediation.Instance.IsAppOpenAdLoaded())
            {
                AdsManager_AdmobMediation.Instance.ShowAppOpenAd();
                yield return new WaitForSeconds(0.2f);
                break;
            }

            loadingTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        async.allowSceneActivation = true;
    }
}
