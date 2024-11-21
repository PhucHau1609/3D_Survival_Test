using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button loadGameBTN;

    private void Start()
    {
        loadGameBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.StartLoadedGame();
        });
    }
    public void LoadSceneGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game Button Preesed");
        Application.Quit();
    }
}
