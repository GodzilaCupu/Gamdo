using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameStoryController : MonoBehaviour
{
    [Header("Timer + Count Box")]
    [SerializeField] private TextMeshProUGUI[] txtUI;

    [Header("Timer")]
    [SerializeField] private float countDown;

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
    [SerializeField] private AudioSource bgmSound;
    [SerializeField] private AudioSource[] sfx_Sounds; 

    [Header("General")]
    [SerializeField] private GameObject panelTask;
    //[SerializeField] private Button btnPause;

    private string[] _txtSetting;
    public bool isOpened,isMute;

    Tutorial tutor;

    // Start is called before the first frame update
    void Start()
    {
        progresBox = DataBase.GetCurrentProgres("Box");
        //btnPause = GameObject.Find("BTN_Pause").GetComponent<Button>();
        //btnPause.onClick.AddListener(SetPauseBTN);

        SetCongratsBTN();
        SetSetting();
        ResetProgres();
        SetSoundValue();

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level01"))
            tutor = this.gameObject.GetComponent<Tutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckProggres();

        if (countDown > 0 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level01") && tutor.GetCountTutor == 5)
        {
            countDown -= Time.deltaTime;
            Timmer(countDown);
        }
        else if (countDown > 0 && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Level01"))
        {
            countDown -= Time.deltaTime; 
            Timmer(countDown);
        }

    }

    //Udah Bener Semua
    #region Setting 

    private void SetPauseBTN()
    {
        panelSetting.SetActive(true);
        Time.timeScale = 0;
        isOpened = true;
    }

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

                //Pause Button
                case 4:
                    btnSetting[4].GetComponent<Button>().onClick.AddListener(SetPauseBTN);
                    break;
            }
        }
        
        panelSetting.SetActive(false);


    }

    private void SetSoundValue()
    {
        bgmSound = this.GetComponent<AudioSource>();
        if (DataBase.GetAudio("BGM") == 0)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = false;
            bgmSound.mute = false;
            isMute = false;
        }
        else if (DataBase.GetAudio("BGM") == 1)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = true;
            bgmSound.mute = true;
            isMute = true;
        }
    }

    private void CheckBGMValue(bool value)
    {
        if(value == false)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = false;
            DataBase.SetAudio("BGM", 0);
            bgmSound.mute = false;
            isMute = false;
        }else if (value == true)
        {
            btnSetting[1].GetComponent<Toggle>().isOn = true;
            DataBase.SetAudio("BGM", 1);
            bgmSound.mute = true;
            isMute = true;
        }
    }

    private void ClosePanelSetting()
    {
        panelSetting.SetActive(false);
        Time.timeScale = 1;
        isOpened = false;
    }

    private void SaveProgress()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level01":
                DataBase.SetCurrentProgres("Level", 1);
                sfx_Sounds[0].Play();
                break;

            case "Level02":
                DataBase.SetCurrentProgres("Level", 2);
                sfx_Sounds[0].Play();
                break;

            case "Level03":
                DataBase.SetCurrentProgres("Level", 3);
                sfx_Sounds[0].Play();
                break;
        }
        DataBase.SetAudio("BGM", DataBase.GetAudio("BGM"));
    }

    private void BackToMainMenu()
    {
        //main Menu
        SceneManager.LoadScene("Menu");
    }

    #endregion

    #region GamePlay Progress

    private void SetCongratsBTN()
    {
        btnCongrats[0].GetComponent<Button>().onClick.AddListener(BackToMainMenu);
        btnCongrats[1].GetComponent<Button>().onClick.AddListener(NextLevel);
    }

    private void NextLevel()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level01":
                SceneManager.LoadScene("Level02");
                break;

            case "Level02":
                SceneManager.LoadScene("Level03");
                break;

            case "Level03":
                SceneManager.LoadScene("Menu");
                DataBase.SetCurrentProgres("Level",3);
                break;

            default:
                Debug.Log("Check Ur Key");
                break;
        }
    }

    private void CheckProggres()
    {
        //StartCoroutine(PlayCountdown(0));
        progresBox = DataBase.GetCurrentProgres("Box");
        txtUI[1].SetText(progresBox + "/" + maxBox);
        BoxFull(maxBox);
    }

    public void Timmer(float dispaly)
    {
        if (countDown <= 0.8)
        {
            countDown = 0.5f;
            if (progresBox == maxBox || progresBox >8)
            {
                DataBase.SetCurrentProgres("LevelStars1", 3);
                panelCongrats.SetActive(true);
                Time.timeScale = 0f;
                imgStar.sprite = _imgStars[0];
                isOpened = true;
                Debug.Log("3");
            }
            else if (progresBox <=8 && progresBox >= 5 )
            {
                DataBase.SetCurrentProgres("LevelStars1", 2);
                panelCongrats.SetActive(true);
                Time.timeScale = 0f;
                imgStar.sprite = _imgStars[1];
                isOpened = true;
                Debug.Log("2");
            }
            else if (progresBox < 5 && progresBox > 2)
            {
                DataBase.SetCurrentProgres("LevelStars1", 1);
                panelCongrats.SetActive(true);
                Time.timeScale = 0f;
                imgStar.sprite = _imgStars[2];
                isOpened = true;
                Debug.Log("1");
            }else if(progresBox == 0)
            {
                DataBase.SetCurrentProgres("LevelStars1", 0);
                panelCongrats.SetActive(true);
                Time.timeScale = 0f;
                imgStar.sprite = _imgStars[3];
                isOpened = true;
                Debug.Log("0");
            }
        }

        float minute = Mathf.FloorToInt(dispaly / 60);
        float second = Mathf.FloorToInt(dispaly % 60);

        txtUI[0].SetText(string.Format("{0:00}:{1:00}", minute, second));
    }

    private void BoxFull(int value)
    {
        if(progresBox == value)
        {
            DataBase.SetCurrentProgres("LevelStars1", 3);
            panelCongrats.SetActive(true);
            imgStar.sprite = _imgStars[0];
            isOpened = true;
            Debug.Log("3");
        }
    }

    private void ResetProgres()
    {
        txtUI[0].SetText("00:00");

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
                StartCoroutine(CountdownDelay(2));
                isOpened = false;
                break;

            case "Level03":
                DataBase.SetCurrentProgres("Level", 3);
                StartCoroutine(CountdownDelay(2));
                isOpened = false;
                break;

            default:
                Debug.Log("Check Ur Key");
                break;
        }
    }

    #endregion
    public void TaskGetOpen()
    {
        panelTask.SetActive(true);
    }

    public void TaskGetClose()
    {
        panelTask.SetActive(false);
    }

    IEnumerator SavedDelay(int value)
    {
        panelSaved.SetActive(true);
        yield return new WaitForSeconds(value);
        panelSaved.SetActive(false);
        Debug.LogWarning("SAVED");
    }

    IEnumerator CountdownDelay(int value)
    {
        isOpened = true;
        yield return new WaitForSeconds(value);
        isOpened = false;
        Timmer(countDown);

    }

    public float GetCountDown
    {
        get { return countDown; }
    }
}
