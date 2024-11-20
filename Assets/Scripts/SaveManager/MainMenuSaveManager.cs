using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSaveManager : MonoBehaviour
{
    public static MainMenuSaveManager Instance { get; set; }
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


    [System.Serializable]
    public class VolumeSetting
    {
        public float music;
        public float effects;
        public float master;
    }

    public void SaveVolumeSettings(float _music, float _effects,float _master)
    {
        VolumeSetting volumeSetting = new VolumeSetting()
        {
            music = _music,
            effects = _effects,
            master = _master
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSetting));
        PlayerPrefs.Save();

        print("Saved to Player Pref");
    }

    public VolumeSetting LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSetting>(PlayerPrefs.GetString("Volume"));
    }

    public float LoadMusicVolume()
    {
        var volumeSettings = JsonUtility.FromJson<VolumeSetting>(PlayerPrefs.GetString("Volume"));
        return volumeSettings.music;
    }
}
