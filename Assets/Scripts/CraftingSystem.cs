using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;

    //Craft Buttons
    Button craftAxeBTN, craftPlankBTN, craftFoundationBTN, craftWallBTN, craftStorageBoxBTN, craftBowBTN, craftFishingRodBTN, craftHoeBTN, craftCampfireBTN;

    //Requirement Text
    Text AxeReq1, AxeReq2, PlankReq1, FoundationReq1, WallReq1, StorageBoxReq1, BowReq1, FishingRodReq1, HoeReq1, HoeReq2, CampfireReq1, CampfireReq2;

    public bool isOpen;

    //All Blueprint
    private Blueprint AxeBLP;
    private Blueprint PlankBLP;
    private Blueprint FoundationBLP;
    private Blueprint WallBLP;
    private Blueprint StorageBoxBLP;
    private Blueprint BowBLP;
    private Blueprint FishingRodBLP;
    private Blueprint HoeBLP;
    private Blueprint CampfireBLP;


    public static CraftingSystem Instance {get; set;}

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
    // Start is called before the first frame update
    void Start()
    {
        AxeBLP = new Blueprint("Axe", 1, 2, "Stone", 3, "Stick", 3);
        PlankBLP = new Blueprint("Plank", 2, 1, "Log", 1, "", 0);
        FoundationBLP = new Blueprint("Foundation", 1, 1, "Plank", 4, "", 0);
        WallBLP = new Blueprint("Wall", 1, 1, "Plank", 2, "", 0);
        StorageBoxBLP = new Blueprint("StorageBox", 1, 1, "Plank", 2, "", 0);
        BowBLP = new Blueprint("Bow", 1, 1, "Stick", 3, "", 0);
        FishingRodBLP = new Blueprint("FishingRod", 1, 1, "Stick", 1, "", 0);
        HoeBLP = new Blueprint("Hoe", 1, 1, "Stone", 5, "Stick", 3);
        CampfireBLP = new Blueprint("Campfire",1, 1, "Stone", 5, "Stick", 5);


        isOpen = false;

        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate{OpenToolsCategory();});
        
        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate{OpenSurvivalCategory();});

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate{OpenRefineCategory();});

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate{OpenConstructionCategory();});

        //AXE
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("Req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("Req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate{CraftAnyItem(AxeBLP);});//

        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("Req1").GetComponent<Text>();

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate{CraftAnyItem(PlankBLP);});//

        //Foundation
        FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("Req1").GetComponent<Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate{CraftAnyItem(FoundationBLP);});//

        //Wall
        WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("Req1").GetComponent<Text>();

        craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate{CraftAnyItem(WallBLP);});//

        //Storage Box
        StorageBoxReq1 = survivalScreenUI.transform.Find("StorageBox").transform.Find("Req1").GetComponent<Text>();

        craftStorageBoxBTN = survivalScreenUI.transform.Find("StorageBox").transform.Find("Button").GetComponent<Button>();
        craftStorageBoxBTN.onClick.AddListener(delegate { CraftAnyItem(StorageBoxBLP); });//

        //BOW
        BowReq1 = toolsScreenUI.transform.Find("Bow").transform.Find("Req1").GetComponent<Text>();

        craftBowBTN = toolsScreenUI.transform.Find("Bow").transform.Find("Button").GetComponent<Button>();
        craftBowBTN.onClick.AddListener(delegate{CraftAnyItem(BowBLP);});//

        //FishingRod
        FishingRodReq1 = toolsScreenUI.transform.Find("FishingRod").transform.Find("Req1").GetComponent<Text>();

        craftFishingRodBTN = toolsScreenUI.transform.Find("FishingRod").transform.Find("Button").GetComponent<Button>();
        craftFishingRodBTN.onClick.AddListener(delegate{CraftAnyItem(FishingRodBLP);});//

        //Hoe
        HoeReq1 = toolsScreenUI.transform.Find("Hoe").transform.Find("Req1").GetComponent<Text>();
        HoeReq2 = toolsScreenUI.transform.Find("Hoe").transform.Find("Req2").GetComponent<Text>();

        craftHoeBTN = toolsScreenUI.transform.Find("Hoe").transform.Find("Button").GetComponent<Button>();
        craftHoeBTN.onClick.AddListener(delegate{CraftAnyItem(HoeBLP);});//

        //Campfire
        CampfireReq1 = survivalScreenUI.transform.Find("Campfire").transform.Find("Req1").GetComponent<Text>();
        CampfireReq2 = survivalScreenUI.transform.Find("Campfire").transform.Find("Req2").GetComponent<Text>();

        craftCampfireBTN = survivalScreenUI.transform.Find("Campfire").transform.Find("Button").GetComponent<Button>();
        craftCampfireBTN.onClick.AddListener(delegate{CraftAnyItem(CampfireBLP);});//

    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }

    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }

    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);
    }

    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(true);
    }

    
    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        
        StartCoroutine(craftedDelayForSound(blueprintToCraft));


        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
        }
        else if (blueprintToCraft.numOfRequirements ==2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }
        
        StartCoroutine(calculate());

    }
    public IEnumerator calculate()
    {
        // yield return new WaitForSeconds(1f);
        // InventorySystem.Instance.ReCalculateList();
        yield return 0;//So there is no delay
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();//Add this
    }
    IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < blueprintToCraft.numOfItemsToProduced; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName, true);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            MovementManager.Instance.EnableLook(false);

            craftingScreenUI.SetActive(true);
            craftingScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;

            RefreshNeededItems();

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen) //dang sua luc 4h10
        {
            MovementManager.Instance.EnableLook(true);

            CloseCraftingSystemUI();
        }
    }

    public void CloseCraftingSystemUI()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

        if (!InventorySystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

        }

        isOpen = false;
    }    

    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach(string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
                case "Log":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
            }
        }
        
        //---AXE---//
        AxeReq1.text = "3 Stone [" + stone_count + "]";
        AxeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }


        //---Plank---//
        PlankReq1.text = "1 Log [" + log_count + "]";

        if (log_count >= 1)
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }

        //---Foundation---//
        FoundationReq1.text = "4 Plank [" + plank_count + "]";

        if (plank_count >= 4)
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        //---Wall---//
        WallReq1.text = "2 Plank [" + plank_count + "]";

        if (plank_count >= 2)
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }

        //---Storage Box---//
        StorageBoxReq1.text = "2 Plank [" + plank_count + "]";

        if (plank_count >= 2)
        {
            craftStorageBoxBTN.gameObject.SetActive(true);
        }
        else
        {
            craftStorageBoxBTN.gameObject.SetActive(false);
        }

        //---BOW---//
        BowReq1.text = "3 Stick [" + stick_count + "]";

        if (stick_count >= 3)
        {
            craftBowBTN.gameObject.SetActive(true);
        }
        else
        {
            craftBowBTN.gameObject.SetActive(false);
        }

        //---FishingRod---//
        FishingRodReq1.text = "1 Stick [" + stick_count + "]";

        if (stick_count >= 1)
        {
            craftFishingRodBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFishingRodBTN.gameObject.SetActive(false);
        }

        //---Hoe---//
        HoeReq1.text = "5 Stone [" + stone_count + "]";
        HoeReq2.text = "3 Stick [" + stick_count + "]";

        if (stone_count >= 5 && stick_count >= 3)
        {
            craftHoeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftHoeBTN.gameObject.SetActive(false);
        }

        //---Campfire---//
        CampfireReq1.text = "5 Stone [" + stone_count + "]";
        CampfireReq2.text = "5 Stick [" + stick_count + "]";

        if (stone_count >= 5 && stick_count >= 5)
        {
            craftCampfireBTN.gameObject.SetActive(true);
        }
        else
        {
            craftCampfireBTN.gameObject.SetActive(false);
        }
    }
}
