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

    public GameObject mainpanel;
    public GameObject settingpanel;

    private void OnEnable()
    {
        VisualElement root = settingMenuDocument.rootVisualElement;

        slider1 = root.Q<Slider>("slider1");
        slider2 = root.Q<Slider>("slider2");
        slider3 = root.Q<Slider>("slider3");

        BackButton = root.Q<Button>("BackButton");

        BackButton.clickable.clicked += ShowBack;

        // Gắn sự kiện cho Slider (nếu cần)
        slider1.RegisterValueChangedCallback(evt => UpdateVolume("Master", evt.newValue));
        slider2.RegisterValueChangedCallback(evt => UpdateVolume("Music", evt.newValue));
        slider3.RegisterValueChangedCallback(evt => UpdateVolume("Effects", evt.newValue));

        // Tải các giá trị âm lượng từ PlayerPrefs
        LoadVolumeSettings();
    }

    private void ShowBack()
    {
        mainpanel.SetActive(true);
        settingpanel.SetActive(false);
    }

    private void UpdateVolume(string type, float value)
    {
        switch (type)
        {
            case "Master":
                AudioListener.volume = value;  // Cập nhật âm lượng master
                PlayerPrefs.SetFloat("MasterVolume", value);
                break;
            case "Music":
                // Cập nhật âm lượng nhạc, có thể sử dụng AudioSource cho nhạc
                // ví dụ: musicAudioSource.volume = value;
                PlayerPrefs.SetFloat("MusicVolume", value);
                break;
            case "Effects":
                // Cập nhật âm lượng hiệu ứng âm thanh, có thể sử dụng AudioSource cho các hiệu ứng
                // ví dụ: effectsAudioSource.volume = value;
                PlayerPrefs.SetFloat("EffectsVolume", value);
                break;
        }
    }

    private void LoadVolumeSettings()
    {
        // Tải các giá trị âm lượng từ PlayerPrefs
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f); // Mặc định là 1 nếu chưa lưu
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);   // Mặc định là 1 nếu chưa lưu
        float effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1f); // Mặc định là 1 nếu chưa lưu

        // Cập nhật các Slider với giá trị đã lưu
        slider1.value = masterVolume;
        slider2.value = musicVolume;
        slider3.value = effectsVolume;
    }
}
