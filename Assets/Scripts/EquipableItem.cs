using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{ 
    public Animator animator;
    public bool swingWait = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && //left mouse button
        InventorySystem.Instance.isOpen == false && 
        CraftingSystem.Instance.isOpen == false && 
        SelectionManager.Instance.handIsVisible == false &&
        swingWait == false && 
        !ConstructionManager.Instance.inConstructionMode) //video 19 phat hienj thieu swingwait == false 
        {
            swingWait = true;
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
            StartCoroutine(NewSwingDelay());
        }

    }
    public void TreesGetHit() //GetHit in in video 15 -> rename TreesGetHit
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);

            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    IEnumerator NewSwingDelay()
    {
        yield return new WaitForSeconds(1f);
        swingWait = false;
    }
}
