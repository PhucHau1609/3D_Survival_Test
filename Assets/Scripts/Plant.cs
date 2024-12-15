using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel;
    [SerializeField] GameObject youngPlantModel;
    [SerializeField] GameObject maturePlantModel;

    [SerializeField] List<GameObject> plantProduceSpawns;

    [SerializeField] GameObject producePrefab;

    public int dayOfPlanting;
    [SerializeField] int plantAge = 0; //depends on the watering frequency

    [SerializeField] int ageForYoungModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;

    [SerializeField] int daysForNewProduce; //Day it take for new fruit to grow after the initial batch;
    [SerializeField] int daysRemainingForNewProduceCounter;

    [SerializeField] bool isOneTimeHarvest;
    public bool isWatered; //Only if the plant is watered at the end of the day, it will "age";

    private void OnEnable()
    {
        TimeManager.Instance.OnDayPass.AddListener(DayPass);
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDayPass.RemoveListener(DayPass);
    }

    private void OnDestroy()
    {
        GetComponentInParent<Soil>().isEmpty = true;
        GetComponentInParent<Soil>().plantName = "";
        GetComponentInParent<Soil>().currentPlant = null;
    }

    private void DayPass()
    {
        if (isWatered)
        {
            plantAge++;

            isWatered = false;
            GetComponentInChildren<Soil>().MakeSoilNotWatered();
            GetComponent<SphereCollider>().enabled = false;
        }

        CheckGrowth();

        if (!isOneTimeHarvest)
        {
            CheckProduce();
        }
        

    }

    private void CheckGrowth()
    {
        seedModel.SetActive(plantAge < ageForYoungModel);
        youngPlantModel.SetActive(plantAge >= ageForYoungModel && plantAge < ageForMatureModel);
        maturePlantModel.SetActive(plantAge >= ageForMatureModel);

        if (plantAge >= ageForMatureModel && isOneTimeHarvest)
        {
            MakePlantPickable();
        }
    }

    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void CheckProduce()
    {
        if (plantAge == ageForFirstProduceBatch)
        {
            GenerateProduceForEmptySpawns();
        }

        if (plantAge > ageForFirstProduceBatch)
        {
            if (daysRemainingForNewProduceCounter == 0)
            {
                GenerateProduceForEmptySpawns();

                daysRemainingForNewProduceCounter = daysForNewProduce;
            }
            else
            {
                daysRemainingForNewProduceCounter--;
            }
        }        
    }

    private void GenerateProduceForEmptySpawns()
    {
        foreach (GameObject spawn in plantProduceSpawns)
        {
            if (spawn.transform.childCount == 0)
            {
                // Instantiate the produce from the prefab.
                GameObject produce = Instantiate(producePrefab);

                // Set the produce to be a child of the current spawn in the list.
                produce.transform.parent = spawn.transform;

                // Position the produce in the middle of the spawn
                Vector3 producePosition = Vector3.zero;
                producePosition.y = 0f;
                produce.transform.localPosition = producePosition;
            }
        }
    }
}
