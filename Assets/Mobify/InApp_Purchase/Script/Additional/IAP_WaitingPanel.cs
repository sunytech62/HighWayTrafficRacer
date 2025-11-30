using UnityEngine;

public class IAP_WaitingPanel : MonoBehaviour
{
    #region Refrences

    [HideInInspector] public UnityEngine.UI.Text infoTxt = null;
    UnityEngine.UI.Text InfoTxt
    {
        get
        {
            try
            {
                if (infoTxt == null) infoTxt = transform.Find("infoTxt").GetComponent<UnityEngine.UI.Text>();
            }
            catch (System.Exception) { }
            if (infoTxt == null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.ToLower().Equals("infoTxt".ToLower()))
                    {
                        infoTxt = transform.GetChild(i).GetComponent<UnityEngine.UI.Text>();
                        break;
                    }
                }
            }
            return infoTxt;
        }
    }

    [HideInInspector] public GameObject closeBtn = null;
    GameObject CloseBtn
    {
        get
        {
            if (closeBtn == null)
            {
                try
                {
                    closeBtn = transform.Find("closeBtn").gameObject;
                }
                catch (System.Exception) { }
                if(closeBtn == null) 
                {
                    for(int i=0;i<transform.childCount;i++)
                    {
                        if(transform.GetChild(i).GetComponent<UnityEngine.UI.Button>() != null)
                        {
                            closeBtn = transform.GetChild(i).gameObject;
                            break;
                        }
                    }
                }
            }
            return closeBtn;
        }
    }

    [HideInInspector] public Transform animObj = null;
    Transform AnimObj
    {
        get
        {
            if (animObj == null)
            {
                try
                {
                    if (transform.Find("IconBG").TryGetComponent(out animObj))
                        if (animObj.GetComponent<UnityEngine.UI.Image>() != null)
                            animObj.GetComponent<UnityEngine.UI.Image>().preserveAspect = true;
                }
                catch (System.Exception) { }
                if (animObj == null)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).name.ToLower().Equals("IconBG".ToLower()))
                        {
                            animObj = transform.GetChild(i);
                            break;
                        }
                    }
                }
            }
            return animObj;
        }
    }

    #endregion

    #region Public Function
    public void EnablePanel(Status status)
    {
        switch (status)
        {
            case Status.Purchasing:
                {
                    AnimObj.gameObject.SetActive(true);
                    InfoTxt.text = "Processing Purchase.\nPlease Wait";
                    CloseBtn.SetActive(false);
                    break;
                }
            case Status.Restored:
                {
                    AnimObj.gameObject.SetActive(false);
                    InfoTxt.text = "Successfully Restored.";
                    CloseBtn.SetActive(true);
                    break;
                }
            case Status.Restoring:
                {
                    AnimObj.gameObject.SetActive(true);
                    InfoTxt.text = "Restoring Purchase.\nPlease Wait";
                    CloseBtn.SetActive(false);
                    break;
                }
            case Status.Purchased:
                {
                    AnimObj.gameObject.SetActive(false);
                    InfoTxt.text = "Successfully Purchased.";
                    CloseBtn.SetActive(true);
                    break;
                }
            case Status.RestoreFailed:
                {
                    AnimObj.gameObject.SetActive(false);
                    InfoTxt.text = "You Have Nothing To Restore.";
                    CloseBtn.SetActive(true);
                    break;
                }
        }
#if AdsManager_Applovin
AdsManager_Applovin.Instance.SaveLastBannerStatus();
#endif
        gameObject.SetActive(true);
        currentWaitingTime = maxWaitingTime;
    }
    public void DisablePanel()
    {
#if AdsManager_Applovin
AdsManager_Applovin.Instance.LoadLastBannerStatus();
#endif
        gameObject.SetActive(false);
    }
#endregion

    #region Variable
    Vector3 rotationSpeed = new Vector3(0, 0, -400);
    static readonly float maxWaitingTime = 5.0f;
    float currentWaitingTime = maxWaitingTime;
    #endregion

    #region Built-in Function
    void Update()
    {
        AnimObj.Rotate(rotationSpeed * Time.deltaTime);
        currentWaitingTime -= Time.deltaTime;
        if(currentWaitingTime < 0.0f && !CloseBtn.activeSelf)
        {
            CloseBtn.SetActive(true);
        }
    }
    void OnDisable()
    {
        try
        {
            //UnityEngine.Purchasing.UnityPurchasing.ClearTransactionLog();
        }catch(System.Exception) { }
    }
    #endregion

    #region Custom Enum
    public enum Status
    {
        Purchasing,
        Restoring,
        Purchased,
        Restored,
        RestoreFailed,
    }
    #endregion
}