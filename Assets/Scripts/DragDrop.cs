using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta; //canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var termpItemReference = itemBeingDragged;

        itemBeingDragged = null;

        if (termpItemReference.transform.parent == termpItemReference.transform.root)
        {
            termpItemReference.SetActive(false);

            AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();

            dialogManager.ShowDialog("Do you want to drop this item?", (response)=> {
                if (response)
                {
                    DropItemIntoTheWorld(termpItemReference);
                }
                else
                {
                    CancelDragging(termpItemReference);
                }
            });
        }

        if (termpItemReference.transform.parent == startParent)
        {
            CancelDragging(termpItemReference);
        }

        if (termpItemReference.transform.parent != termpItemReference.transform.root
            && termpItemReference.transform.parent != startParent)
        {
            if (termpItemReference.transform.parent.childCount > 2)
            {
                CancelDragging(termpItemReference);
                Debug.Log("Was not accepted into this slot");
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DivideStack(termpItemReference);
                }
                Debug.Log("Should be moved to another slot");
            }
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void DivideStack(GameObject termpItemReference)
    {
        InventoryItem item = termpItemReference.GetComponent<InventoryItem>();
        if (item.amountInInventory > 1)
        {
            item.amountInInventory -= 1;
            InventorySystem.Instance.AddToInventory(item.thisName, false);
        }
    }

    void CancelDragging(GameObject termpItemReference)
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        termpItemReference.SetActive(true);
    }

    private void DropItemIntoTheWorld(GameObject termpItemReference)
    {
        string cleanName = termpItemReference.name.Split(new string[] {"(Clone)"}, StringSplitOptions.None)[0];

        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        item.transform.position = Vector3.zero;
        var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
        item.transform.localPosition = new Vector3(dropSpawnPosition.x, dropSpawnPosition.y, dropSpawnPosition.z);

        var itemsObject = FindObjectOfType<EnviromentManager>().gameObject.transform.Find("[Items]");
        item.transform.SetParent(itemsObject.transform);

        DestroyImmediate(termpItemReference.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        
    }
}