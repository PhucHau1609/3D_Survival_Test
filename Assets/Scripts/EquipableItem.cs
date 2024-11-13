using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{ 
    public Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && //left mouse button
        InventorySystem.Instance.isOpen == false && 
        CraftingSystem.Instance.isOpen == false && 
        SelectionManager.Instance.handIsVisible == false)
        {          
            GameObject selectedTree = SelectionManager.Instance.selectedTree;
            
            if(selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().GetHit();
            }
            animator.SetTrigger("hit");
        }

    }
}
