using UnityEngine;

public class StorePanel : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] GameObject[] selectedBtns;

    static int selectionPanelIndex = 0;

    void Start()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == selectionPanelIndex)
                panels[i].SetActive(true);
            else
                panels[i].SetActive(false);
        }
        for (int i = 0; i < selectedBtns.Length; i++)
        {
            if (i == selectionPanelIndex)
                selectedBtns[i].SetActive(true);
            else
                selectedBtns[i].SetActive(false);
        }
    }

    public void SelectPanel(int panelIndex)
    {
        selectionPanelIndex = panelIndex;
        UpdateUI();
    }
}
