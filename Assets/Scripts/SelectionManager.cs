using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Thêm thư viện TextMeshPro

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }
    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_Info_UI;
    TextMeshProUGUI interaction_text; // Sử dụng TextMeshProUGUI thay vì Text

    public Image centerDotImage;
    public Image handIcon;

    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;

    private void Start()
    {
        onTarget = false;

        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>(); // Lấy thành phần TextMeshProUGUI
    }

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

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            NPC npc = selectionTransform.GetComponent<NPC>();

            if(npc && npc.playerInRange)
            {
                interaction_text.text = "Talk";
                interaction_Info_UI.SetActive(true);

                if(Input.GetMouseButtonDown(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }

                if(DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(false);
                }
            }
            else
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }

            if(choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if(selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            if (interactable && interactable.playerInRange)
            {

                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if(interactable.CompareTag("pickable"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;
                }
                else
                {
                    handIcon.gameObject.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);

                    handIsVisible = false;
                }
            }

            else
            {
                onTarget = false;

                //interaction_Info_UI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                centerDotImage.gameObject.SetActive(true);

                handIsVisible = false;
            }
        }
        else
        {
            onTarget = false;

            interaction_Info_UI.SetActive(false);
            handIcon.gameObject.SetActive(false);
            centerDotImage.gameObject.SetActive(true);

            handIsVisible = false;
        }
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}