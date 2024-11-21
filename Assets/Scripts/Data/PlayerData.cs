using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float[] playerStats;

    //public float playerHealth;

    public float[] playerPositionAndRotation;

    //public string[] inventoryCount;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot) 
    {
        playerStats = _playerStats;
        playerPositionAndRotation = _playerPosAndRot;
    }

}
