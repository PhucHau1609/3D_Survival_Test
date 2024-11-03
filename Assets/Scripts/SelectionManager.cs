using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Thêm thư viện TextMeshPro

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text; // Sử dụng TextMeshProUGUI thay vì Text

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>(); // Lấy thành phần TextMeshProUGUI
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            var interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable != null) // Kiểm tra nếu interactable khác null
            {
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                interaction_Info_UI.SetActive(false);
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }
}
