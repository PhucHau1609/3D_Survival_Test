using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FishData", menuName ="FishData", order = 1)]
public class FishData : ScriptableObject
{

    public string fishName;
    public GameObject inventroyItem;
    public int probability; // chance to catch fish in percentage % 20

    public int fishDifficulty;
}
