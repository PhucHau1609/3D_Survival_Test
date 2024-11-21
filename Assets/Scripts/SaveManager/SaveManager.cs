using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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

    // Json Project Save path
    string jsonPathProject;
    // Json external/real Save path
    string jsonPathPersistant;
    // Binary Save path
    string binaryPath;


    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
        binaryPath = Application.persistentDataPath + "/save_game.bin";
    }

    #region || ------------ General Section ------------

    #region || ------------  Saving  ------------
    public void SaveGame()
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        SavingTypeSwitch(data);
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

    public void SavingTypeSwitch(AllGameData gameData)
    {
        if(isSavingToJson)
        {
            SaveGameDataToJsonFile(gameData);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }
    #endregion

    #region || ------------ Loading ------------
    public AllGameData LoadingTypeSwitch()
    {
        if(isSavingToJson)
        {
            AllGameData gameData = LoadGameDataToJsonFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataToBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        //Player jsonString
        SetPlayerData(LoadingTypeSwitch().playerData);
        

        //Enviroment Data


    }

    private void SetPlayerData(PlayerData playerData)
    {
        // Kiểm tra Instance và playerBody
        if (PlayerState.Instance == null || PlayerState.Instance.playerBody == null)
        {
            Debug.LogError("PlayerState or playerBody is null. Cannot set player data.");
            return;
        }

        // Set player stats
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

        // Set player position
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        // Set player rotation
        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
    }


    public void StartLoadedGame()
    {
        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayLoading());
    }

    private IEnumerator DelayLoading()
    {
        yield return new WaitForSeconds(1);

        LoadGame();


    }

    #endregion

    #endregion

    #region || ------------ To Binary Section ---------------
    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        Debug.Log("Data saved to: " + binaryPath);
    }

    public AllGameData LoadGameDataToBinaryFile()
    {
     

        if(File.Exists(binaryPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();


            Debug.Log("Data Loaded from: " + binaryPath);


            return data;
        }
        else
        {
            return null;
        }
    }



    #endregion

    #region || ------------ To Json Section ---------------
    public void SaveGameDataToJsonFile(AllGameData gameData)
    {
       string json = JsonUtility.ToJson(gameData);

       string encryted = EncryptionDecryption(json);


        using (StreamWriter writer = new StreamWriter(jsonPathProject))
        {
            writer.Write(encryted);
            Debug.Log("Saved Gmae to Json file at: " + jsonPathProject);
        };
    }

    public AllGameData LoadGameDataToJsonFile()
    {
        using (StreamReader reader = new StreamReader(jsonPathProject))
        {
            string json = reader.ReadToEnd();

            string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted);
            return data;
        };
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

    #region || ----------- Encryption -----------
    
    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "1234567";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result;
    }

    #endregion
}
