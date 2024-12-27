using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertDialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text messageText;
    public Button okButton;
    public Button cancelButton;

    private System.Action<bool> responceCallback;

    private void Start()
    {
        okButton.onClick.AddListener(()=> HandleResponse(true));
        cancelButton.onClick.AddListener(()=> HandleResponse(false));
    }

    public void ShowDialog(string message, System.Action<bool> callback)
    {
        responceCallback = callback;
        messageText.text = message;
        dialogBox.SetActive(true);
    }

    private void HandleResponse(bool response)
    {
        dialogBox.SetActive(false);
        responceCallback?.Invoke(response);
    }
}
