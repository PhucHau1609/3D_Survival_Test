using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isEmpty = true;
    public bool playerInRange;
    public string plantName;

    internal void PlantSeed()
    {
        InventoryItem selectedSeed = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false;
        string onlyPlantName = selectedSeed.thisName.Split(new string[] { " Seed"}, StringSplitOptions.None)[0];

        plantName = onlyPlantName;
    }

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
    
}
