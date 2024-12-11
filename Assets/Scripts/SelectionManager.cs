using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System; // Thêm thư viện TextMeshPro

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

    public GameObject selectedStorageBox;
    public GameObject selectedCampfire;

    public GameObject selectedSoil;

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

            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }



            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();


            if (interactable && interactable.playerInRange)
            {

                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;
            }

            StorageBox storageBox = selectionTransform.GetComponent<StorageBox>();

            if( storageBox && storageBox.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Open";
                interaction_Info_UI.SetActive(true);

                selectedStorageBox = storageBox.gameObject;

                if (Input.GetMouseButtonDown(0))
                {
                    StorageManager.Instance.OpenBox(storageBox);
                }
            }
            else
            {
                if(selectedStorageBox != null)
                {
                    selectedStorageBox = null;
                }
            }


            Campfire campfire = selectionTransform.GetComponent<Campfire>();

            if (campfire && campfire.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
            {
                interaction_text.text = "Interace";
                interaction_Info_UI.SetActive(true);

                selectedCampfire = campfire.gameObject;

                if (Input.GetMouseButtonDown(0) && campfire.isCooking == false)
                {
                    campfire.OpenUI();
                }
            }
            else
            {
                if (selectedCampfire != null)
                {
                    selectedCampfire = null;
                }
            }




            Animal animal = selectionTransform.GetComponent<Animal>();
            if(animal && animal.playerInRange)
            {
                if (animal.isDead)
                {
                    interaction_text.text = "Loot";
                    interaction_Info_UI.SetActive(true);
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handIsVisible = true;
                    if(Input.GetMouseButtonDown(0))
                    {
                      Lootable lootable = animal.GetComponent<Lootable>();
                        Loot(lootable);
                    }
                }
                else
                {
                    interaction_text.text = animal.animalName;
                    interaction_Info_UI.SetActive(true);

                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    handIsVisible = false;

                    if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon() && EquipSystem.Instance.IsThereASwinglock() == false)
                    {
                        StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));

                    }

                }
            }

            Soil soil = selectionTransform.GetComponent<Soil>();

            if (soil && soil.playerInRange )
            {
                if(soil.isEmpty && EquipSystem.Instance.IsPlayerHoldingSeed())
                {
                    string seedName = EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
                    string onlyPlantName = seedName.Split(new string[] { " Seed"}, StringSplitOptions.None)[0];
                    interaction_text.text = "Plant " + onlyPlantName;
                    interaction_Info_UI.SetActive(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        soil.PlantSeed();
                        Destroy(EquipSystem.Instance.selectedItem);
                        Destroy(EquipSystem.Instance.selectedItemModel);
                    }
                }

                else if(soil.isEmpty)
                {
                    interaction_text.text = "Soil";
                    interaction_Info_UI.SetActive(true);
                }
                else{
                    interaction_text.text = soil.plantName;
                    interaction_Info_UI.SetActive(true);
                }

                selectedSoil = soil.gameObject;
            }
            else
            {
                if (selectedSoil != null)
                {
                    selectedSoil = null;
                }
            }


            if(!interactable && !animal)
            {
                onTarget = false;
                handIsVisible = false;

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }

            if(!npc && !interactable && !animal && !choppableTree && !storageBox && !campfire && !soil)
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }
        }

    }

    private void Loot(Lootable lootable)
    {
        if(lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();
             foreach (LootPossibility loot in lootable.possibleLoot)
            {
                // 0 -> 1 (50% drop rate) 1/2 0,1
                // -1 -> 1 (30% drop rate) 1/3 -1, 0 , 1
                // -2 -> 1 (25% drop rate) 1/4 -1, -1 , 1
                // -3 -> 1 (20% drop rate) 1/5 -1, -2 , 1

                // -10 -> 1 () 1/12 8%

                // -3 -> 2 (1/6 1/6 2/6) -3, -2, -1, 0, 1(17%), 2(17%) (33% to get something)

                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax+1);
                if(lootAmount > 0)
                {
                    LootRecieved lt = new LootRecieved();
                    lt.item = loot.item;
                    lt.amount = lootAmount;
                    recievedLoot.Add(lt);
                }
            }

             lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }

        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;
        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecieved.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name +"_Model"), new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z ), Quaternion.Euler(0,0,0));
            }
        }

        if (lootable.GetComponent<Animal>())
        {
            lootable.GetComponent<Animal>().bloodPuddle.transform.SetParent(lootable.transform.parent);
        }

        Destroy(lootable.gameObject);
        // if(chest) {don't destroy}

    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
      yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
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


