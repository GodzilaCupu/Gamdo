using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    GameStoryController controller;
    private string[] _text;
    private int countTutor;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button[] btnTutorial;
    [SerializeField] private Image ImageBtnnTutorial;
    [SerializeField] private Sprite[] _spitreBtnnTutorial;
    [SerializeField] private GameObject panelTutorial;

    private void Start()
    {
        SetTutorial();   
        controller = GameObject.Find("GameController").GetComponent<GameStoryController>();
        controller.isOpened = true;
    }

    private void Update()
    {
        CheckTutor();

        Debug.Log("Count : " + countTutor);
    }

    private void SetTutorial()
    {
        countTutor = 0;
        _text = new string[5];

        _text[0] = "Selamat Datang !! \n kamu dapat menggerakan pemain menggunakan tombol 'W' untuk bergerak maju, Tombol W bergerak kearah Depan, 'S' untuk bergerak ke arah belakang, 'D' untuk begerak kearah kanan, dan 'A' untuk bergerak kearah samping kiri, kamu juga dapat melompat dengan menekan tombol 'Space' ";
        _text[1] = "Untuk mengambil Box, kamu dapat menggunakan tombol 'G', untuk melempar menggunakan tombol 'Left Shift', dan menjatuhkan barang 'Left Control'";
        _text[2] = "Kamu harus mengumpulkan pada tempat yang telah di tentukan, tempat tersebut masing masing levelnya berbeda,\noleh karena itu perhatikan petunjuk yang telah tersedia dengan baik, untuk melihat detailnya silahkan tekan tombol '!' pada sisi layar ";
        _text[3] = "kamu dapat menekan tombol 'ESC' untuk berhenti sementara ";
        _text[4] = "Selamat Bermain <3 ";

        ImageBtnnTutorial = btnTutorial[1].gameObject.GetComponent<Image>();

        text.SetText(_text[0]);

        btnTutorial[0].onClick.AddListener(PrevTutor);
        btnTutorial[1].onClick.AddListener(NextTutor);
    }

    private void NextTutor()
    {
        countTutor++;
        text.SetText(_text[countTutor]);
    }

    private void PrevTutor()
    {
        countTutor--;
        text.SetText(_text[countTutor]);
    }

    private void SelesaiTutorial()
    {
        countTutor++;
        panelTutorial.SetActive(false);
        controller.isOpened = false;
    }

    private void CheckTutor()
    {
        if (countTutor == 0)
        {
            btnTutorial[0].interactable = false;
            btnTutorial[1].interactable = true;
            ImageBtnnTutorial.sprite = _spitreBtnnTutorial[0];
        }
        else if (countTutor > 0 && countTutor < 4)
        {
            btnTutorial[0].interactable = true;
            btnTutorial[1].interactable = true;
            ImageBtnnTutorial.sprite = _spitreBtnnTutorial[0];
        }
        else if (countTutor == 4)
        {
            btnTutorial[0].interactable = true;
            btnTutorial[1].interactable = true;
            ImageBtnnTutorial.sprite = _spitreBtnnTutorial[1];
        }

        if (countTutor == 4)
        {
            btnTutorial[1].onClick.RemoveAllListeners();
            btnTutorial[1].onClick.AddListener(SelesaiTutorial);
        }
        else if (countTutor < 4)
        {
            btnTutorial[1].onClick.RemoveAllListeners();
            btnTutorial[1].onClick.AddListener(NextTutor);
        }
    }

    public int GetCountTutor
    {
        get { return countTutor; }
    }
}
