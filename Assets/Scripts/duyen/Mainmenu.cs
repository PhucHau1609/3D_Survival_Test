using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Mainmenu : MonoBehaviour
{
    [SerializeField] UIDocument mainMenuDocument;

    private Button playButton;
    private Button LoadButton;
    private Button SettingButton;
    private Button HowtoplayButton;
    private Button ExitButton;

    public GameObject mainpanel;
    public GameObject settingpanel;
    public GameObject loadpanel;
    public GameObject hdpanel;
    private void OnEnable()
    {
        VisualElement root = mainMenuDocument.rootVisualElement;

        playButton = root.Q<Button>("NewButton");
        LoadButton = root.Q<Button>("LoadButton");
        SettingButton = root.Q<Button>("SettingsButton");
        HowtoplayButton = root.Q<Button>("HowtoPlayButton");
        ExitButton = root.Q<Button>("ExitButton");

        playButton.clickable.clicked += ShowNew;
        LoadButton.clickable.clicked += ShowLoad;
        SettingButton.clickable.clicked += ShowSet;
        HowtoplayButton.clickable.clicked += ShowHow;        
        ExitButton.clickable.clicked += Showexit;
    }
    private void ShowNew()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ShowLoad()
    {
        mainpanel.SetActive(false);
        settingpanel.SetActive(false);
        hdpanel.SetActive(false);
        loadpanel.SetActive(true);
    }

    private void ShowSet()
    {
        mainpanel.SetActive(false);
        hdpanel.SetActive(false);
        loadpanel.SetActive(false);
        settingpanel.SetActive(true);
    }

    private void ShowHow()
    {
        mainpanel.SetActive(false);
        loadpanel.SetActive(false);
        settingpanel.SetActive(false);
        hdpanel.SetActive(true);
    }

    private void Showexit()
    {
        Application.Quit();
    }
}
