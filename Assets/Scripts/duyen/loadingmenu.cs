using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class loadmenu : MonoBehaviour
{
    [SerializeField] UIDocument loadMenuDocument;

    private Button backButton;
    private Button slot1;
    private Button slot2;
    private Button slot3;

    public GameObject mainpanel;
    public GameObject settingpanel;
    public GameObject loadpanel;
    public GameObject hdpanel;
    
    private void OnEnable()
    {
        VisualElement root = loadMenuDocument.rootVisualElement;

        backButton = root.Q<Button>("backButton");
        slot1 = root.Q<Button>("load1Button");
        slot2 = root.Q<Button>("load2Button");
        slot3 = root.Q<Button>("load3Button");

        if (backButton != null)
        {
            backButton.RegisterCallback<ClickEvent>(evt => Showback());
        }
        else
        {
            Debug.LogError("backButton không được tìm thấy!");
        }

        if (slot1 != null)
        {
            slot1.RegisterCallback<ClickEvent>(evt => LoadSlotData(1));
        }
        if (slot2 != null)
        {
            slot2.RegisterCallback<ClickEvent>(evt => LoadSlotData(2));
        }
        if (slot3 != null)
        {
            slot3.RegisterCallback<ClickEvent>(evt => LoadSlotData(3));
        }

    }

    private void Showback()
    {
        mainpanel.SetActive(true);
        settingpanel.SetActive(false);
        loadpanel.SetActive(false);
        hdpanel.SetActive(false);
    }

    private void LoadSlotData(int slotNumber)
    {
        // Kiểm tra xem slot có dữ liệu không
        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            Debug.Log("Slot " + slotNumber + " trống");
        }
        else
        {
            SaveManager.Instance.StartLoadedGame(slotNumber);
            mainpanel.SetActive(true);
            loadpanel.SetActive(false);
        }
    }
}
