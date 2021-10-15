using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameStoryController : MonoBehaviour
{
    [Header("Timer + Count Box")]
    [SerializeField] private TextMeshProUGUI[] txtUI;

    [Header("Timer")]
    [SerializeField] private float countDown;
    private int timer;

    [Header("Box")]
    [SerializeField] private int maxBox;
    private int progresBox;

    [Header("Congrats")]
    [SerializeField] private GameObject panelCongrats;
    [SerializeField] private GameObject[] btnCongrats;
    [SerializeField] private Image imgStar;
    [SerializeField] private Sprite[] _imgStars;

    [Header("Setting")]
    [SerializeField] private TextMeshProUGUI[] txtSetting;
    [SerializeField] private GameObject[] btnSetting;
    [SerializeField] private GameObject panelSetting;
    [SerializeField] private GameObject panelSaved;
    [SerializeField] private AudioSource sound;

    private string[] _txtSetting;
    public bool isOpened,isMute;

    // Start is called before the first frame update
    void Start()
    {
        SetSetting();
        progresBox = DataBase.GetCurrentProgres("Box");
        ResetProgres();
        SetSoundValue();
    }

    // Update is called once per frame
    void Update()
    {
        CheckProggres();
        if (Input.GetKey(KeyCode.Escape))
        {
            panelSetting.SetActive(true);
            isOpened = true;
        }

    }

    //Udah Bener Semua
    #region Setting 

    private void SetSetting()
    {
        _txtSetting = new string[3];

        _txtSetting[0] = "Setting";
        _txtSetting[1] = "Music";

        txtSetting[0].SetText(_txtSetting[0]);
        txtSetting[1].SetText(_txtSetting[1]);

        for (int i = 0; i <= btnSetting.Length; i++)
        {
            switch (i)
            {
                //close
                case 0:
                    btnSetting[0].GetComponent<Button>().onClick.AddListener(ClosePanelSetting);
                    break;

                //Toggle Music
                case 1:
                    btnSetting[1].GetComponent<Toggle>().onValueChanged.AddListener(CheckBGMValue);
                    break;

                //Save
                case 2:
                    btnSetting[2].GetComponent<Button>().onClick.AddListener(SaveProgress);
                    break;

                //Main Menu
                case 3:
                    btnSetting[3].GetComponent<Button>().onClick.AddListener(BackToMainMenu);
                    break;
            }
        }

            panelSetting.SetActive(false);

    }

    private void SetSoundValue()
    {
        if (DataBase.GetAudio("BGM") == 0)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = false;
            sound.mute = false;
            isMute = false;
        }
        else if (DataBase.GetAudio("BGM") == 1)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = true;
            sound.mute = true;
            isMute = true;
        }
    }

    private void CheckBGMValue(bool value)
    {
        if(value == false)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = false;
            DataBase.SetAudio("BGM", 0);
            sound.mute = false;
            isMute = false;
        }else if (value == true)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = true;
            DataBase.SetAudio("BGM", 1);
            sound.mute = true;
            isMute = true;
        }
    }

    private void ClosePanelSetting()
    {
        panelSetting.SetActive(false);
        isOpened = false;
    }

    private void SaveProgress()
    {
        DataBase.SetAudio("BGM", DataBase.GetAudio("BGM"));

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level01":
                DataBase.SetCurrentProgres("Level", 1);
                break;

            case "Level02":
                DataBase.SetCurrentProgres("Level", 2);
                break;

            case "Level03":
                DataBase.SetCurrentProgres("Level", 3);
                break;

            default:
                Debug.Log("Check Ur Key");
                break;
        }
        StartCoroutine(SavedDelay(2));
    }

    private void BackToMainMenu()
    {
        //main Menu
        SceneManager.LoadScene("Menu");
    }

    #endregion

    private void CheckProggres()
    {
        StartCoroutine(PlayCountdown(0));
        progresBox = DataBase.GetCurrentProgres("Box");
        txtUI[1].SetText(progresBox + "/" + maxBox);

        if (timer <= 0)
        {
            if(progresBox == maxBox)
            {
                DataBase.SetCurrentProgres("Star", 3);
                
            }
        }
    }

    private void ResetProgres()
    {
        isOpened = false;
        // Set Timer
        if (timer <= 9)
            txtUI[0].SetText("00:0" + timer);
        else
            txtUI[0].SetText("00:" + timer);

        DataBase.SetCurrentProgres("Box", 0);

        // Set CountBox
        txtUI[1].SetText(progresBox + "/" + maxBox);

        switch (SceneManager.GetActiveScene().name)
        {
            case "Level01":
                DataBase.SetCurrentProgres("Level", 1);

                break;

            case "Level02":
                DataBase.SetCurrentProgres("Level", 2);
                break;

            case "Level03":
                DataBase.SetCurrentProgres("Level", 3);
                break;

            default:
                Debug.Log("Check Ur Key");
                break;
        }
    }


    IEnumerator SavedDelay(int value)
    {
        panelSaved.SetActive(true);
        yield return new WaitForSeconds(value);
        panelSaved.SetActive(false);

    }

    IEnumerator PlayCountdown(int value)
    {
        yield return new WaitForSeconds(value);
        timer = Mathf.FloorToInt(countDown -= Time.deltaTime);
        if(timer<=9)
            txtUI[0].SetText("00:0" + timer);
        else
            txtUI[0].SetText("00:" + timer);
    }

}
