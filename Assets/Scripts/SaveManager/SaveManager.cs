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


    string fileName = "SaveGame";


    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region || ------------ General Section ------------

    #region || ------------  Saving  ------------
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        SavingTypeSwitch(data, slotNumber);
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

    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if(isSavingToJson)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }
    #endregion

    #region || ------------ Loading ------------
    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if(isSavingToJson)
        {
            AllGameData gameData = LoadGameDataToJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataToBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        //Player jsonString
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);
        

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


    public void StartLoadedGame(int slotNumber)
    {
        SceneManager.LoadScene("GameScene");

        StartCoroutine(DelayLoading(slotNumber));
    }

    private IEnumerator DelayLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1);

        LoadGame(slotNumber);


    }

    #endregion

    #endregion

    #region || ------------ To Binary Section ---------------
    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        Debug.Log("Data saved to: " + binaryPath + fileName + slotNumber + ".bin");
    }

    public AllGameData LoadGameDataToBinaryFile(int slotNumber)
    {
     

        if(File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();


            Debug.Log("Data Loaded from: " + binaryPath + fileName + slotNumber + ".bin");


            return data;
        }
        else
        {
            return null;
        }
    }



    #endregion

    #region || ------------ To Json Section ---------------
    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
       string json = JsonUtility.ToJson(gameData);

       string encryted = EncryptionDecryption(json);


        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.Write(encryted);
            Debug.Log("Saved Gmae to Json file at: " + jsonPathProject + fileName + slotNumber + ".json");
        };
    }

    public AllGameData LoadGameDataToJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
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

    #region || ----------- Utility -----------

    public bool DoesFileExists(int slotNumber)
    {
        if(isSavingToJson)
        {
            if(System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json")) //SaveGame0.json
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExists(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    #endregion
}
