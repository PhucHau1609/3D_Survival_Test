using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public bool playerInRange;

    public bool isCooking;

    public float cookingTimer;

    public CookableFood foodBeingCooked;
    public string readyFood;

    public GameObject fire;
    public GameObject rawMeat;

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (distance < 10f)
        {
            playerInRange = true;

        }
        else
        {
            playerInRange = false;
        }

        if(isCooking)
        {
            cookingTimer -= Time.deltaTime;
            fire.SetActive(true);
            rawMeat.SetActive(true);
        }
        else
        {
            rawMeat.SetActive(false);
            fire.SetActive(false);
        }

        if(cookingTimer <= 0 && isCooking)
        {
            isCooking = false;
            readyFood = GetCookedFood(foodBeingCooked);
        }
    }

    private string GetCookedFood(CookableFood food)
    {
        return food.cookedFoodName;
    }

    public void OpenUI()
    {
        CampfireUIManger.Instance.OpenUI();
        CampfireUIManger.Instance.selectedCampfire = this;

        if(readyFood != "")
        {
            GameObject rf = Instantiate(Resources.Load<GameObject>(readyFood),
                CampfireUIManger.Instance.foodSlot.transform.position,
                CampfireUIManger.Instance.foodSlot.transform.rotation);

            rf.transform.SetParent(CampfireUIManger.Instance.foodSlot.transform);

            readyFood = "";

        }
    }

    public void StartCooking(InventoryItem food)
    {
        foodBeingCooked = ConvertIntoCookable(food);

        isCooking = true;

        cookingTimer = TimeToCookFood(foodBeingCooked);
    }

    private CookableFood ConvertIntoCookable(InventoryItem food)
    {
        foreach(CookableFood cookable in CampfireUIManger.Instance.cookingData.validFoods)
        {
            if(cookable.name == food.thisName)
            {
                return cookable;
            }
        }

        return new CookableFood();
    }

    private float TimeToCookFood(CookableFood food)
    {
        return food.timeToCook;
    }
}
