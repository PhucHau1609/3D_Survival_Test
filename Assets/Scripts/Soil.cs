using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isEmpty = true;
    public bool playerInRange;
    public string plantName;

    public Plant currentPlant;

    public Material defaultMaterial;
    public Material wateredMaterial;
    
    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if(distance < 10f)
        {
            playerInRange = true;

        }
        else
        {
            playerInRange= false;
        }
    }

    internal void PlantSeed()
    {
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false;

        string onlyPlantName = selectedSeed.thisName.Split(new string[] { " Seed"}, StringSplitOptions.None)[0];
        plantName = onlyPlantName;

        //Instantiate Plant Prefab
        GameObject instantiatedPlant = Instantiate(Resources.Load($"{onlyPlantName}Plant") as GameObject);

        //Set the instantiated plant to be a child of the soil
        instantiatedPlant.transform.parent = gameObject.transform;

        //Make the plant's position in the middle of the soil
        Vector3 plantPosition = Vector3.zero;
        plantPosition.y = 0f;
        instantiatedPlant.transform.localPosition = plantPosition;

        //Set reference to the plant
        currentPlant = instantiatedPlant.GetComponent<Plant>();

        // set planting day
        currentPlant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }



    internal void MakeSoilWatered()
    {
        GetComponent<Renderer>().material = wateredMaterial;
    }
    internal void MakeSoilNotWatered()
    {
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
