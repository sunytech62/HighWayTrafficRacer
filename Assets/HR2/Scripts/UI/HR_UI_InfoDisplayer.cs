using System.Collections;
using TMPro;
using UnityEngine;

public class HR_UI_InfoDisplayer : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static HR_UI_InfoDisplayer instance;
    public static HR_UI_InfoDisplayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<HR_UI_InfoDisplayer>();
            }

            return instance;
        }
    }
    #endregion

    public GameObject content;

    public TextMeshProUGUI descText;

    public void ShowInfo(string description)
    {
        descText.text = description;
        content.SetActive(true);
        StartCoroutine("CloseInfoDelayed");
    }

    public void CloseInfo()
    {
        content.SetActive(false);
    }

    private IEnumerator CloseInfoDelayed()
    {
        yield return new WaitForSeconds(3);
        content.SetActive(false);
    }
}
