using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeper : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    public GameObject shopkeeperDialogUI;
    public Button buyBTN;
    public Button sellBTN;
    public Button exitBTN;

    public GameObject buyPanelUI;
    public GameObject sellPanelUI;

    private void Start()
    {
        shopkeeperDialogUI.SetActive(false);

        buyBTN.onClick.AddListener(BuyMode);
        sellBTN.onClick.AddListener(SellMode);
        exitBTN.onClick.AddListener(StopTalking);
    }

    private void BuyMode()
    {
        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(true);

        HideDialogUI();
    }

    private void SellMode()
    {
        sellPanelUI.SetActive(true);
        buyPanelUI.SetActive(false);

        HideDialogUI();
    }

    private void DialogMode()
    {
        DisplayDialogUI();

        sellPanelUI.SetActive(false);
        buyPanelUI.SetActive(false);
    }

    public void Talk()
    {
        isTalkingWithPlayer = true;
        DisplayDialogUI();
    }

    public void StopTalking()
    {
        isTalkingWithPlayer = false;
        HideDialogUI();
    }

    private void DisplayDialogUI()
    {
        shopkeeperDialogUI.SetActive(true);
    }

    private void HideDialogUI()
    {
        shopkeeperDialogUI.SetActive(false);
    }

    #region || --- On Trigger Events --- ||
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    #endregion
}