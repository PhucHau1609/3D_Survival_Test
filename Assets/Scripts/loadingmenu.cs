using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class loadmenu : MonoBehaviour
{
    [SerializeField] UIDocument loadMenuDocument;

    private Button playButton;
    private Button LoadButton;
    private Button SettingButton;
    private Button ExitButton;
    private void Awake()
    {
        VisualElement root = loadMenuDocument.rootVisualElement;

        playButton = root.Q<Button>("NewButton");
        LoadButton = root.Q<Button>("LoadButton");
        SettingButton = root.Q<Button>("SettingsButton");
        ExitButton = root.Q<Button>("ExitButton");

        playButton.clickable.clicked += ShowNew;
        LoadButton.clickable.clicked += ShowLoad;
        SettingButton.clickable.clicked += ShowSet;
        ExitButton.clickable.clicked += Showexit;
    }
    private void ShowNew()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void ShowLoad()
    {
        print("Showing load");
    }

    private void ShowSet()
    {
        print("Showing set");
    }

    private void Showexit()
    {
        Application.Quit();
    }
}
