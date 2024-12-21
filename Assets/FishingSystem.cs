using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public enum WaterSource
{
    Lake,
    River,
    Ocean
}

public class FishingSystem : MonoBehaviour
{
    public static FishingSystem Instance { get; set; }

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

    public List<FishData> lakeFishList; // cod
    public List<FishData> riverFishList; // salmon
    public List<FishData> oceanFishList; // tuna

    public bool isThereABite;
    bool hasPulled;

    public static event Action OnFishingEnd;

    public GameObject minigame;

    FishData fishBiting;

    public FishMovement fishMovement;

    internal void StartFishing(WaterSource waterSouce)
    {
        StartCoroutine(FishingCoroutine(waterSouce));
    }

    IEnumerator FishingCoroutine(WaterSource waterSouce)
    {
        yield return new WaitForSeconds(3f);
        FishData fish = CalculateBite(waterSouce);

        if (fish.fishName == "NoBite")
        {
            Debug.LogWarning("No fish caught");
            EndFishing();

        }
        else
        {
            Debug.LogWarning(fish.fishName + " is biting");
            StartCoroutine(StartFishStruggle(fish));
        }
    }

    IEnumerator StartFishStruggle(FishData fish)
    {
        isThereABite = true;
        // wait until player pulls the rod
        while (!hasPulled)
        {
            yield return null;
        }

        Debug.LogWarning("Start MiniGame");
        fishBiting = fish;
        StartMinigame();
    }

    private void StartMinigame()
    {
        minigame.SetActive(true);
        fishMovement.SetDifficulty(fishBiting);
    }

    public void SetHasPulled()
    {
        hasPulled = true;
    }

    private void EndFishing()
    {
        isThereABite = false;
        hasPulled = false;
        // Trigger end fishing event

        fishBiting = null;

        OnFishingEnd?.Invoke();

        // reset the Fishing rod model
        var slot = EquipSystem.Instance.selectedNumber;

        EquipSystem.Instance.SelectQuickSlot(slot);
        EquipSystem.Instance.SelectQuickSlot(slot);

    }

    private FishData CalculateBite(WaterSource waterSouce)
    {
        List<FishData> availableFish = GetAvailableFish(waterSouce);

        // Calculate total probability
        float totalProbability = 0f;
        foreach(FishData fish in availableFish) // tuna 5% (0-4) salmon 20% (5-24) nobite 10% (24-34) 
        {
            totalProbability += fish.probability;
        }

        // Generate random number between 0 and total probability
        int randomValue = UnityEngine.Random.Range(0, Mathf.FloorToInt(totalProbability) + 1); // 0 - 35 // 17
        Debug.Log("Random value is " + randomValue);

        // Loop through the fish and check if the random number falls into

        float cumulativeProbability = 0f;
        foreach(FishData fish in availableFish)
        {
            cumulativeProbability += fish.probability;
            if(randomValue <= cumulativeProbability)
            {
                return fish;
            }
        }
        // This should never happen

        return null;
    }

    private List<FishData> GetAvailableFish(WaterSource waterSouce)
    {
        switch (waterSouce)
        {
            case WaterSource.Lake:
                return lakeFishList;
            case WaterSource.River:
                return riverFishList;
            case WaterSource.Ocean:
                return oceanFishList;
            default:
                return null;
        }
    }

    internal void EndMinigame(bool success)
    {
        minigame.gameObject.SetActive(false);

        if (success)
        {           
            Debug.Log("Fish Caught");
            InventorySystem.Instance.AddToInventory(fishBiting.fishName);
            EndFishing();
        }
        else
        {
            Debug.Log("Fish Escaped");
            EndFishing();
        }
    }

}
