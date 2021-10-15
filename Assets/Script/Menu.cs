using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Menu Utama")]
    [SerializeField] private Button[] btnMenuUtama;
    [SerializeField] private Button btnCloseInfo;
    [SerializeField] private GameObject panelInfo;

    [Header("Level Menu")]
    [SerializeField] private Button[] btnLevel;
    [SerializeField] private Button btnCloseLevel;
    [SerializeField] private GameObject panelLevel,panelWarning;
    [SerializeField] private Sprite[] spriteLevel;

    private void Start()
    {
        RestartGameplay();
        SetMenuBTN();
    }

    private void RestartGameplay()
    {
        if(DataBase.GetCurrentProgres("Level") < 1)
            DataBase.SetCurrentProgres("Level", 1);
        else if (DataBase.GetCurrentProgres("Level") > 1)
            DataBase.SetCurrentProgres("Level", DataBase.GetCurrentProgres("Level"));

        panelInfo.SetActive(false);
        panelLevel.SetActive(false);
    }

    private void SetMenuBTN()
    {
        btnMenuUtama[0].onClick.AddListener(RestartLevel);
        btnMenuUtama[1].onClick.AddListener(ContinueLevel);
        btnMenuUtama[2].onClick.AddListener(OpenPanelInfo);
        btnCloseInfo.onClick.AddListener(ClosePanelInfo);
        btnCloseLevel.onClick.AddListener(BackToMainMenu);
    }

    private void OpenPanelInfo() 
    {
        btnMenuUtama[0].gameObject.SetActive(false);
        btnMenuUtama[1].gameObject.SetActive(false);
        btnMenuUtama[2].gameObject.SetActive(false);
        panelInfo.SetActive(true);
    }
    private void ClosePanelInfo() 
    {
        btnMenuUtama[0].gameObject.SetActive(true);
        btnMenuUtama[1].gameObject.SetActive(true);
        btnMenuUtama[2].gameObject.SetActive(true);
        panelInfo.SetActive(false);
    }

    private void RestartLevel()
    {
        btnMenuUtama[0].gameObject.SetActive(false);
        btnMenuUtama[1].gameObject.SetActive(false);
        btnMenuUtama[2].gameObject.SetActive(false);

        panelLevel.SetActive(true);
        DataBase.SetCurrentProgres("Level", 1);

        for (int i = 0; i <= btnLevel.Length; i++)
        {
            if (i == 0)
                btnLevel[0].onClick.AddListener(delegate { GetLevel("Level01"); });
            else
                btnLevel[i].onClick.AddListener(delegate { StartCoroutine(DelayWarning(3)); });
        }
    }

    private void ContinueLevel()
    {
        btnMenuUtama[0].gameObject.SetActive(false);
        btnMenuUtama[1].gameObject.SetActive(false);
        btnMenuUtama[2].gameObject.SetActive(false);

        panelLevel.SetActive(true);
        int level = DataBase.GetCurrentProgres("Level");

        switch (level)
        {
            case 1:
                btnLevel[0].onClick.AddListener(delegate { GetLevel("Level01"); });
                btnLevel[0].gameObject.GetComponent<Image>().sprite = spriteLevel[0];
                break;

            case 2:
                btnLevel[1].onClick.AddListener(delegate { GetLevel("Level02"); });
                btnLevel[1].gameObject.GetComponent<Image>().sprite = spriteLevel[1];
                break;

            case 3:
                btnLevel[2].onClick.AddListener(delegate { GetLevel("Level03"); });
                btnLevel[2].gameObject.GetComponent<Image>().sprite = spriteLevel[2];
                break;
        }

        for(int i = 4; i <= btnLevel.Length; i++)
        {
            btnLevel[i].gameObject.GetComponent<Image>().sprite = spriteLevel[3];
            btnLevel[i].onClick.AddListener(delegate { StartCoroutine(DelayWarning(3));});

        }

    }

    private void BackToMainMenu()
    {
        btnMenuUtama[0].gameObject.SetActive(true);
        btnMenuUtama[1].gameObject.SetActive(true);
        btnMenuUtama[2].gameObject.SetActive(true);

        panelLevel.SetActive(false);
    }

    private void GetLevel(string value)
    {
        SceneManager.LoadScene(value);
    }

    IEnumerator DelayWarning(int value)
    {
        panelWarning.SetActive(true);
        yield return new WaitForSeconds(value);
        panelWarning.SetActive(false);
    }

}
