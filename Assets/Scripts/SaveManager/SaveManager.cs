using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
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

        DontDestroyOnLoad(gameObject);
    }

    public bool isSavingToJson;

    #region || ------------ General Section ---------------


    public void SaveGame()
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        SavedAllGameData(data);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydrationPercent;


        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.position.z;


        return new PlayerData(playerStats, playerPosAndRot);

    }

    public void SavedAllGameData(AllGameData gameData)
    {
        if(isSavingToJson)
        {
            //SaveGameDataToBinaryFile(gameData)
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }

    #endregion

    #region || ------------ To Binary Section ---------------
    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save_game.bin";
        FileStream stream = new FileStream(path,FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        Debug.Log("Data saved to: " + Application.persistentDataPath + "/save_game.bin");
    }

    public AllGameData LoadGameDataToBinaryFile()
    {
        string path = Application.persistentDataPath + "/save_game.bin";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }



    #endregion

    #region || ------------ Setting Section ---------------

    #region || ------------ Volume Settings ---------------
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
    #endregion


    #endregion
}
