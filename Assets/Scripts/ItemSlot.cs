using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{

    // public GameObject Item
    // {
    //     get
    //     {
    //         if (transform.childCount > 0)
    //         {
    //             return transform.GetChild(0).gameObject;
    //         }

    //         return null;
    //     }
    // }

    public void OnDrop(PointerEventData eventData)
    {
        //if there is not item already then set our item.
        if (transform.childCount <= 1)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot") == false)
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                InventorySystem.Instance.ReCalculateList();
            }

            if(transform.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                InventorySystem.Instance.ReCalculateList();
            }
        }
        else
        {
            InventoryItem draggedItem = DragDrop.itemBeingDragged.GetComponent<InventoryItem>();

            if (draggedItem.thisName == GetStoredItem().thisName 
                && IsLimitExceded(draggedItem) == false)
            {
                GetStoredItem().amountInInventory += draggedItem.amountInInventory;
                DestroyImmediate(DragDrop.itemBeingDragged);
            }
            else 
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
            }
        }
    }

    InventoryItem GetStoredItem()
    {
        return transform.GetChild(0).GetComponent<InventoryItem>();
    }

    bool IsLimitExceded(InventoryItem draggedItem)
    {
        if ((draggedItem.amountInInventory = GetStoredItem().amountInInventory) > InventorySystem.Instance.stackLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}