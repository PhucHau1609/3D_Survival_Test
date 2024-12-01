using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Settingmenu : MonoBehaviour
{
    [SerializeField] UIDocument settingMenuDocument;

    private Slider slider1;
    private Slider slider2;
    private Slider slider3;
    private Button BackButton;
    private void Awake()
    {
        VisualElement root = settingMenuDocument.rootVisualElement;

        slider1 = root.Q<Slider>("slider1");
        slider2 = root.Q<Slider>("slider2");
        slider3 = root.Q<Slider>("slider3");

        BackButton = root.Q<Button>("bachbutton");

        BackButton.clickable.clicked += Showexit;

        // Gắn sự kiện cho Slider (nếu cần)
        slider1.RegisterValueChangedCallback(evt => Debug.Log($"Slider1 Value: {evt.newValue}"));
        slider2.RegisterValueChangedCallback(evt => Debug.Log($"Slider2 Value: {evt.newValue}"));
        slider3.RegisterValueChangedCallback(evt => Debug.Log($"Slider3 Value: {evt.newValue}"));
    }

    private void Showexit()
    {
        Application.Quit();
    }
}
