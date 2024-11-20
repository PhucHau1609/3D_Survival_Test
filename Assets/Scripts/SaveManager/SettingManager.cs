using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuSaveManager;
public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; set; }

    public Button backBTN;

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider musicSlider;
    public GameObject musicValue;

    public Slider effectsSlider;
    public GameObject effectsValue;


    private void Start()
    {
        backBTN.onClick.AddListener(() =>
        {
            MainMenuSaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
        });

        StartCoroutine(LoadAndApplySetting());
    }

    IEnumerator LoadAndApplySetting()
    {
        LoadAndSetVolume();

        //load graphicsetting or key bindinds

        yield return new WaitForSeconds(0.1f);
    } 
    
    private void LoadAndSetVolume()
    {
        VolumeSetting volumeSetting = MainMenuSaveManager.Instance.LoadVolumeSettings();

        masterSlider.value = volumeSetting.master;
        musicSlider.value = volumeSetting.music;
        effectsSlider.value = volumeSetting.effects;

        print("Volume setting are loaded");
    }    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }
}
