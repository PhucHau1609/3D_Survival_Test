using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<InventorySlot> slotList = new List<InventorySlot>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private InventorySlot whatSlotToEquip;

    public bool isOpen;

    //public bool isFull;

    public TextMeshProUGUI currencyUI;

    //Pickup popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    public List<string> itemPickedup;

    public int stackLimit = 3;

    internal int currentCoins = 100;


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


    void Start()
    {
        isOpen = false;
        //isFull = false;

        PopulateSlotList();

        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                InventorySlot slot = child.GetComponent<InventorySlot>();
                slotList.Add(slot);
            }

        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            MovementManager.Instance.EnableLook(false);

            OpenUI();

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            MovementManager.Instance.EnableLook(true);

            CloseUI();
        }

        currencyUI.text = $"{currentCoins} Coins";
    }

    public void OpenUI()
    {
        inventoryScreenUI.SetActive(true);
        inventoryScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        isOpen = true;

        ReCalculateList();
    }

    public void CloseUI()
    {
        inventoryScreenUI.SetActive(false);

        if (!CraftingSystem.Instance.isOpen &&
            !StorageManager.Instance.storageUIOpen &&
            !CampfireUIManger.Instance.isUiOpen &&
            !BuySystem.Instance.ShopKeeper.isTalkingWithPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }

        isOpen = false;
    }

    public void AddToInventory(string itemName, bool shouldStack)
    {

        InventorySlot stack = CheckIfStackExists(itemName);

        if (stack != null && shouldStack)
        {
            stack.itemInSlot.amountInInventory += 1;
            stack.UpdateItemInSlot();
        }
        else
        {
            whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(itemName);
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

        QuestManager.Instance.RefreshTrackerList();
    }

    private InventorySlot CheckIfStackExists(string itemName)
    {
        foreach (InventorySlot inventorySlot in slotList)
        {
            inventorySlot.UpdateItemInSlot();
            if (inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                if (inventorySlot.itemInSlot.thisName == itemName
                    && inventorySlot.itemInSlot.amountInInventory < stackLimit)
                {
                    return inventorySlot;
                }
            }
        }
        return null;
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;

        pickupImage.sprite = itemSprite;

        StartCoroutine(HidePickupAlertAfterDelay(1f));
    }

    private IEnumerator HidePickupAlertAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);


        pickupAlert.SetActive(false);
    }

    private InventorySlot FindNextEmptySlot()
    {
        foreach (InventorySlot slot in slotList)
        {
            if (slot.transform.childCount <= 1)
            {
                return slot;
            }
        }
        return new InventorySlot();
    }

    public bool CheckSlotsAvailable(int emptyMeeded)
    {
        int emptySlot = 0;
        foreach (InventorySlot slot in slotList)
        {
            if (slot.transform.childCount > 21)
            {
                emptySlot += 1;
            }
        }
        if (emptySlot == emptyMeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string itemName, int amountToRemove)
    {
        int remainingAmountToRemove = amountToRemove;
        while (remainingAmountToRemove != 0)
        {
            int previousRemainingAmount = remainingAmountToRemove;

            foreach (InventorySlot slot in slotList)
            {
                if (slot.itemInSlot != null && slot.itemInSlot.thisName == itemName)
                {
                    slot.itemInSlot.amountInInventory--;
                    remainingAmountToRemove--;

                    if (slot.itemInSlot.amountInInventory == 0)
                    {
                        Destroy(slot.itemInSlot.gameObject);
                        slot.itemInSlot = null;
                    }
                    break;
                }
            }

            if (previousRemainingAmount == remainingAmountToRemove)
            {
                Debug.Log("Item not found or insufficient quantity in inventory");
                break;
            }

            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
            QuestManager.Instance.RefreshTrackerList();
        }
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (InventorySlot inventorySlot in slotList)
        {
            InventoryItem item = inventorySlot.itemInSlot;

            if (item != null)
            {
                if (item.amountInInventory > 0)
                {
                    for (int i = 0; i < item.amountInInventory; i++)
                    {
                        itemList.Add(item.thisName);
                    }
                }
            }
        }
    }

    public int CheckItemAmount(string name)
    {
        int itemCounter = 0;

        foreach (string item in itemList)
        {
            if (item == name)
            {
                itemCounter++;
            }

        }
        return itemCounter;
    }

}
