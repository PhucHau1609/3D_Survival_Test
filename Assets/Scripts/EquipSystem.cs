using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();


    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject toolHolder;
    public GameObject selectedItemModel;

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


    private void Start()
    {
        PopulateSlotList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
    }

    public void SelectQuickSlot(int number)
    {

        if(checkIfSlotIsFull(number) == true)
        {

            if(selectedNumber != number)
            {
                selectedNumber = number;

                //Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);
                //Changing the color

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }


                Text toBeChanged = numbersHolder.transform.Find("number" + number).transform.Find("Text")
                    .GetComponent<Text>();

                toBeChanged.color = Color.white;
            }
            else //trying to select the same slot
            {
                selectedNumber = -1; // null

                //Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if(selectedItemModel !=null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                //Changing the color

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.gray;
                }
            }
            
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if(selectedItemModel !=null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }
        
        string selectedItemName = selectedItem.name.Replace("(Clone)","");
        // selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
        //     new Vector3(0.6f, 0, 0.4f), Quaternion.Euler(0, -12.5f, -20f));

        selectedItemModel = Instantiate(Resources.Load<GameObject>(CalculateItemModel(selectedItemName)));
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    private string CalculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "Axe":
                SoundManager.Instance.PlaySound(SoundManager.Instance.drawMetalicToolSound);
                return "Axe_Model";
            case "Bow":
                SoundManager.Instance.PlaySound(SoundManager.Instance.drawMetalicToolSound);
                return "Bow_Model";
            case "Hoe":
                SoundManager.Instance.PlaySound(SoundManager.Instance.drawMetalicToolSound);
                return "Hoe_Model";
            case "Tomato Seed":
                return "Hand_Model";
            case "Pumpkin Seed":
                return "Hand_Model";
            case "WateringCan":
                return "WateringCan_Model";
            case "FishingRod":
                return "FishingRod_Model";
            default:
                return null;
        }
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber-1].transform.GetChild(0).gameObject;
    }

    /*bool checkIfSlotIsFull(int slotNumber)
    {
        if(quickSlotsList[slotNumber].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    bool checkIfSlotIsFull(int slotNumber)
    {
        // Trừ 1 cho slotNumber vì mảng `quickSlotsList` bắt đầu từ 0
        int adjustedSlot = slotNumber - 1;
        if (adjustedSlot >= 0 && adjustedSlot < quickSlotsList.Count)
        {
            return quickSlotsList[adjustedSlot].transform.childCount > 0;
        }
        return false;
    }


    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        InventorySystem.Instance.ReCalculateList();

    }


    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if(selectedItem != null)
        {
            if(selectedItem.GetComponent<Weapon>() != null)
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
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
       if(selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;

        }
       else
        {
            return 0;
        }
    }

    public bool IsPlayerHoldingSeed()
    {
        if (selectedItemModel != null)
        {
            switch (selectedItemModel.gameObject.name)
            {
                case "Hand_Model(Clone)":
                    return true;
                case "Hand_Model":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsThereASwinglock()
    {
        if( selectedItemModel && selectedItemModel.GetComponent<EquipSystem>())
        {
            return selectedItemModel.GetComponent<EquipableItem>().swingWait;
        }
        else
        {
            return false;
        }

    }

    internal bool IsPlayerHoldingWateringCan()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.GetComponent<InventoryItem>().thisName)
            {
                case "WateringCan":
                    return true;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }

    
}