using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class hdmenu : MonoBehaviour
{
    [SerializeField] private Button backButton;

    public GameObject mainpanel;
    public GameObject settingpanel;
    public GameObject loadpanel;
    public GameObject hdpanel;

    private void OnEnable()
    {

        if (backButton != null)
        {
            backButton.onClick.AddListener(Showback);
        }
        else
        {
            Debug.LogError("backButton không được tìm thấy!");
        }
    }
    private void Showback()
    {
        mainpanel.SetActive(true);
        hdpanel.SetActive(false);
    }
}
